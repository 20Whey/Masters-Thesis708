using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using base_move_classes;
using Production;
using Sensors;



namespace init
{

  public class basic_init
  {
 
      public Dictionary<string, basic_move> create(Dictionary<string,  basic_move> moves)
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
      public Factories.BeliefFactory init_belief_factory()
      {
        var bf =  new Factories.BeliefFactory(null);
        bf.add_location_belief("close_to_enemy", simple_game.get_closest_target(bf.Agent.this_ob).transform.position,0.5f);
        bf.add_belief("enemy_is_stunned", ()=> simple_game.get_closest_target(bf.Agent.this_ob).GetComponent<basic_character>().stunned);
        return bf;
      }
      public Factories.ActionFactory init_action_factory()
      {
          var af =  new Factories.ActionFactory();
          return af;
          
      }
      public Factories.GoalFactory  init_goal_factory()
      {
          var go =  new Factories.GoalFactory();
          return go;
      }
  }

}