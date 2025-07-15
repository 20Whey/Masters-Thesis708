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
        public static bool? check_is_valid(string input, bool? value)
        {
            if (states[input] == value) return true;
            return false;
        }
    }

    public class GOAP_Character
    {
        public GOAP_Character? self;
        public GameObject thisOb;
        public UnityEngine.Vector2 position = thisOb.transform.position;
        public GOAP_Character(GameObject ourObject)
        {
            self = this;
            position = UnityEngine.Vector2.zero;
            thisOb = ourObject;
        }
    }

    public abstract class GOAP_Component
    {
        public int id = -1;
        public string name = "Not Named";
    }











}