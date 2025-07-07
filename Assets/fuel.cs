using UnityEngine;

public class fuel : MonoBehaviour
{

    public float fuzzy_gas;
    public float gas;

    private bool moving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gas = 5.0f;
    }

    // Update is called once per frame 
    void Update()
    {
        moving = gameObject.GetComponent<basics>().moving;

        if (moving && gas > 0f)
        {
            gas -= Time.deltaTime * 0.5f;
        }
        else
        {
            gameObject.GetComponent<basics>().moving = false;
        }

        fuzzy_gas = return_fuel_check_priority(gas);
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
