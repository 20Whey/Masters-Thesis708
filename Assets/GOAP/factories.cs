using UnityEngine;
using Goap;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Unity.VisualScripting;

namespace Production
{
    public class Factories
    {

        public class GoalFactory
        {
            readonly List<Goal> _goals = new List<Goal>();
            public void add_goal(string name, world_state goal_validation,float priority, List<Belief> beliefs)
            {
                _goals.Add(new Goal.Builder(name)
                .set_goal_validation(goal_validation)
                .set_priority(priority)
                .add_beliefs(beliefs)
                .Build());
            }
            public List<Goal> return_goals()
            {
                return _goals;
            }
            
            
        }
        public class ActionFactory 
        {
            //may change costs
            private List<Action> _output = new  List<Action>();
            public void add_action_to_list(string name, Func<bool?> func, world_state[] impacts, world_state[] requirements)
            {
                 this._output.Add(new Action.Builder(name)
                .add_function(func)
                .add_impacts(impacts).add_requirement(requirements)
                .Build());
            }

            public List<Action> return_actions()
            {
                return this._output;
            }
                            
        }
        public class BeliefFactory
        {
            public int name;
            [CanBeNull] public GOAP_Character Agent;

            public HashSet<Belief> Beliefs = new HashSet<Belief>();

            public BeliefFactory(GOAP_Character agent, int id)
            {
                this.Agent = agent;
                this.name = id;
            }
            public void add_belief(string identifier, Func<bool?> condition)
            {
                Beliefs.Add(new Belief.Builder(identifier)
                .add_sensor(condition)
                .Build());
            }
            public void add_location_belief(string identifier, Vector2 targetLocation, float dist)
            {
                Beliefs.Add(new Belief.Builder(identifier)
                .add_sensor(() => in_range_of(targetLocation, dist))
                .add_location(() => targetLocation)
                .Build());
            }

            public void add_desired_worldstate_belief(string key, string identifier, bool? value)
            {
                Beliefs.Add(new Belief.Builder(identifier)
                .add_sensor(() => add_global_sensor(key, value)).Build());
            }

            bool? add_global_sensor(string key, bool? value)
            {
                return world_states.check_is_valid(key, value);
            }
            
            bool in_range_of(Vector2 position, float range)
            {
                return (Vector2.Distance(Agent.this_ob.transform.position, position) > range) ? true : false;
            }
           public HashSet<Belief> return_hash()
           {
               return this.Beliefs;
           }
            //     new fuzzy_value = (current_value - smallest_value)  /(biggest_value - smallest_value)
        }
    }
    
    public class Belief : GOAP_Component
    {
            public float Priority;
            public string Name { get; }
            Func<bool?> _condition = () => false;
            Func<UnityEngine.Vector2> _observedLocation = () => UnityEngine.Vector2.zero;
            public UnityEngine.Vector2 Location;
            Belief(string name)
            {
                Name = name;
            }
            public class Builder
            {
                //Beliefs are the agents eyes for a case by case 
                public Belief Belief;
                public Builder(string name)
                {
                    Belief = new Belief(name);
                }
                public Builder add_sensor(Func<bool?> condition)
                {
                    Belief._condition = condition;
                    return this;
                }
                public Builder add_location(Func<Vector2> observedLocation)
                {
                    Belief._observedLocation = observedLocation;
                    return this;
                }
                public Belief Build()
                {
                    return this.Belief;
                }
            }
        }

    public class Goal : GOAP_Component
    {
        //what can the AI see
        public HashSet<Belief> Beliefs;
        public List<world_state> Target;
        public float Priority;
        public string Name;
        Goal(string name)
        {
            Name = name;
        }
        public class Builder
        {
            public Goal Goal;

            public Builder(string name)
            {
                Goal = new Goal(name);
                Goal.Beliefs = new HashSet<Belief>();
            }

            public Builder add_beliefs(List<Belief> beliefs)
            {
                Goal.Beliefs.AddRange(beliefs);
                return this;
            }
            public Builder set_priority(float value)
            {
                Goal.Priority = value;
                return this;
            }
            public Builder set_goal_validation(world_state state)
            {
                Goal.Target.Add(state);
                return this;
            }
            public Goal Build()
            {
                return this.Goal;
            }
        }
    }

    public class Action : GOAP_Component
    {

    //bodge tree structure        
        public Action Child { get; set; }
        public Action Parent { get; set; }
        
        private float _cost = 0.5f;
        public Func<bool?> Func;
        public readonly HashSet<world_state> _requirements;
        public readonly HashSet<world_state> _impact; 
        public Action(string name)
        {
            this.Name = name;
        }
        
        public class Builder
        {
            public Action action;
            public Builder(string name)
            {
               action = new Action(name);
            }

            public Builder add_requirement(params world_state[] condition)
            {
                action._requirements.AddRange(condition);;
                return this;
            }
            public Builder add_function(Func<bool?> func)
            {
                action.Func = func;
                return this;
            }
            public Builder modify_cost(float cost)
            {
                action._cost += cost;
                return this;
            }
            public Builder add_impacts(params world_state[] states)
            {
                for (var i = 0; i < states.Length; i++)
                {
                    action._impact.Add(states[i]);
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