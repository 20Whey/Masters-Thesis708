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
    private Dictionary<string, basic_move> moves;
    public basic_character character;
   void Awake()
   {
      // character = gameObject.GetComponent<basic_character>();
      moves = basic_init.create(moves);
      
      
   }
   
    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(run_action(character.local_worldstate, character));
    }

    public IEnumerator run_action(world_states the_world, basic_character our_unit )
    {
        foreach (Action action in our_unit.plan)
        {
            while (!validation(action, the_world))
            {
                //RUN THE ACTION 
                //THIS WILL WORK FOR MOVES
            //    Debug.Log(action.Name);
                if (moves.ContainsKey(action.Name))
                {
                    moves[action.Name].do_move(our_unit);
                }
                else
                //we contain everything we need
                { 
                    action.Func();
                }
                yield return new WaitForSeconds(0.1f);
                Debug.Log("doing" +" "+  action.Name);
            }
            Debug.Log("passed " + " "+ action.Name);
        }
    }


    public bool validation(Action current, world_states the_world)
    {
        foreach (var itm in current._impact) 
        {
            
       /*     Debug.Log(itm.Key); 
        Debug.Log(itm.Value);
        Debug.Log(the_world.states[itm.Key]);*/
            if (!the_world.comparison(itm.Key, itm.Value)) return false;
        }
        return true;
    }

/*
    public IEnumerator perform_actions(List<Action> actions)
    {
        List<Action> visited = new List<Action>();
        
   /*     foreach (Action act in actions)
        {
           mvs.Add(moves[act.Name]); 
        }
        do
        {
            var current = actions[0];
            if (moves.ContainsKey(current.Name))
            {
                var type = moves[current.Name].move_type;
                var cmove = moves[current.Name];
                     switch (type)
                     {
                         case move_types.block:
                             cmove = (block_move)cmove;
                             cmove.do_move(gameObject.GetComponent<basic_character>());
                             break;
                         default:
                             cmove.do_move(gameObject.GetComponent<basic_character>().target.gameObject
                             .GetComponent<basic_character>());
                             break;
                     }
                     yield return new WaitForSeconds(cmove.cooldown);
                 } else
                              {
                                  
                              } 
                             visited.Add(current);
                             actions.RemoveAt(0);
                
            }while (actions.Count > 0);
           
           
        } */
    }
    
    
    

