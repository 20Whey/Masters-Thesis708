using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using base_move_classes;
using Goap;
using Production;
using Sensors;
using Unity.Mathematics;



namespace init
{

    public class basic_init
    {
        public static Dictionary<string, basic_move> create(Dictionary<string, basic_move> moves)
        {
            moves = new Dictionary<string, basic_move>();
            moves.Add("Straight", new basic_move { }.setup("Straight", 1, 0.5f));
            moves.Add("Kick", new basic_move { }.setup("Kick", 2, 0.8f));

            moves.Add("shove", new push_move { }.setup("shove", 1, 0.5f));
            moves.Add("bamboozle", new stun_move { }.setup("bamboozle", 0, 2f));
            moves.Add("follow_up_strike", new basic_move { }.setup("follow_up_strike", 3, 0.4f));

            moves.Add("Round_House", new basic_move { }.setup("Round_House", 2, 0.4f));
            moves.Add("guard_up", new block_move() { }.setup("guard_up", 0, 1.2f));
            moves.Add("counter", new stun_move() { }.setup("counter", 3, 0.7f));
            return moves;
        }
        public static Factories.BeliefFactory init_belief_factory(character slf, singleton singleton)
        {
            var bf = new Factories.BeliefFactory(slf);
            var us = bf.Agent.this_ob.GetComponent<basic_character>();

            bf.add_location_belief("close_to_enemy", simple_game.get_closest_target(bf.Agent.this_ob).transform.position,
            0.01f);

            bf.add_belief("moving", () => rushed_additions.convert_bool(bf.Agent.this_ob.GetComponent<basic_character>().moving));

            bf.add_belief("is_enemy_alive", () => rushed_additions.convert_bool(simple_game.evaluate(simple_game.get_closest_target(bf.Agent.this_ob))));

            bf.add_belief("starting_combo", (() => rushed_additions.convert_bool(bf.Agent.this_ob.GetComponent<basic_character>().started_combo)));

            bf.add_belief("is_opponent_stunned", (() => rushed_additions.convert_bool(simple_game.is_opponent_stunned(us))));
            
            bf.add_belief("enemy_exists", (() => rushed_additions.convert_bool(simple_game.evaluate(us.target))));
            
            bf.add_numerical_belief("enemy_health",  simple_game.get_closest_target(us.gameObject).GetComponent<basic_character>().getHealth(),
            0.0f,  simple_game.get_closest_target(us.gameObject).GetComponent<basic_character>().getHealth, "<");
            
            foreach (var itm in bf.Beliefs)
            {
                if (!singleton.global_beliefs.ContainsKey(itm.Key))
                {
                    singleton.global_beliefs.Add(itm.Key, itm.Value);
                
                        var bel = new singleton.displayed_beliefs();
                        bel.key = itm.Key;
                        if (itm.Value._target_value != null) bel.value = itm.Value._target_value;
                        bel.condition = rushed_additions.convert_float(itm.Value._condition());
                        singleton.display_beliefsfr.Add(bel);
                        
                }
            }
            return bf;
        }
        public static Factories.ActionFactory init_action_factory(Factories.BeliefFactory belief_factory)
        {
            var af = new Factories.ActionFactory();
            var us = belief_factory.Agent.this_ob.GetComponent<basic_character>();
            Transform? opponent = belief_factory.Agent.this_ob.GetComponent<basic_character>().target;
            basic_character? opponentdat = opponent != null ? opponent.GetComponent<basic_character>() : null;
            
            
            af.add_action_to_list("Straight", () => null,0.5f, new []
            { 
             ("close_to_enemy", 1.0f),
            ( "moving", 0.0f ),
            ( "starting_combo", 0.0f )
            // {belief_factory.grab_belief("starting_combo"), 0.0f}
            }, new []
            {
            ("enemy_health", -2.0f),
            ( "starting_combo", 1.0f ),
            }, true, -1f);

            af.add_action_to_list("stop_moving", () => rushed_additions.convert_bool(simple_game.set_moving(false, us))
            ,0.5f,
            new []
            {
            ( "moving", 1.0f )
            }, new [] {
            ("moving", 0.0f ) });
            
            af.add_action_to_list("move_to_enemy", () => rushed_additions.convert_bool(simple_game.set_moving(true, us)),
            0.5f,new []{
            ("enemy_exists", 1.0f),
            ( "close_to_enemy", 0.0f),
            ("is_enemy_alive", 1.0f),

            },
            new []{
            ("close_to_enemy", 1.0f),
            ( "moving", 1.0f) 
            });
            
            af.add_action_to_list("Kick", () => null, 
            0.5f,new []{
            ( "close_to_enemy", 1.0f ),
            ( "starting_combo", 1.0f )
            // {belief_factory.grab_belief("starting_combo"), 0.0f}
           } , new []
            {
              ("enemy_health", -2.0f),
            ( "starting_combo", 0.0f)
            }, true, -1f);
            
            af.add_action_to_list("find_enemy",
            () => rushed_additions.convert_bool(simple_game.evaluate(us.target = simple_game.get_closest_target(us.self.this_ob))),
            0.5f, new[]
            {
            ("enemy_exists", 0.0f)
            }, new[]
            {
            ("enemy_exists", 1.0f),
            ("is_enemy_alive", 1.0f)
            
            },false);
/*
            af.add_action_to_list("bamboozle", () => rushed_additions.convert_bool(opponentdat.stunned = true), 0.5f, new[]
            {
            ("close_to_enemy", 1.0f),
            ("is_enemy_alive", 1.0f),
            ("is_opponent_stunned", 0.0f)
            }, new[]
            {
            ("is_opponent_stunned", 1.0f),
            ("starting_combo", 1.0f)
            }, true);

            af.add_action_to_list("follow_up_strike", (() => null),0.5f, new[]{
             ("is_opponent_stunned", 1.0f),
             ("is_enemy_alive", 1.0f),
             ("enemy_health", -4.0f),
             ("close_to_enemy", 1.0f) 
             }
            ,new []
            {
             ("starting_combo", 0.0f ),
             ("is_opponent_stunned", 0.0f) 
            }, true
            );*/
            //shadow of war style
            af.add_action_to_list("killing_blow", (() => null), 0.1f, new[]
            {
            ("enemy_health", 0.0f),
            ("is_enemy_alive", 1.0f),
            }, new[]
            {
            ("is_enemy_alive", 0.0f)
            }, false);
            
            // af.add_action_to_list("finish_off_enemy", (() => singleton.Instance.Destroy(opponent.gameObject)), 0.5f);
            return af;
        }
    

    public static Factories.GoalFactory  init_goal_factory(singleton singleton_ref)
      {
          var go =  new Factories.GoalFactory();
          go.add_goal("kill_enemy", new KeyValuePair<string, float>("is_enemy_alive", 0.0f), .5f, 
          singleton_ref,"enemy_health");
          return go;
      }
  }

}