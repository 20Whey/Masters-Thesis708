using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Action = Production.Action;
public class goap_imp : Factories
{ 
    public GameObject first_ai;

    public Dictionary<string, Belief> goals;
    public List<Action> actions;
    void Awake()
    {
        goals = new Dictionary<string, Belief>();
        actions = new List<Action>();
        GOAP_Character basic = new GOAP_Character(first_ai);
        //will refactor into an interface :C
        BeliefFactory b_factory = new BeliefFactory(basic, 0, goals);
    //    ActionFactory a_factory = new ActionFactory();



    }


  
    private void find_suitable_actions(Belief beliefs, List<GOAP_Component> allowed_actions /*Informing_Beliefs*/)
    {
        List<Action> actions = new List<Action>();
        foreach (Action action in allowed_actions)
        {
            
            
        }
            
            
            
    }

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
