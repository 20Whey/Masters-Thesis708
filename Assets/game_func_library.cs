using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using base_move_classes;
using Goap;
using Production;
using Sensors;



namespace init
{

  public class basic_init
  {
      public static Dictionary<string, basic_move> create(Dictionary<string,  basic_move> moves)
      {
          moves = new Dictionary<string,  basic_move>();
          moves.Add("Straight", new basic_move { }.setup("Straight", 1, 0.5f));
          moves.Add("Kick", new basic_move {}.setup("Kick", 2, 0.8f));
          
          moves.Add("shove", new push_move{}.setup("shove", 1, 0.5f));
          moves.Add("bamboozle", new stun_move{}.setup("bamboozle", 0, 2f));
          moves.Add("follow_up_strike", new basic_move{}.setup("follow_up_strike",3, 0.4f) );
          
          moves.Add("Round_House", new basic_move{}.setup("Round_House", 2, 0.4f));
          moves.Add("guard_up", new block_move() {}.setup("guard_up", 0, 1.2f));
          moves.Add("counter", new stun_move(){}.setup("counter", 3, 0.7f));
          return moves;
      }
      public static Factories.BeliefFactory init_belief_factory(character slf)
      {
        var bf =  new Factories.BeliefFactory(slf);
        bf.add_location_belief("close_to_enemy", simple_game.get_closest_target(bf.Agent.this_ob).transform.position,0.5f);
        
        bf.add_belief("moving", () => bf.Agent.this_ob.GetComponent<basic_character>().moving);
        
        bf.add_belief("is_enemy_alive", () => simple_game.evaluate(simple_game.get_closest_target(bf.Agent.this_ob)));
        
        bf.add_belief("starting_combo", (() => bf.Agent.this_ob.GetComponent<basic_character>().started_combo));

        foreach (var itm in bf.Beliefs)
        {
            if (!singleton.Instance.global_beliefs.ContainsKey(itm.Key)) singleton.Instance.global_beliefs.Add(itm.Key, itm.Value);
        }
        return bf;
      }
      public static Factories.ActionFactory init_action_factory(Factories.BeliefFactory belief_factory)
      {
          var af =  new Factories.ActionFactory();
          af.add_action_to_list("Straight",() => null, new Dictionary<string, bool>()
          {
          { "close_to_enemy", true},
          { "moving", false},
          { "starting_combo", false}
         // {belief_factory.grab_belief("starting_combo"), false}
          }, new Dictionary<string, bool>()
          {
          {"starting_combo", true},
          } );
          
          af.add_action_to_list("stop_moving", () => simple_game.set_moving(false, belief_factory.Agent.this_ob.GetComponent<basic_character>())
          , new Dictionary<string, bool>()
          {
          {"moving", true}
          }, new Dictionary<string, bool>(){{"moving", false}});
          
          af.add_action_to_list("Kick",() => null, new Dictionary<string, bool>()
          {
          { "close_to_enemy", true},
          {"moving", false},
          { "starting_combo", true}
          // {belief_factory.grab_belief("starting_combo"), false}
          }, new Dictionary<string, bool>()
          {
          {"starting_combo", false},
          {"is_enemy_alive", false}
          });
          
          af.add_action_to_list("move_to_enemy", () => simple_game.set_moving(true, belief_factory.Agent.this_ob.GetComponent<basic_character>()), 
          new Dictionary<string, bool>()
          {
          { "close_to_enemy", false},
          {"is_enemy_alive", true}
          }, 
          new Dictionary<string, bool>() { {"close_to_enemy", true}, {"moving", true}
          } );
          return af;
      }
      public static Factories.GoalFactory  init_goal_factory(Factories.BeliefFactory  belief_factory)
      {
          var go =  new Factories.GoalFactory();
          go.add_goal("kill_enemy", new KeyValuePair<string, bool>("is_enemy_alive", false), .5f, 
          "is_enemy_alive");
          return go;
      }
  }

}