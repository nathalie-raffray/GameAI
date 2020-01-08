using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class GoapPlanner : MonoBehaviour
{
   // private PriorityQueue<Node> possibleActions;
    private List<Node> pactions;
    private PriorityQueue<Node> actionQueue;

    private PriorityQueue<Node> bestNextAction; //this will keep track of which is the best action to take 
   // private Queue<GoapAction> plan;
    public Stack<Node> plan; //if intercepted by thief, clear plan
                             // private List<GoapAction> actions;
                             //maybe do stack <int, intarray> for picking shit up at caravan (what do i pick up?)

    public IList<GoapAction> actions; 
    
    
    // Start is called before the first frame update
    void Awake()
    {
        //possibleActions = new PriorityQueue<Node>();
        actionQueue = new PriorityQueue<Node>();
        if (actionQueue == null) Debug.Log("null");
        bestNextAction = new PriorityQueue<Node>();
        pactions = new List<Node>();

        actions = new ReadOnlyCollection<GoapAction>
    (new List<GoapAction> {
         gameObject.GetComponent<T1>(), gameObject.GetComponent<T2>(), gameObject.GetComponent<T3>(), gameObject.GetComponent<T4>(), gameObject.GetComponent<T5>(),
     gameObject.GetComponent<T6>(), gameObject.GetComponent<T7>(), gameObject.GetComponent<T8>(), gameObject.GetComponent<DropShitOff>(), gameObject.GetComponent<PickShitUp>()});

        //Node start = new Node(GameM.caravanState, GameM.inventoryState, -1, null);
       // buildPlan(start);
        //gameObject.GetComponent<PlayerManager>().plan = plan;

        

    }

    public int[] clone(int[] arr)
    {
        int[] cloneArr = new int[arr.Length];
        for (int i = 0; i<arr.Length; i++)
        {
            cloneArr[i] = arr[i];
        }
        return cloneArr;
    }

    Node handlePickUp(Node current)
    {
       // Debug.Log(current.inventory.ToString()); //should just be 0s

        Node m,n;
        for (int i = 0; i<actions.Count; i++)
        {
            if (actions[i].checkPreconditionsAtCaravan(current) == true) //check if i>=0 && i<8 since only trading actions are considered
                                                                                                    //also, i am passing currentState as 'currentInventory' since currentInventory is zero (bcuz i just dropped shit off)
            {
               
                n = new Node(clone(current.caravan), clone(current.inventory), i, current);
                actions[i].fillInventoryToTrade(n);
                n.inventory = actions[i].createNewInv(n.inventory);
                bestNextAction.Enqueue(n);
            }//REMEMBER TO CLEAR YOUR INVENTORY ONCE YOU DROP SHIT OFF
        }

        //Debug.Log("best Next action: "+bestNextAction.ToString());

        if (bestNextAction.isEmpty()) Debug.Log("Mission control, this is houston");
        //OK THE PROBLEM IS THAT YOU ADD CARAVAN PLUS INVENTORY SO Y0U GET SAME COST FOR EACH ACTION HERE
        n = bestNextAction.Dequeue();
        m = new Node(clone(current.caravan), clone(current.inventory), 9, current);
        n.previous = m;
       // Debug.Log("chosen best: " + n.ToString());
      
        m.inventory = clone(current.inventory);
        m.caravan = clone(current.caravan);
        actions[n.actionIndex].fillInventoryToTrade(m);
        //actions[n.actionIndex].fillInventoryToTrade(n);
     
        //n.actionIndex = 9; //this action index is 9 because it is a 'pick shit up' action
        actionQueue.Enqueue(m);
        actionQueue.Enqueue(n);
        //Debug.Log("chosen best after update: " + n.ToString());
        bestNextAction.Clear();
        return n;
    }
   

    public Stack<Node> buildPlan(Node start) //A star! //you should probably pass in the start node for this
    {
        bool searchFailed = false;
        Node current = start;
       // if (actionQueue == null) Debug.Log("null");
       actionQueue.Clear(); //empty queue for the new plan

        Node n;
        Node temp;
        int count = 0;
        while (!reachedGoal(current) && !searchFailed)
        {
            count++;
            //possibleActions.Clear();
            pactions.Clear();
            for (int i = 0; i<actions.Count; i++)
            {
               
                if (actions[i].checkPreconditions(current) == true && !current.visited[i])
                {
                    if(i == 9) //this is for picking shit up at the caravan
                    {
                        n = handlePickUp(current); //WHAT I CHANGED WAS SIMPLY HANDLEPICK UP. I MADE IT TAKE THE NEXT ACTION AS WELL AND IT WORKED
                       // possibleActions.Enqueue(n);
                       // pactions.Add(n);
                       // actionQueue.Enqueue(n);
                    }
                    else if (i == 8) //this for dropping shit off at the caravan
                    {
                        if(actions[i].createNewCaravanDropOff(current) == null ||
                             actions[i].createNewInventoryDropOff() == null){
                            Debug.Log("NULL AHOY");
                        }
                        if(current.actionIndex != 9)
                        {
                            n = new Node(actions[i].createNewCaravanDropOff(current), actions[i].createNewInventoryDropOff(), i, current);
                           // possibleActions.Enqueue(n);
                           // pactions.Add(n); //need this line for some reason
                            actionQueue.Enqueue(n);
                        }
                       // n = new Node(actions[i].createNewCaravanDropOff(current), actions[i].createNewInventoryDropOff(), i, current);
                        //actionQueue.Enqueue(n);

                    }
                    else //trading actions
                    {
                        n = new Node(current.caravan, actions[i].createNewInv(current.inventory), i, current);
                      //  possibleActions.Enqueue(n);
                       // pactions.Add(n); //need this line for some reason
                        actionQueue.Enqueue(n);
                    }
                    

                }
            }

           

            if (!actionQueue.isEmpty())
            {
                temp = current;
                current = actionQueue.Dequeue();
                pactions.Add(current);
                while (!actionQueue.isEmpty() && actionQueue.Peek().getNodeFn() == current.getNodeFn())
                {
                    if(actionQueue.Peek().getNodeFn2() < current.getNodeFn2())
                    {
                        current = actionQueue.Dequeue();
                        pactions.Add(current);
                   }
                   else
                    {
                        
                       pactions.Add(actionQueue.Dequeue());
                    }
                }
                temp.visited[current.actionIndex] = true;
                pactions.Remove(current);
                for(int i = 0; i<pactions.Count; i++)
                {
                    actionQueue.Enqueue(pactions[i]);
                }
            }
            
           
            else
            {
                searchFailed = true;
                Debug.Log("SEARCH FAILED");
            }

        }

        if (!searchFailed || searchFailed)
        {
            plan = current.backTrack(); //backtrack returns a stack of nodes with the first action to be performed at top of the stack.
            return plan;
            Node li;
            while (plan.Count != 0)
            {
                li = plan.Pop();
                Debug.Log(actions[li.actionIndex].actionName() + "| caravan: " + spToString(li.caravan) + " | inventory: " + spToString(li.inventory) + " | f(n)= "+ li.getNodeFn() + " | f2(n)= " + li.getNodeFn2());
            }
        }
        return null;


    }

    string spToString(int[] sp)
    {
        string s = "["+sp[0];
        for (int i = 1; i<sp.Length; i++)
        {
            s += ", " + sp[i];
        }
        s += "]";
        return s;
    }

    bool reachedGoal(Node n)
    {
        bool reached = true;
        for (int i = 0; i<n.caravan.Length; i++)
        {
            if (n.caravan[i] < 2) reached = false;
        }
        return reached;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
