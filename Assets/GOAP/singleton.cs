using System.Collections.Generic;
using JetBrains.Annotations;
using Production;
using UnityEngine;

public class singleton : MonoBehaviour
{
   
    public static singleton Instance { get; private set; }
    public Dictionary<string, Belief> global_beliefs;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            global_beliefs = new Dictionary<string, Belief>();
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

