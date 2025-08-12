using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Sensors;
using Action = Production.Action;
using Goal = Production.Goal;
using game_logic;
using JetBrains.Annotations;
using UnityEngine.InputSystem.LowLevel;
public class goap_imp : Factories
{
    public GameObject first_ai;

    public Dictionary<string, Belief> beliefs;
    public List<Action> actions;
    public world_states current_worldstate;


    void Awake()
    {
        var c_worldstate = new world_states();
        beliefs = new Dictionary<string, Belief>();
        actions = new List<Action>();
        GOAP_Character basic = new GOAP_Character(first_ai);
        //will refactor into an interface :C
        BeliefFactory b_factory = new BeliefFactory(basic, 0);
        GoalFactory g_factory = new GoalFactory();
        ActionFactory a_factory = new ActionFactory();

        setup_worldstate(c_worldstate);


        b_factory.add_belief("is_another_car_close_enough", () => car_game.is_car_close_enough(basic.this_ob));
        b_factory.add_belief("is_target_in_same_lane",
        () => car_game.is_car_in_right_lane(basic.this_ob, car_game.get_closest_car(basic.this_ob).gameObject));
        b_factory.add_desired_worldstate_belief("stop_the_race", c_worldstate, "race_is_running", false);


        //    g_factory.add_goal("kill_closest_car", ("a", false), 0.5f, b_factory.return_hash().ToList());

        List<Goal> goals = g_factory.return_goals();

        //    ActionFactory a_factory = new ActionFactory();
        // b_factory.add_belief("the sky is green", fuel is 10);

    }
    private void setup_worldstate(world_states state)
    {
        state.add_state("multiple_cars", true);
        state.add_state("race_is_running", true);
        state.add_state("car_is_moving", false);
        state.add_state("is_refueling", false);

    }


    /*    private Action[] find_suitable_actions(Goal end_goal, List<Action> allowed_actions /*Informing_Belief)
      {

        Action return_if_not_valid()
          {

          }
          foreach (Belief belief in end_goal.Beliefs)
          {

          }
          List<Action> actions = new List<Action>();
      }
  */

    //is Action valid
    bool action_validation(world_states simulated_worldstate, Action next_action)
    {
        foreach (var item in next_action._requirements)
        {
            if (!simulated_worldstate.check_is_valid(item.Key.Name, item.Value))
                return false;
        }
        return true;
    }

    //Shrimple BFS
    






    //action validation
    public bool clean_filter(world_states current, Action other)
    {
        foreach (var req in other._requirements)
        {
            if (!current.check_is_valid(req.Key.Name, req.Value)) return false;
        }
        return true;
    }


    [CanBeNull]
    private List<IActionAdjacent> find_all_suitable_actions(world_states c_worldstate, IActionAdjacent c_action,
    List<Action> allowed_actions)
    {
        IActionAdjacent self = c_action.return_self() as IActionAdjacent;
        List<IActionAdjacent> naction_list = new List<IActionAdjacent>();
        foreach (var act in allowed_actions)
        {
            if (clean_filter(c_worldstate, act))
            {
                naction_list.Add(act);
            }
            else
            {
                return null;
            }

        }
        return naction_list;
    }

//map out each possible action for each stage 
  

/*def map_out(last_action_or_goal:goal|Action, actn_list):
    for itm in find_all_suitable_actions(last_action_or_goal, actn_list):
        last_action_or_goal.add_child(itm)
    #DEBUG THREE
    #print(len(last_action_or_goal.children))
    return last_action_or_goal
*/

    bool worldstate_validatiion(world_states sim_state, Goal goal)
    {
        foreach (var item in goal._requirements)
        {
           if(!sim_state.check_is_valid(item.Key.Name, item.Value))return false;
        }
        return true;
    }
 
    
    [CanBeNull]
    public List<Node> discover_tree(world_states sim_state, Goal start, List<Action> allowed)
    {
        //create root 
        int nm = 0;
        List<Node> visited = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        Node root = new Node(start, nm);
        root.c_state = sim_state;
        //add root to BFS queue and visited
        queue.Enqueue(root);
        visited.Add(root);
        //create a tree structure
        do
        {
            Node current = queue.Dequeue();
            List<IActionAdjacent> potentialOptions = find_all_suitable_actions(current.c_state, current.held_obj, allowed);
            nm++;
            for (var i = 0; i < potentialOptions.Count; i++)
            {
                current.add_child(new Node(potentialOptions[i], nm));
                Action itm = potentialOptions.ElementAt(i) as Action;
                if (action_validation(current.c_state, itm))
                {
                    Node valid_node = new Node(itm, nm);
                    valid_node.c_state = mutate_state(valid_node.Parent.grab_state(), valid_node.held_obj as Action);
                    queue.Enqueue(valid_node);
                    visited.Add(valid_node);
                    if (worldstate_validatiion(valid_node.c_state, start)) return visited;
                }
            }
            // consider breaking?;
        } while (queue.Count > 0);
        return null;
    }

    
    //tree traversal and finish planner;

    public void Planner(List<Goal> goals, List<Action> allowed_actions)
    {
        //order by ascending
        IOrderedEnumerable<Goal> ordered_goals = goals.OrderBy(goal => goal.Priority);
        world_states simulated_worldstate = current_worldstate;
        
        /*  foreach (Goal orderedGoal in ordered_goals)
        {
          //  List<Node> tree_to_traverse = discover_tree(simulated_worldstate, orderedGoal, allowed_actions);
            //simple valid check can we even proceed. limit plans to a selection of 3 to start.
            //create plan and then
        }*/





    }

    private world_states mutate_state(world_states state, Action input)
    {
        foreach (var req in input._impact)
        {
            state.add_state(req.Key, req.Value);
        }
        return state;
    }

            /*
           def Planner(beliefs:list[goal], actn:list[Action]):
               #code for sorting a list of beliefs by priority, axed it
               beliefs = sorted(beliefs, key=rank_by_size)
               possible_plan = []

               for goal in beliefs:
                   goal = map_out(goal, actn)
                  
                   possible_plan.append(map_out(find_suitable_action(goal, actn), actn))
                   #while not at current worldstate, work backwards
                   while(final_action_validation(possible_plan[0]) != True):
                       next_action = map_out(find_suitable_action(possible_plan[0], actn),actn)
                           #break if the next action wouldn't be valid

                      #insert action at the beginning (saves having to reverse list later)
                       next_action.parent = possible_plan[0]
                       possible_plan.insert(0,next_action)

                       if next_action == None:
                           break
                      #DEBUG FOUR
               for element in possible_plan:
                  print(element.name)
                  element.func()

*/
   
}
//if(goal_validation(orderedGoal, allowed_actions))
         //   List<IActionAdjacent>[] possible_plan = new List<IActionAdjacent>[3];
            
         /*
         
         
            do
            { 
                List<IActionAdjacent> current_plan = new List<IActionAdjacent>();
                current_plan.Add(orderedGoal);
                
                //obselete just need tree traversal now.
                List<Action> pnext_actions = find_all_suitable_actions(,possible_plan.First(), allowed_actions);
                foreach (Action action in pnext_actions)
                {
                    (Node)current_plan.First().add_child(action);
                }
            } while (final_action_validation(possible_plan[0].First() != orderedGoal as Goal));


*/

/*

           Planner(player, actL)*/
        // beliefs.OrderBy(item => item.priority);
        //
        // do
        // {
        //     
        // }
        // while( )
        //
        //
    
