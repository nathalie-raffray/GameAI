using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShitOff : GoapAction
{
    // Start is called before the first frame update
    public override string actionName() { return "Drop All Spices Off At Caravan"; }
    public override bool checkPreconditions(Node n)
    {
        if (!n.atCaravan()) return true; // && (n.inventory[0] >= 2)
        return false;
    }

    public override int[] createNewCaravanDropOff(Node current) {
        int[] newCaravan = new int[current.caravan.Length];

        for (int i = 0; i<current.caravan.Length; i++)
        {
            newCaravan[i] = current.caravan[i] + current.inventory[i];
        }
        return newCaravan;
     }
    public override int[] createNewInventoryDropOff()
    {
        return new int[7];
    }
}
