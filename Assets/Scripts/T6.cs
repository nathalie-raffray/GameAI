using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T6 : GoapAction
{
    // Trader 6: Takes 2 Turmeric, 1 Saffron and 1 Cinnamon and gives 1 Pepper unit
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T6"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[0] >= 2) && (n.inventory[1] >= 1) && (n.inventory[3] >= 1)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if ((n.caravan[0] >= 2) && (n.caravan[1] >= 1) && (n.caravan[3] >= 1)) return true;
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[5] += 1;
        newInv[0] -= 2;
        newInv[1] -= 1;
        newInv[3] -= 1;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[0] -= 2;
        n.caravan[1] -= 1;
        n.caravan[3] -= 1;
        n.inventory[0] += 2;
        n.inventory[1] += 1;
        n.inventory[3] += 1;
    }
}
