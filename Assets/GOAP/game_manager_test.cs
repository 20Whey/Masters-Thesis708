using System.Collections.Generic;
using Goap;
using UnityEngine;
using Production;
using Sensors;
using Action = Production.Action;
using Goal = Production.Goal;
public class game_manager_test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject first_ai;

    public Dictionary<string, Belief> beliefs;
    public List<Action> actions;
    public world_states current_worldstate;


    void Awake()
    {
        /* var c_worldstate = new world_states();
         beliefs = new Dictionary<string, Belief>();
         actions = new List<Action>();
         character basic = new character(first_ai);
         //will refactor into an interface :C
         Factories.BeliefFactory b_factory = new Factories.BeliefFactory(basic, 0);
         Factories.GoalFactory g_factory = new Factories.GoalFactory();
         Factories.ActionFactory a_factory = new Factories.ActionFactory();

         a_factory.add_action_to_list("drive_forwards", () =>  );
         a_factory.add_action_to_list("drive_backwards", () => );

         b_factory.add_belief("is_target_in_same_lane", () => car_game.is_car_in_right_lane(basic.this_ob, car_game.get_closest_car(basic.this_ob).gameObject));
         b_factory.add_location_belief("are_we_close_to_other_car", car_game.get_closest_car(basic.this_ob).transform.position, 0.5f);

         g_factory.add_goal("crash_into_enemies", new KeyValuePair<string, bool>("multiple_cars", false), 0.1f, b_factory.grab_belief("is_target_in_same_lane"), b_factory.grab_belief("are_we_close_to_other_car"));

         List<Goal> goals = g_factory.return_goals();




     }

     private world_states setup_worldstate(world_states state)
     {
         state.add_state("multiple_cars", true);
         state.add_state("race_is_running", true);
         state.add_state("car_is_moving", false);
         state.add_state("is_refueling", false);
         return state;

     }

     // Update is called once per frame
     void Update()
     {

     }*/
    }
}
