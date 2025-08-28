using UnityEngine;
using Goap;
using System;
using System.Collections.Generic;
using System.Linq;
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
            public void add_goal(string name, KeyValuePair<string, bool> goal_validation,float priority, params string[] beliefs)
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
            public void add_action_to_list(string name, Func<bool?> func, (string, bool)[] requirements, (string, bool)[] impacts)
            {
                 this._output.Add(new Action.Builder(name)
                .add_function(func)
                .add_impacts(impacts)
                .add_requirement(requirements)
                .Build());
            }

            public List<Action> return_actions()
            {
                return this._output;
            }
                            
        }
        public class BeliefFactory
        {
            [CanBeNull] public character Agent;

            public Dictionary<string, Belief> Beliefs = new Dictionary<string, Belief>();

            public BeliefFactory(character agent)
            {
                this.Agent = agent;
            }
            public void add_belief(string identifier, Func<bool> condition)
            {
                Beliefs.Add(identifier, new Belief.Builder(identifier)
                .add_sensor(condition)
                .Build()
                );
            }
            public void add_location_belief(string identifier, Vector2 targetLocation, float dist)
            {
                Beliefs.Add(identifier,new Belief.Builder(identifier)
                .add_sensor(() => in_range_of(targetLocation, dist))
                .add_location(() => targetLocation)
                .Build()
                );
            }
            
            
            public void add_target_belief(string identifier, Func<Transform> target)
            {
                Beliefs.Add(identifier,new Belief.Builder(identifier).
                add_target(target)
                .Build()
                );
            }
            //this one is legacy
            public void add_desired_worldstate_belief(string identifier, world_states c_world, Belief key, bool value)
            {
                Beliefs.Add(identifier,new Belief.Builder(identifier)
                .add_sensor(() => add_global_sensor(c_world, key, value))
                .Build());
            }

          public bool add_global_sensor(world_states c_world, Belief key, bool value)
            {
                return c_world.comparison(key.Name, value);  
            }
            
            bool in_range_of(Vector2 position, float range)
            {
                return (Vector2.Distance(Agent.this_ob.transform.position, position) > range) ? true : false;
            }

         
           public Belief grab_belief(string identifier)
           {
               return this.Beliefs[identifier];
           }
            //     new fuzzy_value = (current_value - smallest_value)  /(biggest_value - smallest_value)
        }
    }
    
    public class Belief : IGoapComponent
    {
            public float Priority;
            public string Name { get; set; }
            public int Id { get; set; }
            Func<bool> _condition = () => false;
            Func<UnityEngine.Vector2> _observedLocation = () => Vector2.zero;
            public UnityEngine.Vector2 Location;
            Func<UnityEngine.Transform> _observedTarget = () => GameObject.Instantiate(new GameObject()).transform;
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
                public Builder add_sensor(Func<bool> condition)
                {
                    Belief._condition = condition;
                    return this;
                }
                public Builder add_location(Func<Vector2> observedLocation)
                {
                    Belief._observedLocation = observedLocation;
                    return this;
                }
                public Builder add_target(Func<UnityEngine.Transform> observed_character)
                {
                    Belief._observedTarget = observed_character;
                    return this;
                }
                public Belief Build()
                {
                    return this.Belief;
                }
            }
        }

    public class Goal : IActionAdjacent
    {
        //what can the AI see
        public Dictionary<string, Belief> Beliefs;
        public world_state Target;
        public float Priority;
        public string Name { get; set; }
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
                Goal.Beliefs = new Dictionary<string, Belief>();
            }
            public Builder add_beliefs( params string[] beliefs)
            {
                foreach (var item in beliefs)
                {
                    Goal.Beliefs.Add(item, singleton.Instance.retrieve_belief(item));
                }
                return this;
            }
            public Builder set_priority(float value)
            {
                Goal.Priority = value;
                return this;
            }
            public Builder set_goal_validation(KeyValuePair<string, bool> state)
            {
                //refactor for subm
                var c = new world_state();
                
                c.init(state);
                Goal.Target = c;
                return this;
            }
            public Goal Build()
            {
                this.Goal.self = this.Goal;
                return this.Goal;
            }
        }
        public int Id
        {
            get;
            set;
        }

        public Dictionary<string, bool> _requirements
        {
            get;
            set;
        }
        public object self
        {
            get;
            set;
        }
    }

    public class Action : IActionAdjacent
    {
        private float _cost = 0.5f;
        public Func<bool?> Func;
      
        public Dictionary<string, bool> _impact;
        public bool has_requirements;
        public Action(string name)
        {
            this.Name = name;
            this.has_requirements = true;
        }
        
        public class Builder
        {
            public Action action;
            public Builder(string name)
            {
               action = new Action(name);
            }

            public Builder add_requirement((string, bool)[] condition)
            {
                action._requirements = new Dictionary<string, bool>();
                foreach (var itm in condition)
                {
                    action._requirements.Add(itm.Item1, itm.Item2);
                }
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
            public Builder add_impacts((string, bool)[] states)
            {
                action._impact = new Dictionary<string, bool>();
                foreach (var itm in states)
                {
                    action._impact.Add(itm.Item1, itm.Item2);
                }
                return this;

            }
            public Action Build()
            {
                this.action.self = this.action;
                return this.action;
            }
        }
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

        [CanBeNull]
        public Dictionary<string, bool> _requirements
        {
            get;
            set;
        }
        public object self
        {
            get;
            set;
        }
    }
}