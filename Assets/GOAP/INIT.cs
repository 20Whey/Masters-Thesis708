using Production;
using Sensors;
using UnityEngine;

public class INIT : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Factories.BeliefFactory bfact;
    public Factories.ActionFactory afact;
    public Factories.GoalFactory agoal;
    
    
    
    void Start()
    {
        bfact.add_location_belief("is_enemy_close",simple_game.get_closest_target(gameObject).transform.position, 0.5f);
        afact.add_action_to_list();
        
        
        
        
        
        
      //  bfact.add_belief();
        
        
        
        
    }

    
    
    
    
}
