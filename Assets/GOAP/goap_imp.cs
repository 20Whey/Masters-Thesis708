using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Action = Production.Action;
using Goal = Production.Goal;
public class goap_imp : Factories
{ 
    public GameObject first_ai;

    public Dictionary<string, Belief> beliefs;
    public List<Action> actions;
    void Awake()
    {
        beliefs = new Dictionary<string, Belief>();
        actions = new List<Action>();
        GOAP_Character basic = new GOAP_Character(first_ai);
        //will refactor into an interface :C
        BeliefFactory b_factory = new BeliefFactory(basic,0);
        GoalFactory g_factory = new GoalFactory();
        ActionFactory a_factory = new ActionFactory();


       var a = new world_state();
       a.key = "gmaing";
       a.value = true;
        
        
        
        g_factory.add_goal("kill_closest_car", a, 0.5f, b_factory.return_hash().ToList());
       
        
        
        //    ActionFactory a_factory = new ActionFactory();
       // b_factory.add_belief("the sky is green", fuel is 10);

        
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
private void map_out(){
}
    
    public void Planner(List<Belief> goals, List<Action> allowed_actions)
    {
        //order by ascending
        IOrderedEnumerable<Belief> ordered_goals = goals.OrderBy(goal => goal.Priority);
        foreach (Belief orderedGoal in ordered_goals)
        {
            
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
