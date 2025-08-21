using System;
using System.Collections;
using System.Collections.Generic;
using base_move_classes;
using UnityEngine;
using init;
using Production;
using Unity.Collections;
using Action = Production.Action;
public class fight : MonoBehaviour
{ 
    private Dictionary<string, basic_move> moves;
    public List<String> attack_combo;
   void Awake()
   {
      moves = basic_init.create(moves);
      
      
   }

   // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame


    public IEnumerator fight_opponent(List<Action> actions)
    {
        List<basic_move> mvs = new List<basic_move>();
   /*     foreach (Action act in actions)
        {
           mvs.Add(moves[act.Name]); 
        }*/
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
            }
            mvs.RemoveAt(0);
        } while (actions.Count > 0);
    }
    
    
    
}
