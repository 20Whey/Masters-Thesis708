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
    public interface IActionAdjacent : IGoapComponent, IHasRequirements,INeedsReference
    {
    }
    
    public interface IHasRequirements
    {
        public Dictionary<string, bool> _requirements { get; set; }
        
    }

    public interface INeedsReference
    {
        public object self {get; set;}
        
        public object return_self()
        {
            return this.self;
        }
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
        public void init(KeyValuePair<string, bool> pair)
        {
            this.key = pair.Key;
            this.value = pair.Value;
        }
    public string key
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
        public Dictionary<string, bool> states;
        public void init([CanBeNull] world_states base_state)
        {
            if (base_state != null)
            {
                states = base_state.states;
            }
            else
            {
                states = new Dictionary<string, bool>();
            }
        }
        /*     public void update(Factories.BeliefFactory bf)
        {
            foreach (KeyValuePair<string, bool> itm in states)
            {
                change_state(new KeyValuePair<string, bool> (itm.Key, bf.grab_belief(itm.Key)._condition()));
            }
        }*/
        public bool has_state(string key)
        {
            foreach (string itm in states.Keys)
            {
                if (itm == key) return true;
            }
            return false;
        }
        public bool real_check(string key, bool val)
        {
            foreach (string itm in states.Keys)
            {
                if (itm.Equals(key) && states[itm] == val) return true;
            }
            return false;
        }

        public void poor_copy(world_states input)
        {
            states = new Dictionary<string, bool>();
            foreach (var inputState in input.states)
            {
                states.Add(inputState.Key, inputState.Value);
            }
        }
        
    
        public bool check_is_valid(string input, bool value)
        {
        if (has_state(input))
            if (states[input] == value) return true;
        
            return false;
        }
       
          
        public bool comparison(string key, bool value)
        {
                if (states[key] == value) return true;
            
            return false;
        }

   
        public void change_state((string, bool) pair)
        {
           
           states[pair.Item1] = pair.Item2;
        }
        public void add_state(string key, bool val)
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