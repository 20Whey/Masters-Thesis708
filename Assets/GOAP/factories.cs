using UnityEngine;
using Goap;
using System;
using System.Collections.Generic;

namespace Production
{


    public class factories
    {

        public class ActionFactory
        {
            public Dictionary<string, Action> actions;

            public ActionFactory(string name, Func<bool?> func, params world_state[] states)
            {
                actions.Add(name, new Action.builder(name)
                .add_function(func)
                .add_impacts(states)
                .build());
            }
        }
        public class BeliefFactory
        {
            public int id;
            public GOAP_Character agent;

            public Dictionary<string, belief> beliefs;

            public BeliefFactory(GOAP_Character agent, int id, Dictionary<string, belief> beliefs)
            {
                this.agent = agent;
                this.id = id;
                this.beliefs = beliefs;
            }
            public void add_belief(string key, Func<bool> condition)
            {
                beliefs.Add(key, new belief.builder(key)
                .add_condition(condition)
                .build());
            }

            public void add_location_belief(string key, Vector2 target_location, float dist)
            {
                beliefs.Add(key, new belief.builder(key)
                .add_condition(() => inrangeof(target_location, dist))
                .add_location(() => target_location)
                .build());
            }
            bool inrangeof(Vector2 position, float range)
            {
                return (Vector2.Distance(agent.thisOb.transform.position, position) > range) ? true : false;
            }
            //     new fuzzy_value = (current_value - smallest_value)  /(biggest_value - smallest_value)
        }


        public class belief : GOAP_Component
        {
            public string Name { get; }
            Func<bool> condition = () => false;
            Func<UnityEngine.Vector2> observed_location = () => UnityEngine.Vector2.zero;
            public UnityEngine.Vector2 location;

            belief(string name)
            {
                Name = name;
            }
            public class builder
            {
                public belief belief;

                public builder(string name)
                {
                    belief = new belief(name);
                }
                public builder add_condition(Func<bool> condition)
                {
                    belief.condition = condition;
                    return this;
                }
                public builder add_location(Func<Vector2> observed_location)
                {
                    belief.observed_location = observed_location;
                    return this;
                }
                public belief build()
                {
                    return this.belief;
                }
            }

        }
    }

    public class Action : GOAP_Component
    {
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