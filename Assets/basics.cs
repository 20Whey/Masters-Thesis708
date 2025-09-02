using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.IO;
using Unity.Mathematics;


namespace base_move_classes
{
    
    public enum move_types{
    basic = 0,
    stun = 1,
    push = 2,
    block = 3
    }
    public class basic_move 
    {
        public move_types move_type;
        public string name;
        public int damage;
        public float cooldown;
        public basic_move self;
        public bool is_hot;
       
       public virtual basic_move setup(string nm, int dam, float cd)
       {
           move_type =  move_types.basic;
           name = nm;
           damage = dam;
           cooldown = cd;
           self = this;
           return self;
       }
       public virtual void do_move(basic_character us)
       {
           if (!is_hot)
           {
                us.target.GetComponent<basic_character>().health -= damage;
               self.is_hot = true;
           }
       }
    }

    public class stun_move : basic_move
    {
        public float stuns_dur;
        public override basic_move setup(string nm,int dam, float cd)
        {
            move_type =  move_types.stun;
            stuns_dur = cd / 2;
            return base.setup(nm, dam, cd);
        }

        public override void do_move(basic_character target)
        {
            if (!is_hot)
            {
                target.stunned = true;
                target.timer = stuns_dur; 
                target.health -= damage;
                self.is_hot = true;
            }
            
        }
    }
    public class push_move : stun_move
    {
        public float push_dist;
        public override basic_move setup(string nm,int dam, float cd)
        {
            move_type =  move_types.push;
            push_dist = cd * 1.5f;
            stuns_dur = cd * 0.1f;
            cooldown = cd;
            damage = dam;
            self = this;
            return self;
        }
        public override void do_move(basic_character target)
        {
            target.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * push_dist, ForceMode2D.Impulse);
            base.do_move(target);
        }
    }
    public class block_move : basic_move
    {
        public float block_dur;
        public override basic_move setup(string nm, int dam, float cd)
        {
            this.block_dur = cd * 0.9f;
            return base.setup(nm, dam, cd);
        }
        public override void do_move(basic_character target)
        {
            move_type =  move_types.block;
            target.blocking = true;
            target.timer = block_dur;
        }
    }
    
    
    
    
    
    
    
    

   


    

}