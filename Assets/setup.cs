    using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Production;
using UnityEditor.Embree;
using UnityEngine;
using UnityEngine.Serialization;

public class setup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 placement_position;
    public GameObject simulation;
    public List<Production.Action> plan;
    public int id;
    [FormerlySerializedAs("be_silly")] public bool run_plan;
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
        
       /* if (simulation == null)
        {
            simulation = Resources.Load(transform.parent.GetComponent<setup>().name) as GameObject;
        }*/
      //  if (transform.parent != null) create_and_ready_sim(new Vector3(transform.parent.transform.position.x+10, 0,0), id);
        create_and_ready_sim(); //setup root
        basic_character = gameObject.GetComponentInChildren<basic_character>();
        //grab sim
    }
    
    public void create_and_ready_sim()
    { 
        //transform.position = 
    //GameObject placed_sim = Instantiate(simulation, new Vector3(0, 0,0), Quaternion.identity);
      //TAKE FIRST WEIGHTS AND APPLY THEM FOR GA VERYY IMPORTANTT
  
    is_sim_setup = false;
    is_simulation_finished = false;
    run_plan = false;

    }
    //create modified plan.
    public void plug_in_action_weights()
    {
        foreach (var item in weights)
        {                                      
            basic_character.actions.grab_Action(item.name).Cost = item.value;
          //  print(basic_character.actions.grab_Action(item.name).Name + " " + basic_character.actions.grab_Action(item.name).Cost );
        }
        
        basic_character.create_plan(basic_character.actions);
        is_sim_setup = true;
        run_plan = false;
    }
    
    void Update()
    {
       // if (basic_character.plan == null && !is_sim_setup)
        if(run_plan){
            Debug.Log("setup plan");
            plug_in_action_weights();
        }
        if (basic_character.plan != null && is_sim_setup)
        {
            plan = basic_character.plan;
            gameObject.GetComponentInChildren<fight>().signal = true;
            is_sim_setup = false;
        }

        if (basic_character.plan_finished)
        {
            is_simulation_finished = true;
            
        }
    }



}