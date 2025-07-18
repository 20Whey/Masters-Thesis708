using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Belief = Production.Belief;
using Action = Production.Action;
public class goap_imp : factories
{ 
    public GameObject first_ai;
    void Awake()
    {
        
        GOAP_Character basic = new GOAP_Character(first_ai);
        //will refactor into an interface :C
        BeliefFactory b_factory = new BeliefFactory(basic, 0, new Dictionary<string, Belief>());
       // ActionFactory a_factory = new ActionFactory();



    }
    public void Planner(List<Belief> goals, List<Action> allowed_actions)
    {
       // goals.OrderBy(item => item.priority);
        
        //
        // do
        // {
        //     
        // }
        // while( )
        //
        
    }



}