using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GA_namespce;
using static GA_namespce.GA;

public class GA_imp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Node;
    public int sim_number;
    public List<GA_Agent> tree;
    public GA_Agent root;

        //wait it's a linked list/pool bruhh
        public List<GA_Agent> construct_tree(){
            tree = new List<GA_Agent>();
            int count = 0;
            root = new GA_Agent(Node, 0);
            root.allowed_actions.ForEach((itm => Debug.Log(itm.Name)));
            tree.Add(root);
            
            
            
            while (count < sim_number)
            {   
                count++;
                GA_Agent new_agent = new GA_Agent(Node, count).create_from(tree[count-1]);
                new_agent.prepare_for_operations();
                new_agent.rebind_costs(create_deviants(new_agent,  tree[count-1]));
                print(new_agent);
            }
            return tree;

        }

        void Start()
        {

            construct_tree();

        }
       



        
        
    

}