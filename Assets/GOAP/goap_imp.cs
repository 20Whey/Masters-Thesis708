using System.Collections.Generic;
using System.Linq;
using Goap;
using Production;
using UnityEngine;
using Sensors;
using Action = Production.Action;
using Goal = Production.Goal;
using base_move_classes;
using JetBrains.Annotations;
using Unity.VisualScripting;
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
        character basic = new character(first_ai);
        //will refactor into an interface :C
      
        
      

    }



    //is Action valid
    bool action_validation(world_states simulated_worldstate, Action next_action)
    {
        foreach (var item in next_action._requirements)
        {
            if (!simulated_worldstate.check_is_valid(item.Key, item.Value))
                return false;
        }
        return true;
    }

//used to find 
    [CanBeNull]
    Node grab_from_state(List<Node> tree, world_states input_worldstate)
    {
        foreach (var item in tree)
        {
            if (input_worldstate.check_mult(input_worldstate)) return item;
        }
        return null;
    }


//I have a filtered tree, all roads lead to the end. 
//technically this version takes the most complex plan possible. by virtue of being the last element
    [CanBeNull]
    List<Node> create_basic_plan(List<Node> tree, world_states state)
    {
        List<Node> plan = new List<Node>();
        //where our worldstate reaches our target; 
        Node start = grab_from_state(tree, state);
        if (start == null) return null; // basically we cant do the goal.real k    
        //get root
        plan.Add(start);
        Node cNode = start;
        while (cNode.Parent != null)
        {
            plan.Add(cNode);
            cNode = cNode.Parent;
        }
        plan.Reverse();
        return plan;
    }

    //action validation
    public bool clean_filter(world_states current, Action other)
    {
        foreach (var req in other._requirements)
        {
            if (!current.check_is_valid(req.Key, req.Value)) return false;
        }
        return true;
    }


    [CanBeNull]
    private List<IActionAdjacent> find_all_suitable_actions(world_states c_worldstate,
    List<Action> allowed_actions)
    {
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

    bool worldstate_validation(world_states sim_state, IHasRequirements goal)
    {
        foreach (var item in goal._requirements)
        {
            if (!sim_state.check_is_valid(item.Key, item.Value)) return false;
        }
        return true;
    }
    //Shrimple BFS  this literally maps out every single possible plan.
    //our farthest back point is genininely our goal
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
            List<IActionAdjacent> potentialOptions = find_all_suitable_actions(current.c_state, allowed);
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
                    if (worldstate_validation(valid_node.c_state, start)) return visited;
                }
            }
            // consider breaking when queue gets too long and if there are no options left
        } while (queue.Count > 0 && nm < 25);
        return null;
    }
    //change input 
    private world_states mutate_state(world_states state, Action input)
    {
        foreach (var req in input._impact)
        {
            state.add_state(req.Key, req.Value);
        }
        return state;
    }

    //tree traversal and finish planner;

    public void Planner(List<Goal> goals, List<Action> allowed_actions)
    {
        //order by ascending
        IOrderedEnumerable<Goal> ordered_goals = goals.OrderBy(goal => goal.Priority);
        world_states simulated_worldstate = current_worldstate;
        foreach (var goal in ordered_goals)
        {
            List<Node> tree = discover_tree(simulated_worldstate, goal, allowed_actions);
            if (create_basic_plan(tree, simulated_worldstate) != null) ;



        }
    }
    
    
    
}


    
