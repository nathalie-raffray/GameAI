using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T2 : GoapAction
{
    //Trader 2: Takes 2 Turmeric units and gives you 1 Saffron unit.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */

    public override string actionName() { return "Trade with T2"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[0] >=2) ) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if (n.caravan[0] >= 2) return true;
        return false;
    }

    //public override void updateGameState()
    //{
    // GameManager.inventoryState[1] += 1;
    // GameManager.inventoryState[0] -= 2;
    //}
    public override void updateNewInv(int[] newInv)
    {
        newInv[1] += 1;
        newInv[0] -= 2;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[0] -= 2;
        n.inventory[0] += 2;
    }

   // public override int[] createNewCaravan(int[] prevState) { return prevState; }


}
