using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Production;
using UnityEngine;

public class singleton : MonoBehaviour
{
    public static goap_imp GoapImp = new goap_imp();

    public Dictionary<string, Belief> global_beliefs;
    public goap_imp goap;

    public bool signal;
    [System.Serializable]
    public struct displayed_beliefs
    {
        public string key;
        public bool condition;
    }
  public List<displayed_beliefs> display_beliefsfr;
    
    
    void Awake()
    {
      
            goap = new goap_imp();
            global_beliefs = new Dictionary<string, Belief>();
            display_beliefsfr = new List<displayed_beliefs>();
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


    void FixedUpdate()
    {

        if (signal)
        {
            for (var i = 0; i < display_beliefsfr.Count; i++)
            {
                var db = new displayed_beliefs();

                db.key = display_beliefsfr[i].key;
                db.condition = retrieve_belief(display_beliefsfr[i].key)._condition();
                
                display_beliefsfr[i]=db;
            }
            
            
        }
        
        
    }


}

