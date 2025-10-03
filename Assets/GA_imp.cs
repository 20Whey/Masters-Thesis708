using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GA_namespce;
using static GA_namespce.GA;
using Random = UnityEngine.Random;

public class GA_imp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Node;
    public int max_sim_number;
    public List<GA_Agent> population;
    public GameObject rt;
    public GA_Agent root;
    public bool create_elites;
    public bool deploy;
    public bool start;

    public struct GA_loop_data_object
    {
        public bool should_continue;
        public double old_fitness;
        public double fitness;
        public int failure_count;
    }

    //wait it's a linked list/pool bruhh
   



    //  var sortedDict = mx.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

    public List<GA_Agent> create_initial_population(int starting_population_size)
    {
        List<GA_Agent> population = new List<GA_Agent>();
        root = new GA_Agent(0, null, rt);
        for (int i = 0; i < starting_population_size; i++)
        {
            GA_Agent agent = new(i, root, Node);
            agent.prepare_for_operations();
            agent.mixup();
            agent.rebind_costs(agent.exposed_costs);
            population.Add(agent);
        }
        return population;
    }


    public List<GA_Agent> return_elite_agents_via_time_fitness(List<GA_Agent> pop)
    {
      List<GA_Agent> elites = pop.OrderBy(item => item.character.timer).Take(max_sim_number / 10).ToList();
      for (int i = 0; i < elites.Count; i++)
      {
          elites[i].character.plan.ForEach(item => elites[i].plan.Add(item.Name));
      }
      return elites;
    }

    public double evaluate_fitness(List<GA_Agent> elites)
    {
        double global_fitness = 0f;
        elites.ForEach(item => global_fitness += item.character.timer);
        return global_fitness/elites.Count;
    }
    
    public void simulate(List<GA_Agent> pop)
    {
        int index = 0;
        do
        {
            GA_Agent item = pop[index];
            var gm = Instantiate(Node, new Vector3(item.id * 8,  1f, 0f), Quaternion.identity);
            var comp = gm.GetComponent<setup>();
            item.character = comp.basic_character;
            comp.id = item.id;
            comp.GA_reference = item;
            for (var i = 0; i < item.wrapped_costs.Length; i++)
            {
                comp.weights[i].name = item.wrapped_costs[i].Item1;
                comp.weights[i].value = item.wrapped_costs[i].Item2;
            }
            comp.run_plan = true;
            index += 1;
        } while (index < pop.Count);
        Debug.Log(pop.Count);
    }
    
    private List<GA_Agent> re_populate(List<GA_Agent> current_elites, List<GA_Agent> cpop)
    {
        int c_id = 0;
        do
        {
            //imp id tracking
            GA_Agent curr = new GA_Agent(c_id, root, Node);
            curr.copy_allowed_actions(current_elites.First());
            GA_Agent curr2 = new GA_Agent(c_id+1, root, Node);
            curr2.copy_allowed_actions(current_elites.First());
            var itm = create_deviants(current_elites[Random.Range(0, current_elites.Count - 1)],
            current_elites[Random.Range(0, current_elites.Count - 1)]);
            curr.prepare_for_operations();
            curr2.prepare_for_operations();
            curr.drag_drop_wrapped_costs(itm.Item1);
            curr2.drag_drop_wrapped_costs(itm.Item2);
            cpop.Add(curr);
            cpop.Add(curr2);
            c_id += 2;
        } while (cpop.Count < max_sim_number);
        return cpop;
    }

    public void clean_pop(List<GA_Agent> population, List<GA_Agent> elites)
    {
        for (var i = 0; i < population.Count; i++)
        {
            if (!elites.Contains(population[i])) population[i].character.destroy_self = true;
        }
    }
    void Update()
    {
        
        if (start)
        {
            start = false;
            population = create_initial_population(max_sim_number);
            simulate(population);
        }

        if (deploy)
        {
            //GA LOOP
            deploy = false;
            StartCoroutine(GALOOP());
        }
    }
//IF WE STOP SEEING IMPROVEMENT AFTER FIVE CYCLES, (fitness value stops lowering) we stop.
        IEnumerator GALOOP(float delay = 10f)
        {
            GA_loop_data_object should_continue(GA_loop_data_object obj)
            {
                if (obj.failure_count > 5)
                {
                    obj.fitness = obj.old_fitness;
                    obj.should_continue = false;
                }
                else
                {
                    if (obj.fitness > obj.old_fitness)
                    {
                        obj.failure_count  +=1;
                        obj.fitness = obj.old_fitness;
                        obj.should_continue = true;
                    }
                    else
                    {
                        obj.failure_count = 0;
                        obj.old_fitness = obj.fitness;
                        obj.should_continue = true;
                    }
                }
                return obj;
            }
            GA_loop_data_object current_container = new GA_loop_data_object();
            //struct if it works
            population = create_initial_population(max_sim_number);
            simulate(population);
            //give time for it to simulate.
            yield return new WaitForSeconds(delay);
            List<GA_Agent> elites = return_elite_agents_via_time_fitness(population);
            clean_pop(population, elites);
            current_container.failure_count = 0;
            current_container.old_fitness = Math.Round(evaluate_fitness(elites), 1);
            //starting fitness
            Debug.Log(current_container.old_fitness + " "+  string.Join(",",elites.First().plan.ToArray()));
            current_container.fitness = current_container.old_fitness;
            do
            {
                //this repopulates and mutates
                population = re_populate(elites, new List<GA_Agent>());
                //run
                simulate(population);
                //wait for imp
                yield return new WaitForSeconds(delay);
                //rank individuals
                elites = return_elite_agents_via_time_fitness(population);
                //clean up 
                clean_pop(population, elites);
                current_container.fitness = Math.Round(evaluate_fitness(elites), 2);
                //assign fitness to population
                current_container = should_continue(current_container);
               Debug.Log(current_container.fitness + " " + current_container.failure_count + " "+  string.Join(",",elites.First().plan.ToArray()));
               
            } while (current_container.should_continue);
            yield return null;
        }

    }

      

