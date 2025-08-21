using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using base_move_classes;
using Goap;
namespace Sensors
{

    public class simple_game : MonoBehaviour
    {

        //can see closest car
        public static Transform get_closest_target(GameObject current_car)
        {
            GameObject containrer = GameObject.Find("Agents");
            int count = containrer.transform.childCount;
            List<Transform> collection = new List<Transform>();
            for (var i = 0; i < count; i++)
            {
                collection.Add(containrer.transform.GetChild(i));
            }
            collection = new List<Transform>(collection.OrderBy(item =>
            Vector2.Distance(current_car.transform.position, item.transform.position)));
            collection.RemoveAt(0);
            return collection[0];
        }
        public static bool evaluate(Transform targ)
        {
            if (targ != null) return true;
            return false;
        }
        public static bool is_target_close_enough(GameObject us)
        {
            return Vector2.Distance(us.transform.position, get_closest_target(us).position) < 2 ? true : false;
        }

        public static bool set_moving(bool input, basic_character self)
        {
            self.moving = input;
            return true;
        }
        public static bool am_i_guarding(basic_character self)
        {
            return self.blocking;
        }
        public static bool is_opponent_stunned(basic_character self)
        {
            return self.stunned;
        }
        public static bool did_opponent_hit_my_guard(basic_character self)
        {
            if (self.blocking && self.struck) return true;
            return false;
        }
        public static bool am_i_stunned(basic_character self)
        {
            if (self.stunned) return true;
            return false;
        }

        
        /*   public static bool starting_combo(basic_character self)
           {

           }
           public static bool finishing_combo(basic_character self)
           {

           }*/
    
}

    
    
    
    
    
}
