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

            moves.Add("Shove", new stun_move() { }.setup("Shove", 0, 0.5f));
            moves.Add("bamboozle", new stun_move { }.setup("bamboozle", 0, 2f));
            moves.Add("follow_up_strike", new basic_move { }.setup("follow_up_strike", 3, 0.4f));

            moves.Add("Round_House", new basic_move { }.setup("Round_House", 2, 0.4f));
            moves.Add("guard_up", new block_move() { }.setup("guard_up", 0, 1.2f));
            moves.Add("counter", new stun_move() { }.setup("counter", 3, 0.7f));
            return moves;
        }
        public static Factories.BeliefFactory init_belief_factory(character slf, manager singleton)
        {
            var bf = new Factories.BeliefFactory(slf);
            var us = bf.Agent.this_ob.GetComponent<basic_character>();

            bf.add_location_belief("close_to_enemy", simple_game.get_closest_target(bf.Agent.this_ob).transform.position,
            0.01f);

            bf.add_belief("moving", () => bf.Agent.this_ob.GetComponent<basic_character>().moving);

            bf.add_belief("enemy_exists", () => simple_game.evaluate(simple_game.get_closest_target(bf.Agent.this_ob)));

            bf.add_belief("starting_combo", (() => bf.Agent.this_ob.GetComponent<basic_character>().started_combo));

            bf.add_belief("is_opponent_stunned", (() => simple_game.is_opponent_stunned(us)));
            
            bf.add_belief("is_target_set", (() => simple_game.evaluate(us.target)));
            foreach (var itm in bf.Beliefs)
            {
                if (!singleton.global_beliefs.ContainsKey(itm.Key))
                {
                    singleton.global_beliefs.Add(itm.Key, itm.Value);
                
                        var bel = new manager.displayed_beliefs();
                        bel.key = itm.Key;
                        bel.condition = itm.Value._condition();
                        singleton.display_beliefsfr.Add(bel);
                }
                
            }
            return bf;
        }
        public static Factories.ActionFactory init_action_factory(Factories.BeliefFactory belief_factory, basic_character input)
        {
            var af = new Factories.ActionFactory();
            var us = belief_factory.Agent.this_ob.GetComponent<basic_character>();
            Transform? opponent = belief_factory.Agent.this_ob.GetComponent<basic_character>().target;
            basic_character? opponentdat = opponent != null ? opponent.GetComponent<basic_character>() : null;
            
            
            
            af.add_action_to_list("Straight", () => null,0.5f, new []
            {
             ("close_to_enemy", true),
             ( "starting_combo", false)
            // {belief_factory.grab_belief("starting_combo"), false}
            }, new []
            {
            ( "starting_combo", true),
            });

            af.add_action_to_list("stop_moving", () => simple_game.set_moving(false, us)
            ,0.5f,
            new []
            {
            ( "moving", true )
            }, new [] {
            ("moving", false ) });
            
            af.add_action_to_list("Shove", () => null, 
            0.5f,new []{
            ( "close_to_enemy", true),
            // {belief_factory.grab_belief("starting_combo"), false}
            }, new []
            {
            ("starting_combo", true),
            ("opponent_stunned", true)
            });

            
            af.add_action_to_list("Kick", () => null,0.5f, new []
            {
            ("close_to_enemy", true),
            ("is_target_set", true),
            ("starting_combo", false)
            // {belief_factory.grab_belief("starting_combo"), false}
            }, new []
            {
            ( "starting_combo", true),
            });
            
            af.add_action_to_list("Round_House", () => null, 
            0.5f,new []{
            ("close_to_enemy", true),
            ("starting_combo", true)
            // {belief_factory.grab_belief("starting_combo"), false}
            } , new []
            {
            ( "starting_combo", false) ,
            ("enemy_exists", false),
            });
            
            af.add_action_to_list("move_to_enemy", () => simple_game.set_moving(true, us),
            0.5f,new []{
            ("is_target_set", true),
            ("close_to_enemy", false),
            },
            new []{
            ("close_to_enemy", true),
            ( "moving", true) 
            });
            
            af.add_action_to_list("find_enemy", 
             () => simple_game.set_closest_target(us.self.this_ob), 
            0.5f,new [] {
            ("enemy_exists", true),
            ("is_target_set", false)
           }, new []
           {
           ("is_target_set", true)
           });

            af.add_action_to_list("bamboozle", () => (opponentdat.stunned = true),0.5f,new[]{
                ("close_to_enemy", true),
                },new [] {
                ("is_opponent_stunned", true),
                ("starting_combo", true)
                });

            af.add_action_to_list("follow_up_strike", (() => null),0.5f, new[]{
             ("is_opponent_stunned", true),
             ("is_target_set", true),
             ("close_to_enemy", true) 
             }
            ,new []
            {
             ("starting_combo", false ),
            ("enemy_exists", false),
             ("is_opponent_stunned", false) 
            }
            );
            
           // af.add_action_to_list("finish_off_enemy", (() => singleton.Instance.Destroy(opponent.gameObject)), 0.5f);
            return af;
        }
    

    public static Factories.GoalFactory  init_goal_factory(Factories.BeliefFactory  belief_factory, manager singleton_ref)
      {
          var go =  new Factories.GoalFactory();
          /*  go.add_goal("kill_enemy", new KeyValuePair<string, bool>("is_enemy_alive", false), .5f, 
          singleton_ref,"is_enemy_alive");*/
          
          go.add_goal("kill_all_enemies", new KeyValuePair<string, bool>("enemy_exists", false), .5f,
          singleton_ref,"enemy_exists");
          
          
          
          return go;
          
          
          
          
          
      }
  }

}