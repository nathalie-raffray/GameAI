using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4 : GoapAction
{
    //  Trader 4: Takes 4 Turmeric units and gives you 1 Cinnamon.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T4"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[0] >= 4)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if (n.caravan[0] >= 4)
        {
            //Debug.Log("IM HERE");
            return true;
        }
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[3] += 1;
        newInv[0] -= 4;
    }

    public override void fillInventoryToTrade(Node n)
    {
        //Debug.Log("IM HERE");
        n.caravan[0] -= 4;
        n.inventory[0] += 4;
    }


}
