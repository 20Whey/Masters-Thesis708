using System;
using System.Collections;
using System.Collections.Generic;
using base_move_classes;
using Goap;
using UnityEngine;
using init;
using Production;
using Unity.Collections;
using Action = Production.Action;
public class fight : MonoBehaviour
{
    public bool started;
    public bool signal = false;
    private Dictionary<string, basic_move> moves;
    public basic_character character;
   void Awake()
   {
      // character = gameObject.GetComponent<basic_character>();
      moves = basic_init.create(moves);
   }

    void Update()
    {
        if (signal)
        {
            StartCoroutine(run_action(character.current_world_state, character));
            signal = false;
        }
    }

    public IEnumerator run_action(world_states the_world, basic_character our_unit )
    {
        foreach (Action action in our_unit.plan)
        {
           Debug.Log("doing" +" "+  action.Name + " "+our_unit.plan.IndexOf(action) + " " + action.Cost);
           
            while (!validation(action, the_world))
            {
                //RUN THE ACTION 
                //THIS WILL WORK FOR MOVES
            //    Debug.Log(action.Name);
                if (moves.ContainsKey(action.Name))
                {
                    moves[action.Name].do_move(our_unit);
                    break;
                }
                else
                //we contain everything we need
                { 
                    action.Func(); 
                }
                yield return new WaitForSeconds(0.1f);
            }
         Debug.Log("passed " + " "+ action.Name);
        }
        //plan finished. tell parent
        our_unit.plan_finished = true;
    }


    public bool validation(Action current, world_states the_world)
    {
        foreach (var itm in current._impact) 
        {
            if (!(the_world.comparison(itm.Key, itm.Value))) return false;
        } 
        return true;
    }
    
}