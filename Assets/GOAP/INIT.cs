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
        bfact.add_location_belief("enemy_should_be_close",simple_game.get_closest_target(gameObject).transform.position, 0.5f);
        
        
        
        
        
        
        
      //  bfact.add_belief();
        
        
        
        
    }

    
    
    
    
}
