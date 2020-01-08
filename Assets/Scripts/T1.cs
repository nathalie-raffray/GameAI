using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T1 : GoapAction
{
    //Trader 1: Gives you 2 turmeric units.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T1";  }
 
    public override bool checkPreconditions(Node n)
    {
        if(!inventoryFullTum(n.inventory) && !n.atCaravan()) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        return true;    
    }

    //public override void updateGameState(Node n)
    //{
    // GameManager.inventoryState[0] = (GameManager.inventoryState[0] + 2) % 4;
    // }
    public override void updateNewInv(int[] newInv)
    {
        newInv[0] = newInv[0] + 2;
    }

    public override void fillInventoryToTrade(Node n)
    {
        //do nothing since turmeric is freeee
    }

  //  public override int[] createNewCaravan(int[] prevState) { return prevState; }



    //  public override int[] createNewState(int[] prevState) //within this i must create a new array and return that one since the array is passed by reference and should be const
    //   {
    //      int[] newState = new int[prevState.Length];
    //     for(int i = 0; i< prevState.Length; i++)
    //     {
    //         newState[i] = prevState[i];
    //   }
    //    newState[0] = (newState[0] + 2) % 4;

    //   return newState;
    // }


}
