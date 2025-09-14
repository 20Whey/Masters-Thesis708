using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GA_namespce;
using static GA_namespce.GA;

public class GA_imp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Node;
    public int max_sim_number;
    public List<GA_Agent> tree;
    public GA_Agent root;
    public bool deploy;
    public bool start;
    
        //wait it's a linked list/pool bruhh
        public List<GA_Agent> construct_tree(){
            Queue<GA_Agent> queue = new Queue<GA_Agent>();
            List<GA_Agent> visited = new List<GA_Agent>();
            
            int count = 0;
            root = new GA_Agent(Node, 0);
            root.prepare_for_operations();
            queue.Enqueue(root);
            do
            {
                count++;
                var parent = queue.Dequeue();
                visited.Add(parent);
                
                for (int i = 0; i < 2; i++)
                {
                    //LOOP CONTENTS
                    GA_Agent new_agent = new GA_Agent(Node, count).create_from(parent);
                    //this already rebinds costs;
                    new_agent.prepare_for_operations();
                    create_deviants(new_agent, parent);
                    queue.Enqueue(new_agent);
                }
                
            } while (max_sim_number > visited.Count);

            Debug.Log(count);
            return visited;


        }
 
 
        void Update()
        {
            if (start) {
                tree = construct_tree();
                start = false;}


            if (deploy)
            {
           //  GameObject nd = Instantiate(Node);
           foreach (var itm in tree[tree.Count - 1].wrapped_costs)
           {
               Debug.Log(itm.Item1 +" "+ itm.Item2);
           }
           
             
             
            // nd.GetComponent<setup>().weights = ;
                deploy = false;
            }
           

}
       



        
        
    

}