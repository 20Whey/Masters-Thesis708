using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
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
        public List<(string, float)> exposed_Immutable_costs;
        public float fitness;
        //first_time_setup
        public GA_Agent(basic_character character, int id)
        {
            allowed_actions = new List<Action>();
            exposed_costs = new (string, float)?[allowed_actions.Count];
            exposed_Immutable_costs = new List<(string, float)>();
            fitness = 0f;
            this.id = id;
            this.character = character;
        }

        public void prepare_for_operations()
        {
            for (var i = 0; i < character.allowed_actions.Count; i++)
            {
                var curr_acc = character.allowed_actions[i];
                allowed_actions.Add(curr_acc);
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


            return this;
        }
        
        //for after mutation
        public  (string, float)?[] rebind_costs( (string, float)?[]  exposed_costs)
        {
            var nm = 0;
            while (exposed_costs.Contains(null))
            {
                if (exposed_costs[nm] == null) exposed_costs[nm] = this.exposed_Immutable_costs[nm];
                nm++;
            }
            return exposed_costs;
        }
    }
    
    public class GA
    {
        
       public (string, float)?[] create_deviants(GA_Agent first_agent, GA_Agent other_agent)
       {
           (string, float)?[] random_simple_crossover(GA_Agent first_agent, GA_Agent other_agent)
           {
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
              //since both agents are the same.
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






