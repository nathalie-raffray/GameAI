using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T7 : GoapAction
{
    // Trader 7: Takes 4 Cardamom units and gives you 1 Sumac unit.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T7"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[2] >= 4)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if (n.caravan[2] >= 4) return true;
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[6] += 1;
        newInv[2] -= 4;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[2] -= 4;
        n.inventory[2] += 4;
    }
}
