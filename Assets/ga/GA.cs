
using System.Collections.Generic;
using System.Linq;
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
        public List<Action> allowed_actions;
        public (string, float)?[] exposed_costs;
        public (string, float)[] wrapped_costs;
        public List<(string, float)> exposed_Immutable_costs;
        public float fitness;
        //first_time_setup
        public GA_Agent(GameObject Node, int id)
        {
            allowed_actions = new  List<Action>();
            character = Node.GetComponent<setup>().basic_character;
            //bug
            allowed_actions = character.actions.return_actions(); 
            
            exposed_costs = new (string, float)?[allowed_actions.Count];
            exposed_Immutable_costs = new List<(string, float)>();
            fitness = 0f;
            this.id = id;
            
            
        }

       //get actions
        public void prepare_for_operations()
        {
            for (var i = 0; i < allowed_actions.Count; i++)
            {
                var curr_acc = allowed_actions.ElementAt(i);
              //  allowed_actions.Add(curr_acc);
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
        public GA_Agent create_from(GA_Agent other)
        {
            this.allowed_actions = other.allowed_actions;
            this.prepare_for_operations();
            return this;
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
        
       public static(string, float)[] create_deviants(GA_Agent first_agent, GA_Agent other_agent)
       {
           (string, float)[] random_simple_crossover(GA_Agent first_agent, GA_Agent other_agent)
           {
               //FIX ME
               var rnd = Random.Range(0, first_agent.exposed_costs.Length);
               var cost_one = first_agent.exposed_costs.Take(rnd).ToArray(); 
               var cost_two = other_agent.exposed_costs.Skip(rnd).ToArray();
               
               var combined =  cost_one.Concat(cost_two).ToArray();
              //Silly Mutation
               for (var i = 0; i < combined.Length; i++)
               {
                  if (combined[i] != null)
                    if (Random.Range(0, 10) > 8) 
                      combined[i] = Random.Range(0, 1) == 0 ? (combined[i].Value.Item1, combined[i].Value.Item2 + 0.125f) : (combined[i].Value.Item1, combined[i].Value.Item2 -0.125f);
               }
               return first_agent.rebind_costs(combined);
           }
           //lobotomise it rq
            
          // GA_Agent new_child = new GA_Agent().create_from(first_agent);
           return Random.Range(0, 1) == 0 ? random_simple_crossover(first_agent, other_agent) : random_simple_crossover(other_agent,first_agent);
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






