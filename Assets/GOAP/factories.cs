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
                actions.Add(name, new Action.Builder(name)
                .add_function(func)
                .add_impacts(states)
                .Build());
            }
        }
        public class BeliefFactory
        {
            public int id;
            public GOAP_Character agent;

            public Dictionary<string, Belief> beliefs;

            public BeliefFactory(GOAP_Character agent, int id, Dictionary<string, Belief> beliefs)
            {
                this.agent = agent;
                this.id = id;
                this.beliefs = beliefs;
            }
            public void add_belief(string key, Func<bool> condition)
            {
                beliefs.Add(key, new Belief.Builder(key)
                .add_condition(condition)
                .Build());
            }

            public void add_location_belief(string key, Vector2 target_location, float dist)
            {
                beliefs.Add(key, new Belief.Builder(key)
                .add_condition(() => inRangeOf(target_location, dist))
                .add_location(() => target_location)
                .Build());
            }
            bool inRangeOf(Vector2 position, float range)
            {
                return (Vector2.Distance(agent.thisOb.transform.position, position) > range) ? true : false;
            }
            //     new fuzzy_value = (current_value - smallest_value)  /(biggest_value - smallest_value)
        }


        public class Belief : GOAP_Component
        {
            public string Name { get; }
            Func<bool> condition = () => false;
            Func<UnityEngine.Vector2> observed_location = () => UnityEngine.Vector2.zero;
            public UnityEngine.Vector2 location;

            Belief(string name)
            {
                Name = name;
            }
            public class Builder
            {
                public Belief belief;

                public Builder(string name)
                {
                    belief = new Belief(name);
                }
                public Builder add_condition(Func<bool> condition)
                {
                    belief.condition = condition;
                    return this;
                }
                public Builder add_location(Func<Vector2> observed_location)
                {
                    belief.observed_location = observed_location;
                    return this;
                }
                public Belief Build()
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
        public class Builder
        {
            public Action action;
            public Builder(string name)
            {
                action = new Action(name);
            }
            public Builder add_function(Func<bool?> Func)
            {
                action.func = Func;
                return this;

            }
            public Builder modify_cost(float Cost)
            {
                action.cost += Cost;
                return this;
            }
            public Builder add_impacts(world_state[] states)
            {
                for (var i = 0; i < states.Length; i++)
                {
                    action.impact.Add(states[i]);
                }
                return this;
            }
            public Action Build()
            {
                return this.action;
            }
        }
    }


}