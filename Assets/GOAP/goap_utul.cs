using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;
namespace Goap
{

    public class world_state
    {
        public string key
        {
            get; set;
        }
        public int value
        {
            get; set;
        }
    }

    public class World_States
    {
        public Dictionary<string, int> states;
        public void init()
        {
            states = new Dictionary<string, int>();
        }
        public bool has_state(string key)
        {
            return states.ContainsKey(key);
        }
        void add_state(string key, int value)
        {
            states.Add(key, value);
        }
        public bool check_is_valid(string input, int value)
        {
            if (states[input] == value) return true;
            return false;
        }
    }

    public class GOAP_Character
    {
        public UnityEngine.Vector2 position = UnityEngine.Vector2.zero;
        public GOAP_Character? self;
        public GameObject thisOb;
    }

    public abstract class GOAP_Component
    {
        public int id = -1;
        public string name = "Not Named";
    }











}