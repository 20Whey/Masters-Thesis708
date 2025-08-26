using System.Collections.Generic;
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
public class basic_character : MonoBehaviour
{
    public bool blocking;
    public bool stunned;
    public bool moving;
	public float health;
    public bool started_combo;
    public bool finishing_combo;
    public float timer;
    public bool struck;
    [CanBeNull] public Transform target;
    public character self;
    public List<Action> allowed_actions = new List<Action>();
    [CanBeNull] public List<Action> plan;

    void Awake()
    {

        blocking = false;
        stunned = false;
        moving = false;
        started_combo = false;
        goap_imp goap = new goap_imp();

        self = new character(gameObject);
        
        Dictionary<string, basic_move> movedict = basic_init.create(new Dictionary<string, basic_move>());
        BeliefFactory b = basic_init.init_belief_factory(self);

        ActionFactory a = basic_init.init_action_factory(b);
        world_states local_worldstate = new world_states();
        local_worldstate.init(null);
        
        local_worldstate.add_state(b.grab_belief("moving"), false);
        
        local_worldstate.add_state(b.grab_belief("starting_combo"), false);
        
        local_worldstate.add_state(b.grab_belief("close_to_enemy"), false);
        
        local_worldstate.add_state(b.grab_belief("is_enemy_alive"), true);

        GoalFactory g = basic_init.init_goal_factory(b);
    
        plan = goap.bPlanner(g.return_goals(), local_worldstate, a.return_actions());
        foreach (var act in plan)
        {
            Debug.Log(act.Name);
        }

    }
    void FixedUpdate()
    {
        if (moving) 
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, (Vector2)target.position, 0.5f);
        }
    }
}
