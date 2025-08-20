using System;
using Unity.VisualScripting;
using UnityEngine;
using game_logic;

public class game_func_library : MonoBehaviour
{
    public static bool try_evade(GameObject us)
    { 
        if (!Convert.ToBoolean(UnityEngine.Random.Range(0, 1))) return false;
        var basics = us.GetComponent<basics>();
        var pthlis = basics.paths;
        int tmp;
        switch (pthlis.IndexOf(basics.path))
        {
            case 0:
                tmp = 1;
                break;
            case 1:
                tmp = (Convert.ToBoolean(UnityEngine.Random.Range(0, 1))) ? 1 : -1;
                break;
            case 2:
                tmp = 1;
                break;
            default:
                tmp = 0;
                break;
        }
        basics.path = pthlis[tmp];
        return true;
    }

    
    public static void switch_paths(int val, basics character)
    {
        switch (val)
        {
            case 0:
                character.path = character.paths[0];
                break;

            case 1:
                character.path = character.paths[1];
                break;

            case 2:
                character.path = character.paths[2];
                break;

            default:
                break;
        }

    }
    
    public static bool try_contest(GameObject us, GameObject target)
    {
        //33/33/33 crash or opp crash if within range
        var tmp = UnityEngine.Random.Range(0, 3);
        if (tmp <=1){
            spin_out(us);
            return false;
        }
        spin_out(target);
        return true;
    }

    public static void spin_out(GameObject character)
    {
        character.GetComponent<basics>().moving = false;
    }
    public static bool? switch_driving_direction()
    {
        throw new NotImplementedException();
    }
}
