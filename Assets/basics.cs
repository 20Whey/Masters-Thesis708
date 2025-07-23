using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.IO;
using Unity.Mathematics;

public class basics : MonoBehaviour
{

    private fuel fuel_script;
    public List<Transform[]> paths;
    public int path_num;
    public Transform[] path;
    public GameObject[] entry_points;
    public Vector2 target;
    public int target_count;
    public bool moving;

    private float start_speed;
    public float speed;

    void Awake()
    {
        fuel_script = gameObject.GetComponent<fuel>();
        start_speed = UnityEngine.Random.Range(0.15f, 0.2f);
        moving = false;
        path_num = UnityEngine.Random.Range(0, 3);
        paths = new List<Transform[]>();
        foreach (GameObject entry_point in entry_points)
        {
            Transform[] ourP = new Transform[9];
            for (var i = 0; i < 9; i++)
            {
                ourP[i] = entry_point.transform.GetChild(i);
            }
            paths.Add(ourP);
        }
    }

    void Start()
    {
        target_count = 0;
        target = paths.ElementAt(0)[0].position;
    }

    void Update()
    {
            if (fuel_script.nitro)
            {
                speed = start_speed + 0.15f;
            }
            else
            {
                speed = start_speed;
            }

            Vector2 pos = (Vector2)gameObject.transform.position;
            if (target_count > 9) target_count = 0; 
            if (moving)
            {
                switch_paths(path_num);
                gameObject.transform.position = Vector2.MoveTowards(pos, target, speed);
            }
            target = (Vector2.Distance(pos, target) > 0.3f) ? target : path[target_count++].position;  
        }
    private void switch_paths(int val)
    {
        switch (val)
        {
            case 0:
                path = paths[0];
                break;

            case 1:
                path = paths[1];
                break;

            case 2:
                path = paths[2];
                break;

            default:
                break;
        }

    }
    
    
}