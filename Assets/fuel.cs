using UnityEngine;
using game_logic;
public class fuel : MonoBehaviour
{
    public bool needs_refuel;
    public float fuzzy_gas;
    public float gas;
    public bool nitro;
    private float multi;
    private bool moving;
    void Start()
    {
        multi = 0.5f;
        gas = 5.0f;
        nitro = false;
        needs_refuel = false;
    }
    void Update()
    {
        float mult;

        moving = gameObject.GetComponent<basics>().moving;
        if (nitro) { mult = multi + 0.1f; }
        else { mult = multi; }

        if (moving && gas > 0.1f) { gas -= Time.deltaTime * mult; } else { gameObject.GetComponent<basics>().speed = 0.01f; }

        fuzzy_gas = return_fuel_check_priority(gas);

        //if (fuzzy_gas > 2f) seek refuel
    }
    public int return_fuel_check_priority(float input)
    {
        switch (input)
        {
            case < 2f:
                return 2;
            case < 3f:
                return 1;
            case < 4f:
                return 0;
            default:
                return 5;
        }
    }
}
