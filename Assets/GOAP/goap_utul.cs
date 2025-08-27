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
            Parent = null;
            c_state = new world_states();
            c_state.init(c_state);
            
        }
        [CanBeNull]
        public Node Parent
        {get; set;} 
        public List<Node> Children
        {get;set;}
        public IActionAdjacent held_obj { get; set; }
        public void add_child(Node child)
        {
            Children.Add(child);
            child.Parent = this;
        }
        public void remove_child(Node child)
        {
            Children.Remove(child);
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
        public bool real_check(string key, bool val)
        {
            foreach (Belief itm in states.Keys)
            {
                if (itm.Name.Equals(key) && states[itm] == val) return true;
            }
            return false;
        }

        public void poor_copy(world_states input)
        {
            states = new Dictionary<Belief, bool>();
            foreach (var inputState in input.states)
            {
                states.Add(inputState.Key, inputState.Value);
            }
        }
        
    
          public bool check_is_valid(Belief input, bool value)
        {
        if (has_state(input.Name))
            
            if (states[input] == value) return true;
            return false;
        }
       
          
        public bool comparison(Belief key, bool value)
        {
            foreach (Belief itm in states.Keys)
            {
                if (itm.Name == key.Name)
                {
                    if (states[itm] == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

   
        public void change_state(KeyValuePair <Belief, bool> pair)
        {
            states[pair.Key] = pair.Value;
        }
        public void add_state(Belief key, bool val)
        {
            states.Add(key, val);
        }
       
        public bool check_mult(world_states input)
        {
            foreach (var item in input.states)
            {
                if (!comparison(item.Key, item.Value)) return false;
            }
            return true;
        }
    }

}