using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Sensors;
using Action = Production.Action;
using Goal = Production.Goal;
using game_logic;
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
        BeliefFactory b_factory = new BeliefFactory(basic,0);
        GoalFactory g_factory = new GoalFactory();
        ActionFactory a_factory = new ActionFactory();

        setup_worldstate(c_worldstate);
        
        
        b_factory.add_belief("is_another_car_close_enough",() => car_game.is_car_close_enough(basic.this_ob));
        b_factory.add_belief("is_target_in_same_lane", () => car_game.is_car_in_right_lane(basic.this_ob,car_game.get_closest_car(basic.this_ob).gameObject));
      
     
        
    //    g_factory.add_goal("kill_closest_car", ("a", false), 0.5f, b_factory.return_hash().ToList());
        
        List<Goal> goals = g_factory.return_goals();
    
	     //    ActionFactory a_factory = new ActionFactory();
        // b_factory.add_belief("the sky is green", fuel is 10);



        
        

    }

    private void setup_worldstate(world_states state)
    {
        state.add_state("multiple_cars", true);
        state.add_state("race_is_running", true);
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
      foreach ( var item in next_action._requirements)
      {
          if (!simulated_worldstate.check_is_valid(item.Key.Name, item.Value))
           return false;
      }
      return true;
  }
  
  //Shrimple BFS
  public HashSet<GOAP_Component> discover_tree(world_states sim_state, GOAP_Component start, List<Action> allowed)
  {
      HashSet<GOAP_Component> visited = new HashSet<GOAP_Component>();
      Queue<GOAP_Component> queue = new Queue<GOAP_Component>();
      queue.Enqueue(start);
      visited.Add(start);
      do
      {
          GOAP_Component current = queue.Dequeue();
          List<GOAP_Component> potentialOptions = 
          find_all_suitable_actions(sim_state, current, allowed);
          
          for (var i = 0; i < potentialOptions.Count; i++)
          {
              current.add_child(potentialOptions[i]);
              Action itm = potentialOptions.ElementAt(i) as Action;
              if (action_validation(sim_state, itm))
              { 
                  queue.Enqueue(itm);
                  visited.Add(itm);
              }
          }
          // consider breaking?;
      }
      while (queue.Count > 0);
      return visited;
  }
  
  //action validation
  public bool clean_filter(world_states current, Action other)
  {
      foreach (var req in other._requirements)
      {
        if (!current.check_is_valid(req.Key.Name, req.Value)) return false;
      }
      return true;
  }
      

  private List<GOAP_Component> find_all_suitable_actions(world_states c_worldstate, GOAP_Component c_action, List<Action> allowed_actions)
  {
      List<GOAP_Component> naction_list = new List<GOAP_Component>();
      foreach (var act in allowed_actions)
      {
          if (clean_filter(c_worldstate, act))
          {
              naction_list.Add(act);
          }
      }
      return naction_list;
  }
  

/*def map_out(last_action_or_goal:goal|Action, actn_list):
    for itm in find_all_suitable_actions(last_action_or_goal, actn_list):
        last_action_or_goal.add_child(itm)
    #DEBUG THREE    
    #print(len(last_action_or_goal.children))
    return last_action_or_goal
*/
        bool final_action_validation(bool b)
        {
            throw new System.NotImplementedException();
        }
    
        //tree traversal and finish planner;

    public void Planner(List<Goal> goals, List<Action> allowed_actions)
    {
        //order by ascending
        IOrderedEnumerable<Goal> ordered_goals = goals.OrderBy(goal => goal.Priority);
        world_states simulated_worldstate = current_worldstate;
        
        foreach (Goal orderedGoal in ordered_goals)
        {
            HashSet<GOAP_Component> tree_to_traverse = discover_tree(simulated_worldstate, orderedGoal, allowed_actions);
            //simple valid check can we even proceed. limit plans to a selection of 3 to start.
            
             //if(goal_validation(orderedGoal, allowed_actions))
            List<GOAP_Component>[] possible_plan = new List<GOAP_Component>[3];
            
            do
            { 
                List<GOAP_Component> current_plan = new List<GOAP_Component>();
                current_plan.Add(orderedGoal);
                
                //obselete just need tree traversal now.
                List<Action> pnext_actions = map_out(finds_suitable_action(possible_plan.First(), allowed_actions));
                foreach (Action action in pnext_actions)
                {
                    current_plan.First().add_child(action);
                }
            } while (final_action_validation(possible_plan[0].First() != orderedGoal as Goal));
        }
        /*
       def Planner(beliefs:list[goal], actn:list[Action]):
           #code for sorting a list of beliefs by priority, axed it
           beliefs = sorted(beliefs, key=rank_by_size)
           possible_plan = []

           for goal in beliefs:
               goal = map_out(goal, actn)
               #ADD INITIAL ENTRY
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
    }





}
