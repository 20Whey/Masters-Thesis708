using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GA_namespce;
using static GA_namespce.GA;

public class GA_imp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Node;
    public int max_sim_number;
    public List<GA_Agent> population;
    public GA_Agent root;
    public bool create_elites;
    public bool deploy;
    public bool start;

    public List<GA_Agent> elites;
    
        //wait it's a linked list/pool bruhh
        public List<GA_Agent> construct_tree(int pop_size){
            Queue<GA_Agent> queue = new Queue<GA_Agent>();
            List<GA_Agent> visited = new List<GA_Agent>();
            
            int count = 0;
            root = new GA_Agent(Node, 0);
            root.prepare_for_operations();
            queue.Enqueue(root);
            do
            {
                count++;
                var parent = queue.Dequeue();
                visited.Add(parent);
                
               
                    //LOOP CONTENTS
                  //  GA_Agent new_agent = new GA_Agent(Node, count).create_from();
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
            for (int i = 0; i < starting_population_size; i++)
            {
                GA_Agent agent = new GA_Agent(Node, i);
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
        //  GA_Agent new_agent = new GA_Agent(Node, count).create_from();
        //this already rebinds costs;
        //    new_agent.prepare_for_operations();
        //     create_deviants(new_agent, parent);
        //     queue.Enqueue(new_agent);

     /*   public List<GA_Agent> discover_variants()
        {
            
        }
*/        public List<GA_Agent> grab_all_plans_and_return_uniques_as_elites(List<GA_Agent> population)
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
            
            foreach (var item in population)
            {
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


    
        void Update()
        {
            if (start) 
            {
                population = create_initial_population(100);
                start = false;
            }

            if (deploy)
            {
                foreach (var item in population)
                {
                    var gm = Instantiate(Node, new Vector3(item.id*5,0f,0f), Quaternion.identity);
                    var comp = gm.GetComponent<setup>();
                    comp.id = item.id;
                    
                    for (var i = 0; i < item.wrapped_costs.Length; i++)
                    {
                        //Debug.Log(itm.Item1 +" "+ itm.Item2);  itm.Item2
                        comp.weights[i].name = item.wrapped_costs[i].Item1;
                        comp.weights[i].value = item.wrapped_costs[i].Item2;
                    }
                }
                
            deploy = false;
            }

            if (create_elites)
            {
                grab_all_plans_and_return_uniques_as_elites(population);
                create_elites = false;
            }









        }
       



        
        
    

}