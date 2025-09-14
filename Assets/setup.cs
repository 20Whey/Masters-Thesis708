using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Production;
using UnityEditor.Embree;
using UnityEngine;

public class setup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 placement_position;
    public GameObject simulation;
    public List<Production.Action> plan;
    public bool be_silly;
    public basic_character basic_character;
    public bool is_sim_setup;
    public bool is_simulation_finished;
    [Serializable]
    public struct weight_obj
    {
        public string name;
        public float value;
    }
    [SerializeField]
    public weight_obj[] weights;

    void Awake()
    {
        
        if (simulation == null)
        {
            simulation = Resources.Load(transform.parent.GetComponent<setup>().name) as GameObject;
        }
        
        if (transform.parent != null) create_and_ready_sim(new Vector3(transform.parent.transform.position.x+10, 0,0));
        else {create_and_ready_sim(new Vector3(0,0,0)); //setup root
        
        }
        //grab sim
    }
    
    public void create_and_ready_sim(Vector3 position)
    {
    GameObject placed_sim = Instantiate(simulation, position, Quaternion.identity);
    placed_sim.transform.SetParent(transform);
    basic_character = gameObject.GetComponentInChildren<basic_character>();  //TAKE FIRST WEIGHTS AND APPLY THEM FOR GA VERYY IMPORTANTT
  
    is_sim_setup = false;
    is_simulation_finished = false;
    be_silly = true;

    }
    //create modified plan.
    public void plug_in_action_weights()
    {
        foreach (var item in weights)
        {                                      
            basic_character.actions.grab_Action(item.name).Cost = item.value;
            print(basic_character.actions.grab_Action(item.name).Name + " "+ basic_character.actions.grab_Action(item.name).Cost );
        }
        basic_character.create_plan(basic_character.actions);
        is_sim_setup = true;
    }
    
    void Update()
    {
       // if (basic_character.plan == null && !is_sim_setup)
        if(be_silly){
            Debug.Log("setup plan");
            plug_in_action_weights();
            be_silly = false;
        }
        if (basic_character.plan != null && is_sim_setup)
        {
            gameObject.GetComponentInChildren<fight>().signal = true;
            is_sim_setup = false;
        }

        if (basic_character.plan_finished)
        {
            is_simulation_finished = true;
        }
    }



}