using System;
using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using Action = Production.Action;
using Goal = Production.Goal;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class goap_imp : Factories
{
    public world_states current_worldstate;

    //is Action valid
    bool action_validation(world_states simulated_worldstate, Action next_action)
    {
        foreach (var item in next_action._requirements)
        {
            if (!simulated_worldstate.comparison(item.Key, item.Value))
                return false;
        }
        return true;
    }

//used to find 

//adding in quick action float validation
    [CanBeNull]
   Node grab_from_state(List<Node> tree, world_states input_worldstate)
    {
        List<Node> matches = new List<Node>();
        for (var i = 0; i < tree.Count; i++)
        {
            var b = input_worldstate.check_mult(tree[i].c_state);
            if (b)
            {
                matches.Add(tree[i]);
            }
        }
        return compare_plans(matches)[0];
    }
   
    List<Node> compare_plans(List<Node>possible_start_points)
    {
        List<(List<Node>, float)> plans  = new List<(List<Node>, float)>();
        possible_start_points.ForEach( possible_start_point => plans.Add(create_weighted_plan(possible_start_point)));
        plans.OrderByDescending(item => item.Item2);

        List<List<Node>> ordered_plans = new List<List<Node>>(); 
        plans.ForEach(item => ordered_plans.Add(item.Item1));
        
        return ordered_plans[0]; 
    }

    (List<Node>, float) create_weighted_plan(Node start)
    {
        (List<Node>, float) plan = (new List<Node>(), 0f);
        //where our worldstate reaches our target; 
        //get root
        plan.Item1.Add(start);
        plan.Item2 = start.held_obj.Cost;
        //  Node cNode = start;
        while (start.Parent != null)
        {
            plan.Item1.Add(start);
            plan.Item2 += start.held_obj.Cost;
            start = start.Parent;
        }
        return plan;
    }   

   
   
//I have a filtered tree, all roads lead to the end. 
//technically this version takes the most complex plan possible. by virtue of being the last element
    List<Node> create_basic_plan(List<Node> tree, world_states state)
    {
        List<Node> plan = new List<Node>();
        //where our worldstate reaches our target; 
        Node start = grab_from_state(tree, state);
        if (start == null) return null; // basically we cant do the goal.real k    
        //get root
        plan.Add(start);
      //  Node cNode = start;
        while (start.Parent != null)
        {
            plan.Add(start);
            start = start.Parent;
        }
        return plan;
    }

    //action validation
    
    public bool does_numerical_action_lead_to_positive_outcome(KeyValuePair<string, float> numerical_state, world_states simulated_worldstate)
    {
        //are we closer to our objective by doing this?
        if (simulated_worldstate.states[numerical_state.Key] - numerical_state.Value >= simulated_worldstate.states[numerical_state.Key])
        {
            return true;
        }
        return false;

    }
    //CURRENTLY BREAKS LOGIC
    public bool clean_filter(world_states current, Action other)
    {
        //if other has requirements
        if (other._impact != null)
        {
            foreach (var req in other._impact)
            {
//                    Debug.Log(current.check_is_valid(req.Key, req.Value));



                if (current.has_state(req.Key))
                {

                    if (req.Value > 1.0f)
                    {
                        if (req.Value > 1.0f && !does_numerical_action_lead_to_positive_outcome(req, current))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!current.check_is_valid(req.Key, req.Value)) return false;
                    }
                }
            /*  else
                {
                    return false;
                }*/
            }
        }
        return true;
    }

    private bool check_for_numerical_validation(KeyValuePair<string, float> state, BeliefFactory bf)
    {
        Debug.Log("WHAATTT");
        //CHECK FOR NUMERICAL
        
                switch (bf.Beliefs[state.Key].operation)
                {
                    case "<":
                        return (state.Value < bf.Beliefs[state.Key]._target_value);
                    
                    case ">":
                        return (state.Value > bf.Beliefs[state.Key]._target_value);
                    
                    default:
                        throw new NotImplementedException();
                        break;
                }
            
            return false;
    }

    private List<IActionAdjacent> find_all_suitable_actions(world_states c_worldstate, List<Action> allowed_actions)
    {
        List<IActionAdjacent> naction_list = new List<IActionAdjacent>();
        
        foreach (var act in allowed_actions)
        {
            if (clean_filter(c_worldstate, act))
            {
                naction_list.Add(act);
            }
        }
        return naction_list;
    }

    bool worldstate_validation(world_states sim_state, world_states real_worldstate, BeliefFactory bf_ref)
    {
        foreach (var item in real_worldstate.states)
        {
            //Debug.Log(item.Key + " "+ bf_ref.Beliefs[item.Key].is_numerical );
            if (!bf_ref.Beliefs[item.Key].is_numerical)
            {
                if (!sim_state.comparison(item.Key, item.Value)) return false;
            } else if (!check_for_numerical_validation(item, bf_ref)) return false;
        }
        return true;
    }
    //Shrimple BFS  this literally maps out every single possible plan.
    //our farthest back point is genininely our goal
    [CanBeNull]
    public List<Node> discover_tree(world_states sim_state, Goal start, List<Action> allowed, BeliefFactory  bf_ref)
    {       
        //create root 
        int nm = 0;
        List<Node> visited = new List<Node>();
        Queue<Node> queue = new Queue<Node>();
        
        Node root = new Node(start, nm);
        root.c_state.init(null);
        root.c_state.add_state(start.Target.key, (start.Target.value));
        
        //add root to BFS queue and visited
        queue.Enqueue(root);
        visited.Add(root);
        //create a tree structure
        do
        {
            Node current = queue.Dequeue();
            List<IActionAdjacent> potentialOptions = find_all_suitable_actions(current.c_state, allowed);
            //Debug.Log(potentialOptions.Count);
               
            nm++;
            for (var i = 0; i < potentialOptions.Count; i++)
            {
                    var c_child = new Node(potentialOptions[i], nm);

                    current.add_child(c_child);
                    if (current.Parent != null )
                    {
                        if (current.Parent.held_obj != potentialOptions[i])
                        {
                            Action itm = c_child.held_obj as Action;
                            //c_child is valid
                            var a = c_child.Parent.c_state;

                            c_child.c_state.init(a);
                            c_child.c_state.poor_copy(mutate_state(c_child.Parent.grab_state(), c_child.held_obj as Action));

                            queue.Enqueue(c_child);
                            visited.Add(c_child);
                            if (worldstate_validation(c_child.c_state, sim_state, bf_ref)) return visited;
                        }
                        else
                        {
                            current.remove_child(c_child);
                        }
                    }
                    else
                    {
                        //  Action itm = c_child.held_obj as Action;
                        //c_child is valid
                        var b = c_child.Parent.c_state;
                        
                        c_child.c_state.init(b);
                        c_child.c_state.poor_copy(mutate_state(c_child.Parent.grab_state(), c_child.held_obj as Action));

                        queue.Enqueue(c_child);
                        visited.Add(c_child);
                        if (worldstate_validation(c_child.c_state, sim_state, bf_ref)) return visited;
                    }
            }
/*  }*/

  // consider breaking when queue gets too long and if there are no options left
        } while (queue.Count > 0 && nm < 500);
return visited;
}
//change input
private world_states mutate_state(world_states state, Action input)
{
world_states edited_ver = new world_states();
edited_ver.init(state);
foreach (var req in input._requirements)
{
  if (!edited_ver.has_state(req.Key))
  {
      edited_ver.add_state(req.Key, req.Value);
  }
  else
  {
      //we know this is a numerical belief, we add a either a positive or negative value, hoping it works and is set up correctly.
     if (Math.Abs(req.Value) > 1.1f)  edited_ver.addition_state((req.Key, req.Value)); else edited_ver.change_state((req.Key, req.Value));
      
  }
}
return edited_ver;
}

//tree traversal and finish planner;


[CanBeNull]
public List<Action> bPlanner(List<Goal> goals, world_states worldstate, List<Action> allowed_actions, BeliefFactory bf_ref)
{
//order by ascending
IOrderedEnumerable<Goal> ordered_goals = goals.OrderBy(goal => goal.Priority);
world_states simulated_worldstate = worldstate;

List<Node> tree = discover_tree(simulated_worldstate, goals[0], allowed_actions, bf_ref);
//foreach goal in ordered_goals...

  List<Action> plan = new List<Action>();
  var pln = create_basic_plan(tree, worldstate);
  if (pln != null)
  {
      for (var i = 0; i < pln.Count; i++)
      {
      //    Debug.Log(pln.ElementAt(i).held_obj.Name);
          plan.Add(pln.ElementAt(i).held_obj.self as Action);
      }
      return plan;
  }
  return null;

}



}


    
