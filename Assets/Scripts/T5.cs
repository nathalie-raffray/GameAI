using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T5 : GoapAction
{
    // Trader 5: Takes 1 Cardamom and 1 Turmeric and gives you 1 Cloves unit.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T5"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[0] >= 1) && (n.inventory[2] >= 1)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if (n.caravan[0] >= 1 && n.caravan[2] >= 1) return true;
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[4] += 1;
        newInv[0] -= 1;
        newInv[2] -= 1;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[0] -= 1;
        n.caravan[2] -= 1;
        n.inventory[0] += 1;
        n.inventory[2] += 1;
    }
}
