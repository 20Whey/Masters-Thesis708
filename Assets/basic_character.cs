using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using base_move_classes;
using JetBrains.Annotations;
using UnityEngine;
using init;
using static Production.Factories;
using Goap;
using BFactory = Production.Factories.BeliefFactory;
using AFactory = Production.Factories.ActionFactory;
using Gfactory = Production.Factories.GoalFactory;
using Production;
using Action = Production.Action;
using Vector2 = UnityEngine.Vector2;
public class basic_character : MonoBehaviour
{
    public bool isdummy;
    public bool blocking;
    public bool stunned;
    public bool moving;
	public float health;
    
    public bool started_combo;
    public bool finishing_combo;
    public bool struck;
    public float timer;
    
    [CanBeNull] public Transform target;
    public character self;
    public List<Action> allowed_actions = new List<Action>();
    
    
    [CanBeNull] public List<Action> plan;
    public world_states local_worldstate;
    public goap_imp goap;
    
    public BeliefFactory belief_factory;
    public GoalFactory goals;
    public ActionFactory actions;
    
    [System.Serializable]
    public struct WorldStates
    {
        public string state;
        public bool value;
    }
    public WorldStates[] state;
    void Start()
    {
        health = 35f;
        if (!isdummy)
        {
            blocking = false;
            stunned = false;
            moving = false;
            started_combo = false;
            goap = singleton.Instance.GoapImp;
            self = new character(gameObject);

           // Dictionary<string, basic_move> movedict = basic_init.create(new Dictionary<string, basic_move>());
           belief_factory = basic_init.init_belief_factory(self);
            actions = basic_init.init_action_factory(belief_factory);  
            
            goals = basic_init.init_goal_factory(belief_factory);
            local_worldstate = new world_states();
            local_worldstate.init(null);

            local_worldstate.add_state(belief_factory.grab_belief("moving").Name, belief_factory.grab_belief("moving")._condition());
            local_worldstate.add_state(belief_factory.grab_belief("close_to_enemy").Name, belief_factory.grab_belief("close_to_enemy")._condition());
            local_worldstate.add_state(belief_factory.grab_belief("is_enemy_alive").Name, belief_factory.grab_belief("is_enemy_alive")._condition());

            local_worldstate.add_state(belief_factory.grab_belief("starting_combo").Name, belief_factory.grab_belief("starting_combo")._condition());
            local_worldstate.add_state(belief_factory.grab_belief("is_opponent_stunned").Name, belief_factory.grab_belief("is_opponent_stunned")._condition());
            local_worldstate.add_state(belief_factory.grab_belief("enemy_exists").Name, belief_factory.grab_belief("enemy_exists")._condition());
            
            

          
  
            plan = goap.bPlanner(goals.return_goals(), local_worldstate, actions.return_actions());
            
            
     
        }
    }
    void FixedUpdate()
    {
        if (!isdummy)
        {
            var singltn = singleton.Instance;
            
            for (var i = 0; i <  local_worldstate.states.Keys.Count; i++)
            {
                //Debug.Log("triggered" + itm);
                var itm = local_worldstate.states.Keys.ElementAt(i);
                local_worldstate.change_state((itm, singltn.retrieve_belief(itm)._condition()));
            }

//MOVE MEE
            if (moving)
            {
                gameObject.transform.position =
                Vector2.MoveTowards(gameObject.transform.position, (Vector2)target.position, 0.5f);
            }

        }
    }
}
