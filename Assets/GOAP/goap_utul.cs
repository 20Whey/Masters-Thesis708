using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Production;
using UnityEngine.Rendering.UI;
using Action = Production.Action;

namespace Goap
{
	public interface IGoapComponent
    {
        [CanBeNull] public string Name { get; set; }
        [CanBeNull] public int Id { get; set; }

    }
    public interface IActionAdjacent : IGoapComponent, IHasRequirements
    {
            public object self {get; set;}
        
        public object return_self()
        {
            return this.self;
        }
       
    }
    
    
    public interface IHasRequirements
    {
        public Dictionary<Belief, bool> _requirements { get; set; }
        
    }
    
    public class Node
    {
        public Node(IActionAdjacent heldObj, int id)
        {
            held_obj = heldObj;
            Id = id;
            Children = new List<Node>();
            c_state = new world_states();
            c_state.init(c_state);
            
        }
        public Node Parent
        {get; set;} 
        public List<Node> Children
        {get;set;}
        public IActionAdjacent held_obj { get; set; }
        public void add_child(Node child)
        {
            Debug.Log(this.held_obj.Name + " " + child.held_obj.Name);
            Children.Add(child);
            child.Parent = this;
        }

        public int Id
        {
            get;
            set;
        }
        //keep track of stuff
        public world_states c_state { get; set; }
        public world_states grab_state()
        {
            return this.c_state;
        }
    }



    public class world_state
    {
        public void init(KeyValuePair<Belief, bool> pair)
        {
            this.key = pair.Key;
            this.value = pair.Value;
        }
    public Belief key
        {
            get; set;
        }
        public bool value
        {
            get; set;
        }
    }
    
    //VISIBLE WORLDSTATES

    public class world_states
    {
        public Dictionary<Belief, bool> states;
        public void init([CanBeNull] world_states base_state)
        {
            if (base_state != null)
            {
                states = base_state.states;
            }
            else
            {
                states = new Dictionary<Belief, bool>();
            }
        }
        
        public bool has_state(string key)
        {
            foreach (Belief itm in states.Keys)
            {
                if (itm.Name == key) return true;
            }
            return false;
        }
        public void change_state(KeyValuePair <Belief, bool> pair)
        {
            states[pair.Key] =  pair.Value;
        }
        public void add_state(Belief key, bool val)
        {
            states.Add(key, val);
        }
        public bool check_is_valid(Belief input, bool value)
        {
         //   Debug.Log(input.Name +" " + value);
            var a = input;
            var b = value;
            a.GetHashCode();
            
            if (states[input] == value) return true;
            return false;
        }
        public bool check_mult(world_states input)
        {
            foreach (var item in input.states)
            {
               if (!this.check_is_valid(item.Key, item.Value)) return false;
            }
            return true;
        }
    }

}