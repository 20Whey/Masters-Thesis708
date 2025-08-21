using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using static Production.Factories;
public class basic_character : MonoBehaviour
{
    public bool blocking;
    public bool stunned;
    public bool moving;
	public float health;
    public float timer;
    public bool struck;
    [CanBeNull] public Transform target;
    public character self;
    public List<Action> allowed_actions = new List<Action>();


    void Awake()
    {
        self = new character(gameObject);
    }
    void FixedUpdate()
    {
        if (moving) 
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, (Vector2)target.position, 0.5f);
        }
    }
}
