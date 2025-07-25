using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Production;
using Action = Production.Action;

namespace Goap
{
	public abstract class GOAP_Component
    {
        [CanBeNull] public int Id { get; set; }
        [CanBeNull] public string Name { get; set; }
        public GOAP_Component Parent{get;private set;} 
        public GOAP_Component Child{get;private set;}


        public void add_child(GOAP_Component child)
        {
            this.Child = child;
            child.Parent = this;
        }
        
    }


    public class world_state
    {
        public string key
        {
            get; set;
        }
        public bool? value
        {
            get; set;
        }
    }

    public class world_states
    {
        public static Dictionary<string, bool?> states;
        public void init()
        {
            states = new Dictionary<string, bool?>();
        }
        public bool has_state(string key)
        {
            return states.ContainsKey(key);
        }
        void add_state(string key, bool? value)
        {
            states.Add(key, value);
        }
        public static bool check_is_valid(string input, bool? value)
        {
            if (states[input] == value) return true;
            return false;
        }
    }

    public class GOAP_Character
    {
        public GOAP_Character? self;
        public GameObject this_ob;
        public List<Belief> beliefs;
        public List<Action> allowed_actions;
        public Transform transform;
        public GOAP_Character(GameObject our_object)
        {
            self = this;
            this_ob = our_object;
            transform = our_object.transform;
            
            allowed_actions = new List<Action>();
            beliefs = new List<Belief>();
            
        }
        public void add_allowed_action(params Action[] added_actions)
        {   
            foreach(Action action in added_actions)
            {
                if (!allowed_actions.Contains(action)) allowed_actions.Add(action);
            }
        }
    }



}