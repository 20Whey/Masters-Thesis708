using System;
using System.Collections.Generic;
using UnityEditor.Embree;
using UnityEngine;

public class setup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector3 placement_position;
    public GameObject simulation;
    public List<Production.Action> plan;
    public basic_character basic_character;
    public bool is_sim_setup;
    public bool is_simulation_finished;
    [Serializable]
    public struct weight_obj
    {
        string name;
        float value;
    }
    [SerializeField]
    public weight_obj[] weights;

    void Awake()
    {
        if (simulation == null) simulation = Resources.Load(transform.parent.GetComponent<setup>().name) as GameObject;
        
        if (transform.parent != null) create_and_ready_sim(new Vector3(transform.parent.transform.position.x, 0,0));
        else create_and_ready_sim(new Vector3(0,0,0)); //setup root
        //grab sim
       
    }
    
    public void create_and_ready_sim(Vector3 position)
    {
    GameObject placed_sim = Instantiate(simulation, position, Quaternion.identity);
    placed_sim.transform.SetParent(transform);
    basic_character = gameObject.GetComponentInChildren<basic_character>();
    is_simulation_finished = false;
    //
  
    //  weights = new weight_obj[placed_sim.transform.] 

    }

    void Update()
    {
        if (basic_character.plan != null && is_sim_setup == false)
        {
            gameObject.GetComponentInChildren<fight>().signal = true;
            is_sim_setup = true;
        }
    }



}