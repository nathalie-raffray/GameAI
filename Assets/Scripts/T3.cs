using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T3 : GoapAction
{
    // Trader 3: Takes 2 Saffron units and gives you 1 Cardamom unit.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public override string actionName() { return "Trade with T3"; }
    public override bool checkPreconditions(Node n)
    {
        if (!inventoryFull(n.inventory) && !n.atCaravan() && (n.inventory[1] >= 2)) return true;
        return false;
    }

    public override bool checkPreconditionsAtCaravan(Node n)
    {
        if (n.caravan[1] >= 2) return true;
        return false;
    }

    public override void updateNewInv(int[] newInv)
    {
        newInv[2] += 1;
        newInv[1] -= 2;
    }

    public override void fillInventoryToTrade(Node n)
    {
        n.caravan[1] -= 2;
        n.inventory[1] += 2;
    }

    //public override int[] createNewCaravan(int[] prevState) { return prevState; }
}
