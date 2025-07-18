using UnityEngine;
using Goap;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework.Internal;

namespace Production
{


    public class factories
    {

        public class ActionFactory
        {
            private List<Action> output;
            public ActionFactory(List<Action> output, string name, Func<bool?> func, params world_state[] states)
            {
                    this.output = output;
            }
            public void add_action_to_list(string name, Func<bool?> func, world_state[] impacts)
            {
                this.output.Add(new Action.builder(name)
                .add_function(func)
                .add_impacts(impacts).build());
            } 
            public List<Action> return_actions()
            {
                return this.output;
            }
                             //  this.output.Add(name, new Action.builder(name)
                             // .add_function(func)
                             // .add_impacts(states)
                             // .build());
        }
        public class BeliefFactory
        {
            public int id;
            [CanBeNull] public GOAP_Character agent;

            public Dictionary<string, Belief> beliefs;

            public BeliefFactory(GOAP_Character agent, int id, Dictionary<string, Belief> beliefs)
            {
                this.agent = agent;
                this.id = id;
                this.beliefs = beliefs;
            }
            public void add_belief(string identifier, Func<bool?> condition)
            {
                beliefs.Add(identifier, new Belief.builder(identifier)
                .add_sensor(condition)
                .build());
            }

            public void add_location_belief(string identifier, Vector2 target_location, float dist)
            {
                beliefs.Add(identifier, new Belief.builder(identifier)
                .add_sensor(() => in_range_of(target_location, dist))
                .add_location(() => target_location)
                .build());
            }

            public void add_desired_worldstate_belief(string key, string identifier, bool? value)
            {
                beliefs.Add(identifier, new Belief.builder(identifier)
                .add_sensor(() => add_global_sensor(key, value)).build());
            }

            public bool? add_global_sensor(string key, bool? value)
            {
                return world_states.check_is_valid(key, value);
            }
            bool? in_range_of(Vector2 position, float range)
            {
                return (Vector2.Distance(agent.this_ob.transform.position, position) > range) ? true : false;
            }
            //     new fuzzy_value = (current_value - smallest_value)  /(biggest_value - smallest_value)
        }
    }
    
    public class Belief : GOAP_Component
    {

            private float priority;
            public string Name { get; }
            Func<bool?> condition = () => false;
            Func<UnityEngine.Vector2> observed_location = () => UnityEngine.Vector2.zero;
            public UnityEngine.Vector2 location;

            Belief(string name)
            {
                Name = name;
            }
            public class builder
            {
                public Belief belief;

                public builder(string name)
                {
                    belief = new Belief(name);
                }
                public builder add_sensor(Func<bool?> condition)
                {
                    belief.condition = condition;
                    return this;
                }
             
                public builder add_location(Func<Vector2> observed_location)
                {
                    belief.observed_location = observed_location;
                    return this;
                }
                public builder set_priority(float value)
                {
                    belief.priority = value;
                    return this;
                }

                public Belief build()
                {
                    return this.belief;
                }
            }

        }



    public class Action : GOAP_Component
    {

    //bodge tree structure        
        public Action child { get; set; }
        public Action parent { get; set; }
        
        private float cost = 0.5f;
        public Func<bool?> func;
        readonly List<world_state> impact;
        public Action(string Name)
        {
            this.name = Name;
        }
        public class builder
        {
            public Action action;
            public builder(string name)
            {
                action = new Action(name);
            }
            public builder add_function(Func<bool?> Func)
            {
                action.func = Func;
                return this;
            }
            public builder modify_cost(float Cost)
            {
                action.cost += Cost;
                return this;
            }
            public builder add_impacts(world_state[] states)
            {
                for (var i = 0; i < states.Length; i++)
                {
                    action.impact.Add(states[i]);
                }
                return this;
            }
            public Action build()
            {
                return this.action;
            }
        }
    }
}