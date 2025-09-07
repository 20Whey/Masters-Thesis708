using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Production;
using UnityEngine;

public class singleton : MonoBehaviour
{
    public goap_imp GoapImp = new goap_imp();
    public static singleton Instance { get; private set; }
    public Dictionary<string, Belief> global_beliefs;
    public goap_imp goap;
    [System.Serializable]
    public struct displayed_beliefs
    {
        public string key;
        public bool condition;
    }
  public List<displayed_beliefs> display_beliefsfr;
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            goap = new goap_imp();
            global_beliefs = new Dictionary<string, Belief>();
            display_beliefsfr = new List<displayed_beliefs>();
        }
    }




    [CanBeNull]
    public Belief retrieve_belief(string identifier)
    {
        if (global_beliefs.ContainsKey(identifier))
        {
            return global_beliefs[identifier];
        }
        return null;
    }
}

