using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T8 : GoapAction
{
    // Trader 8: Takes 1 Saffron, 1 Cinnamon and 1 Cloves unit and gives you 1 Sumac unit.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T8"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[1] >= 1) && (n.inventory[3] >= 1) && (n.inventory[4] >= 1)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if ((n.caravan[1] >= 1) && (n.caravan[3] >= 1) && (n.caravan[4] >= 1)) return true;
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[6] += 1;
        newInv[1] -= 1;
        newInv[3] -= 1;
        newInv[4] -= 1;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[1] -= 1;
        n.caravan[3] -= 1;
        n.caravan[4] -= 1;
        n.inventory[1] += 1;
        n.inventory[3] += 1;
        n.inventory[4] += 1;
    }
}
