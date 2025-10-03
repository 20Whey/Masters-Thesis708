
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Action = Production.Action;
using Random = UnityEngine.Random;

namespace GA_namespce
{
    
    public class GA_Agent
    {
        public basic_character character;
        public int id;
        public GameObject self;
        public List<string> plan;
        public List<Action> allowed_actions;
        public (string, float)?[] exposed_costs;
        public (string, float)[] wrapped_costs;
        public List<(string, float)> exposed_Immutable_costs;
        public float fitness; //uniqueness
        //first_time_setup
        public GA_Agent(int id, [CanBeNull] GA_Agent root, GameObject Node /*predefined and created lil guy*/)
        {
            //basic
            plan = new List<string>();
            allowed_actions = new List<Action>();
            if (root == null)
            {
                character = Node.GetComponent<setup>().basic_character;
                allowed_actions = character.allowed_actions;
            }
            else
            {
                allowed_actions = root.character.actions.return_actions();
            }
            self = Node;
            exposed_costs = new (string, float)?[allowed_actions.Count];
            //bug
            exposed_Immutable_costs = new List<(string, float)>();
            fitness = 0f;
            this.id = id;
        }
       

        public void more_complex_init(GA_Agent agent)
        {
            allowed_actions = character.actions.return_actions(); 
            exposed_costs = new (string, float)?[allowed_actions.Count];
        }
       //get actions
       
        public void prepare_for_operations()
        {
            for (var i = 0; i < allowed_actions.Count; i++)
            {
                var curr_acc = allowed_actions.ElementAt(i);
                //allowed_actions.Add(curr_acc);
                if (curr_acc.is_mutable)
                {
                    exposed_costs[i] = (curr_acc.Name, curr_acc.Cost);
                }
                else
                {
                    exposed_costs[i] = null;
                    exposed_Immutable_costs.Add((curr_acc.Name, curr_acc.Cost));
                }
            }
        }

        public void mixup()
        {
            for (var i = 0; i < exposed_costs.Length; i++)
            {
                if (exposed_costs[i] != null)
                {
                    var val = exposed_costs[i].Value.Item2;
                    exposed_costs[i] = (exposed_costs[i].Value.Item1, val +=  Random.Range(-0.3f, 0.3f));
                }
            }
        }
        public void copy_allowed_actions(GA_Agent other)
        {
            this.allowed_actions = other.allowed_actions;
        }

        public void drag_drop_wrapped_costs((string, float)[] costs)
        {
            this.wrapped_costs = costs;
        }
        
        //for after mutation
        public (string, float)[] rebind_costs((string, float)?[]  exposed_costs)
        {
            wrapped_costs = new (string, float)[exposed_costs.Length];
            int n = 0;
            for (var i = 0; i < exposed_costs.Length; i++){
                if (exposed_costs[i] == null)
                {
                    //fill from exposed immutables
                    wrapped_costs[i] = exposed_Immutable_costs[n];
                    n++;
                }
                else
                {
                    wrapped_costs[i] =  (exposed_costs[i].Value.Item1, exposed_costs[i].Value.Item2);
                }
            }
            return wrapped_costs;
        }
    }
    
    public class GA
    {
        
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
        
        
        
       public static ((string, float)[], (string, float)[]) create_deviants(GA_Agent first_agent, GA_Agent other_agent)
       {
           ((string, float)[], (string, float)[]) random_simple_crossover(GA_Agent first_agent, GA_Agent other_agent)
           {
               (string, float)[] mutation_and_combination(GA_Agent agent, (string, float)?[] combined)
               {
                   for (var i = 0; i < combined.Length; i++)
                   {
                       if (combined[i] != null)
                           if (Random.Range(0, 10) > 6)
                               combined[i] = Random.Range(0, 1) == 0
                               ? (combined[i].Value.Item1, combined[i].Value.Item2 + 0.25f)
                               : (combined[i].Value.Item1, combined[i].Value.Item2 - 0.25f);
                   }
                   return agent.rebind_costs(combined);
               }
               //FIX ME
               var rnd = Random.Range(0, first_agent.exposed_costs.Length);
               var cost_one = first_agent.exposed_costs.Take(rnd).ToArray(); 
               var cost_two = other_agent.exposed_costs.Skip(rnd).ToArray();
               
               var combined =  cost_one.Concat(cost_two).ToArray();
               var combined2 = cost_two.Concat(cost_one).ToArray();
              //Silly Mutation
              return (mutation_and_combination(first_agent,combined), mutation_and_combination(other_agent,combined2));
           }
           //lobotomise it rq
            
          // GA_Agent new_child = new GA_Agent().copy_allowed_actions(first_agent);
            //DUE TO VISUAL BUG, WORKAROUND:  
           return random_simple_crossover(first_agent, other_agent);
       }

       
       
       




        
        
        
        
        
        
        
        
        
        
        
        
        
    }

    
    
    
    
    
    /*
        public class GA_Node
        {


        }



        using UnityEngine;
        public class genetic
        {

            public float harmonic_mean( params float[] fitness)
            {
                return new float();
            }



            public GA_Node crossover(GA_Node a, GA_Node b)
            {

            }


            }


            */




}






