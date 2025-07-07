using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.IO;
public class basics : MonoBehaviour
{
    public List<Transform[]> paths;
    public int path_num;
    public Transform[] path;
    public GameObject[] entry_points;
    public Vector2 target;
    public int target_count;
    public bool moving = false;

    void Awake()
    {

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target_count = 0;
        target = paths.ElementAt(0)[0].position;
    }

    // Update is called once per frame
    void Update()
    {




        Vector2 pos = (Vector2)gameObject.transform.position;
        if (target_count > 9) target_count = 0;
        if (moving)
        {
            switch (path_num)
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
            gameObject.transform.position = Vector2.MoveTowards(pos, target, 0.2f);









        }
        target = (Vector2.Distance(pos, target) > 0.3f) ? target : path[target_count++].position;
    }
}
