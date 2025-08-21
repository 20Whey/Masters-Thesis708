using System.Collections.Generic;
using base_move_classes;
using JetBrains.Annotations;
using UnityEngine;
using init;
using static Production.Factories;
using Goap;
using Production;
using Action = Production.Action;
public class basic_character : MonoBehaviour
{
    public bool blocking;
    public bool stunned;
    public bool moving;
	public float health;
    public float timer;
    public bool struck;
    [CanBeNull] public Transform target;
    public character self;
    public List<Action> allowed_actions = new List<Action>();
    [CanBeNull] public List<Action> plan;

    void Awake()
    {
        goap_imp goap = new  goap_imp();
        
        self = new character(gameObject);

        Dictionary<string, basic_move> movedict = basic_init.create(new Dictionary<string, basic_move>());
        BeliefFactory b = basic_init.init_belief_factory(self);
        ActionFactory a = basic_init.init_action_factory(b);
        GoalFactory g = basic_init.init_goal_factory(b);
        plan = goap.bPlanner(g.return_goals(), a.return_actions());

    }
    void FixedUpdate()
    {
        if (moving) 
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, (Vector2)target.position, 0.5f);
        }
    }
}
