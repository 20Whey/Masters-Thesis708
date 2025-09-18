using System;
using System.Collections.Generic;
using base_move_classes;
using Production;
using Sensors;
using UnityEngine;
using Action = Production.Action; 

public class character
{
        public character self;
        public GameObject this_ob;
        public List<Action> allowed_actions;
        public Transform transform;


      /*  void Awake()
        { 
            Factories.GoalFactory goalFactory = new Factories.GoalFactory();
            
            Factories.ActionFactory factory = new Factories.ActionFactory();
            
           //ACTION STUFF
           Factories.BeliefFactory beliefFactory = new Factories.BeliefFactory(this, 0);
           
           
           /*beliefFactory.add_belief("are_we_moving", () => gameObject.GetComponent<basics>().moving);
           
           beliefFactory.add_belief("is_target_in_same_lane",
           () => car_game.is_car_in_right_lane(this.this_ob, car_game.get_closest_car(this.this_ob).gameObject));
           
           beliefFactory.add_location_belief("are_we_close_to_other_car", 
           car_game.get_closest_car(this.this_ob).transform.position, 0.5f);
           
            
            goalFactory.add_goal("crash_into_enemies", new KeyValuePair<string, bool>("multiple_cars", false), 0.1f, 
                       beliefFactory.grab_belief("is_target_in_same_lane"), 
                                   beliefFactory.grab_belief("are_we_close_to_other_car"));
            
            factory.add_action_to_list("start_moving", () => car_game.set_moving(true, 
            this.gameObject.GetComponent<basics>()),  new Dictionary<Belief, bool>
            {{ , false}}, new Dictionary<Belief, bool>{});     
            
                    
            factory.add_action_to_list("try_crash", 
            () => game_func_library.try_contest(this.this_ob, car_game.get_closest_car(gameObject).gameObject),  
            new Dictionary<Belief, bool>{{"multiple_cars", false}}, 
            new  Dictionary<Belief, bool>
            {
            {beliefFactory.grab_belief("are_we_close_to_other_car"), false },
            {beliefFactory.grab_belief("are_we_moving"), true },
            {beliefFactory.grab_belief("is_target_in_same_lane"), true}
            });
            
            
            
            allowed_actions = factory.return_actions();
            
            
            
            List<Goal> goals = goalFactory.return_goals();  }
                                                                  
*/
        public character(GameObject our_object)
        {
            self = this;
            this_ob = our_object;
            transform = our_object.transform;
            allowed_actions = new List<Action>();
        }
        public void add_allowed_action(params Action[] added_actions)
        {   
            foreach(Action action in added_actions)
            {
                if (!allowed_actions.Contains(action)) allowed_actions.Add(action);
            }
        }
}
