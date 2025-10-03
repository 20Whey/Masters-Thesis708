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
    public List<GA_Agent> construct_tree(int pop_size)
    {
        Queue<GA_Agent> queue = new Queue<GA_Agent>();
        List<GA_Agent> visited = new List<GA_Agent>();

        int count = 0;
        root = new GA_Agent(-1, null, rt);
        root.prepare_for_operations();
        queue.Enqueue(root);
        do
        {
            count++;
            var parent = queue.Dequeue();
            visited.Add(parent);


            //LOOP CONTENTS
            //  GA_Agent new_agent = new GA_Agent(Node, count).copy_allowed_actions();
            //this already rebinds costs;
            //    new_agent.prepare_for_operations();
            //     create_deviants(new_agent, parent);
            //     queue.Enqueue(new_agent);


        } while (max_sim_number > pop_size);

        Debug.Log(count);
        return visited;
    }



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


    /*   public List<GA_Agent> grab_elites(int starting_elites_size, List<GA_Agent> population)
       {
           //Some sort of linq devilry, find every unique plan combination in the list


       }*/


    //CREATE AGENT FROM ELITES 
    //  GA_Agent new_agent = new GA_Agent(Node, count).copy_allowed_actions();
    //this already rebinds costs;
    //    new_agent.prepare_for_operations();
    //     create_deviants(new_agent, parent);
    //     queue.Enqueue(new_agent);

    /*   public List<GA_Agent> discover_variants()
       {

       }
*/
    public List<GA_Agent> grab_all_plans_and_return_uniques_as_elites(List<GA_Agent> pop)
    {
        void is_unique(List<GA_Agent> unique_sequence, GA_Agent input)
        {
            if (unique_sequence.Count == 0)
            {
                unique_sequence.Add(input);
                return;
            }
            for (var c = 0; c < unique_sequence.Count; c++)
            {
                if (input.plan.SequenceEqual(unique_sequence[c].plan))
                {
                    return;
                }
            }
            unique_sequence.Add(input);
        }

        List<GA_Agent> unique_entries = new List<GA_Agent>();
        List<GA_Agent> all_plans = new List<GA_Agent>();
        foreach (var item in pop)
        {
            var c = item.id;

            for (int a = 0; a < item.character.plan.Count; a++)
            {
                item.plan.Add(item.character.plan[a].Name);
            }
            all_plans.Add(item);
        }
        //build unique list
        var i = 0;
        while (i < all_plans.Count)
        {
            is_unique(unique_entries, all_plans[i]);
            i++;
        }
        Debug.Log(unique_entries.Count);
        return unique_entries;
    }

    public List<GA_Agent> return_elite_agents_via_time_fitness(List<GA_Agent> pop)
    {
        return pop.OrderBy(item => item.character.timer).Take(max_sim_number / 10).ToList();
        ;
    }


    public float evaluate_fitness(List<GA_Agent> elites)
    {
        float global_fitness = 0f;
        elites.ForEach(item => global_fitness += item.fitness);
        return global_fitness;

    }


    public void simulate(List<GA_Agent> pop)
    {
        int index = 0;
        do
        {
            GA_Agent item = pop[index];
            var gm = Instantiate(Node, new Vector3(item.id * 5, 0f, 0f), Quaternion.identity);
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

    private void prepare_agent(params GA_Agent[] agents)
    {
        for (var i = 0; i < agents.Length; i++)
        {
            agents[i].prepare_for_operations();



        }

    }

    private List<GA_Agent> re_populate(List<GA_Agent> current_elites, List<GA_Agent> cpop)
    {
        do
        {
            //imp id tracking
            GA_Agent curr = new GA_Agent(0, root, Node);
            curr.copy_allowed_actions(current_elites.First());
            GA_Agent curr2 = new GA_Agent(0, root, Node);
            curr2.copy_allowed_actions(current_elites.First());
            var itm = create_deviants(current_elites[Random.Range(0, current_elites.Count - 1)],
            current_elites[Random.Range(0, current_elites.Count - 1)]);
            curr.prepare_for_operations();
            curr2.prepare_for_operations();
            curr.wrapped_costs = itm.Item1;
            curr2.wrapped_costs = itm.Item2;
            cpop.Add(curr);
            cpop.Add(curr2);
        } while (cpop.Count < max_sim_number);
        return cpop;
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
            deploy = false;
            //GA LOOP

            StartCoroutine(GALOOP());

        }


        GA_loop_data_object should_continue(double fitness, double old_fitness, int failure_count)
        {
            GA_loop_data_object return_val = new GA_loop_data_object();

            if (failure_count > 5)
            {
                return_val.fitness = old_fitness;
                return_val.should_continue = false;
            }
            else
            {
                if (fitness > old_fitness)
                {
                    return_val.failure_count += 1;
                    return_val.fitness = old_fitness;
                    return_val.should_continue = true;
                }
                else
                {
                    return_val.failure_count = 0;
                    return_val.old_fitness = fitness;
                    return_val.should_continue = true;
                }
            }
            return return_val;
        }

//IF WE STOP SEEING IMPROVEMENT AFTER FIVE CYCLES, (fitness value stops lowering) we stop.
        IEnumerator GALOOP()
        {
            //BADBADBADBAD
            //struct if it works
            population = create_initial_population(max_sim_number);
            simulate(population);
            List<GA_Agent> elites = return_elite_agents_via_time_fitness(population);
            int failure_count = 0;
            double old_fitness = Math.Round(evaluate_fitness(elites), 4);
            //starting fitness
            double fitness = old_fitness;
            {
                //rank individuals, I ought to weight it.

                //this repopulates and mutates
                population = re_populate(elites, new List<GA_Agent>());
                simulate(population);
                elites = return_elite_agents_via_time_fitness(population);
                yield return new WaitForSeconds(10f);
                fitness = Math.Round(evaluate_fitness(elites), 4);
            }
            while (should_continue(fitness, old_fitness, failure_count).should_continue) ;

            yield return null;


        }

    }
}
