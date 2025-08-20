using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using game_logic;
using Goap;
namespace Sensors
{
public class car_game : MonoBehaviour
{
    
    //can see closest car
    public static Transform get_closest_car(GameObject current_car)
    {
        GameObject containrer = GameObject.Find("Agents");
        int count = containrer.transform.childCount;
        List<Transform> collection = new List<Transform>();
        for (var i = 0; i < count; i++)
        {
            collection.Add(containrer.transform.GetChild(i));
        }
        collection = new List<Transform>(collection.OrderBy(item => Vector2.Distance(current_car.transform.position, item.transform.position)));
        collection.RemoveAt(0);
        return collection[0];
    }
    
    public static bool is_car_close_enough(GameObject current_car)
    {
      return Vector2.Distance(current_car.transform.position, get_closest_car(current_car).position) < 2 ? true : false;
    }
    public static bool is_car_in_right_lane(GameObject current_car, GameObject target)
    {
       return current_car.GetComponent<basics>().path_num == target.GetComponent<basics>().path_num ? true : false;
    }
    
    public static bool has_passed_race(int val)
    {
        if (val > 2) return true;
        return false;
    }
    

    
    
    
    public static void spin_out(GameObject character)
    {
        character.GetComponent<basics>().moving = false;
    }

    public static bool set_moving(bool input, basics self)
    {
        self.moving = input;
        return true;
    }

}

    
    
    
    
    
}
