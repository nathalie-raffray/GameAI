using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int[] caravan; //whats in the caravan
    public int[] inventory;
    public int actionIndex; //keeps track of which action(transition) preceded this game state. 
    public Node previous;
    public bool[] visited; //visited actions

    public Node(int[] caravan, int[] inventory, int actionIndex, Node previous)
    {
        this.caravan = caravan;
        this.inventory = inventory;
        this.actionIndex = actionIndex;
        this.previous = previous;
        visited = new bool[10]; //everything initialized to false
    }

    public override string ToString()
    {
        return "caravan: "+ spToString(caravan) +" | inventory: "+spToString(inventory)+ "action: " + actionIndex + " | f(n)=" + getNodeFn() + '\n';
    }
    string spToString(int[] sp)
    {
        string s = "[" + sp[0];
        for (int i = 1; i < sp.Length; i++)
        {
            s += ", " + sp[i];
        }
        s += "]";
        return s;
    }

    public float getNodeFn() //implement heuristic
    {
        //need to change this for droppping off at caravan

        float fn = 0;
        for (int i = 0; i < caravan.Length; i++)
        {
            if (2 - (caravan[i] + inventory[i]) < 0) fn += 0;
            else fn += ((2 - (caravan[i] + inventory[i])) * GameM.scalars[i]);
        }
        //if (atCaravan()) return fn + 1; //increase cost to go to the caravan by 1 so that picking up more items is preferred
        return fn;
        //  return GoapPlanner.actions[actionIndex].getFn();
    }

    public float getNodeFn3() //implement heuristic
    {
        //need to change this for droppping off at caravan

        float fn = 0;
        for (int i = 0; i < caravan.Length; i++)
        {
            if (2 - (caravan[i] + inventory[i]) < 0) fn += 0;
            else fn += ((2 - (caravan[i] + inventory[i])) * GameM.scalars[i]);
        }
        //if (atCaravan()) return fn + 1; //increase cost to go to the caravan by 1 so that picking up more items is preferred
        return fn;
        //  return GoapPlanner.actions[actionIndex].getFn();
    }

    public float getNodeFn2() //implement heuristic
    {
        //need to change this for droppping off at caravan

        if (actionIndex == 8) return previous.getNodeFn2(); //if i just dropped stuff off

        float[] whatIsMissing = new float[7];
        float[] whatIsMissingWeighted = new float[7];
        float[] overflow = new float[7];

        float fn = 0;
        for (int i = 0; i< caravan.Length; i++)
        {
            if (2 - (caravan[i] + inventory[i]) < 0) overflow[i] = (caravan[i] + inventory[i]) - 2;
            else
            {
                whatIsMissing[i] = 2 - (caravan[i] + inventory[i]);
                whatIsMissingWeighted[i] = whatIsMissing[i] * GameM.scalars[i];
            }

           // if (2 - (caravan[i] + inventory[i]) < 0) fn += 0                               ;
            //else fn += ( (2 - (caravan[i] + inventory[i])) * GameManager.scalars[i] );
        }
        for(int i = 0; i<overflow.Length; i++)
        {
            if(overflow[i] > 0)
            {
                whatIsMissingWeighted = calculateNewWeights(i, overflow[i], whatIsMissingWeighted, clone(whatIsMissing));
            }
        }

        for(int i = 0; i<whatIsMissingWeighted.Length; i++)
        {
            fn += whatIsMissingWeighted[i];
        }


        //if (atCaravan()) return fn + 1; //increase cost to go to the caravan by 1 so that picking up more items is preferred
        return fn;
      //  return GoapPlanner.actions[actionIndex].getFn();
    }

    public Stack<Node> backTrack() //backtracking to get the plan, presumably from the goal state to the start state. 
    {
        Stack<Node> plan = new Stack<Node>();
        Node temp = this;
        while (temp.previous != null) //at the start state, temp.previous = null
        {
            plan.Push(temp);
            temp = temp.previous;
        }
        return plan;
    }


    public bool atCaravan()
    {
        if (actionIndex == 8) return true; //if my action index is 8, then I am dropping stuff off
        return false;
    }

    public float[] clone(float[] arr)
    {
        float[] cloneArr = new float[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            cloneArr[i] = arr[i];
        }
        return cloneArr;
    }

    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public float[] calculateNewWeights(int spice, float amount, float[] whatIsMissingWeighted, float[] whatIsMissing)
    {//ONLY PROBLEM HERE IS SUMAC -- SUBTRACTING TOO MUCH BECAUSE I SUBTRACT FOR BOTH RECIPES
        //if(amount >4) amount = 4;

        if (spice == 0) //turmeric, each turmeric has weight 0.5
        {
            for(int i = 0; i<amount; i++)
            {
                if (whatIsMissing[1] > 0) //saffron takes 2 turmerics
                {
                    whatIsMissingWeighted[1] -= 0.5f; /*1 saffron has weight 2 (because weight(saffron) = weight(2 turmerics) + weight(walking) = 0.5f*2 + 1
                                                                                                                               = 2 */
                    whatIsMissing[1] -= 0.5f;
                }
                else if (whatIsMissing[3] > 0) //cinammon takes 4 turmerics
                {
                    whatIsMissingWeighted[3] -= 0.5f;
                    whatIsMissing[3] -= 0.25f;
                }
                else if (whatIsMissing[4] > 0) //clove takes 1 turmerics
                {
                    whatIsMissingWeighted[4] -= 0.5f;
                    whatIsMissing[4] -= 1; //1 clove takes 1 turmeric
                }
                else if (whatIsMissing[5] > 0) //pepper takes 2 turmerics
                {
                    whatIsMissingWeighted[5] -= 0.5f;
                    whatIsMissing[5] -= 0.5f; //1 turmeric is 0.5 of the amount of turmerics pepper calls for
                }

            }
        }else if(spice == 1) //saffron
        {  /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
            for (int i = 0; i < amount; i++)
            {
                if (whatIsMissing[2] > 0) //cardamom takes 2 saffron
                {
                    whatIsMissingWeighted[2] -= 2f; 
                    whatIsMissing[2] -= 0.5f;
                }
                else if (whatIsMissing[5] > 0) //pepper takes 1 saffron
                {
                    whatIsMissingWeighted[5] -= 2f;
                    whatIsMissing[5] -= 1;
                }
                else if (whatIsMissing[6] > 0) //sumac takes 1 saffron
                {
                    whatIsMissingWeighted[6] -= 2f;
                    whatIsMissing[6] -= 1; //sumac takes 1 saffron
                }
                

            }
        }
        else if (spice == 2) //cardamom
        {  /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
            for (int i = 0; i < amount; i++)
            {
                if (whatIsMissing[4] > 0) //clove takes 1 cardamom
                {
                    whatIsMissingWeighted[4] -= 5f;
                    whatIsMissing[4] -= 1f;
                }
                else if (whatIsMissing[5] > 0) //sumac takes 4 cardamom
                {
                   // whatIsMissingWeighted[5] -= 5f;
                    //whatIsMissing[5] -= 0.25f;
                }
                


            }
        }
        else if (spice == 3) //cinnammon
        {  /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
            for (int i = 0; i < amount; i++)
            {
                if (whatIsMissing[5] > 0) //pepper takes 1 cinammon
                {
                    whatIsMissingWeighted[5] -= 3f;
                    whatIsMissing[5] -= 1f;
                }
                else if (whatIsMissing[6] > 0) //sumac takes 1 cinammon
                {
                    whatIsMissingWeighted[6] -= 3f;
                    whatIsMissing[6] -= 1f;
                }

            }
        }
        else if (spice == 4) //clove
        {  /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
            for (int i = 0; i < amount; i++)
            {
                if (whatIsMissing[6] > 0) //sumac takes 1 clove
                {
                    whatIsMissingWeighted[6] -= 7f;
                    whatIsMissing[6] -= 1f;
                }
             

            }
        }

        return whatIsMissingWeighted;
    }

}
