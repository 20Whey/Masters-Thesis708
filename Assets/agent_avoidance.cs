using System;
using Unity.VisualScripting;
using UnityEngine;

public class agent_avoidance : MonoBehaviour
{


    public void try_crash(GameObject us, GameObject target)
    {
        //try to prompt opp reaction
        
        
    }

    public bool try_evade(GameObject us)
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

    public bool? try_contest(GameObject us, GameObject target)
    {
        //33/33/33 crash, opp crash or nothing 
        var tmp = UnityEngine.Random.Range(0, 2);
        if (tmp!=2)
            if (!Convert.ToBoolean(tmp))
            {
                spin_out(us);
                return false;
            }

            else
            {
                spin_out(target);
                return true;
            }

        return null;
    }

    public void spin_out(GameObject character)
    {
        character.GetComponent<basics>().moving = false;
    }
}
