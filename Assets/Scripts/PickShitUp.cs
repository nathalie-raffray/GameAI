using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickShitUp : GoapAction
{
    //  Trader 4: Takes 4 Turmeric units and gives you 1 Cinnamon.
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    string[] spi = new string[]{"Tum", "Saff", "Card", "Cinn", "Clo", "Pep", "Sum"};

    public override string actionName2(Node n) { 
        string s ="";
        bool pickedUpSomething = false;
        for(int i = 0; i<spi.Length; i++){
            if(n.inventory[i] != 0){
                pickedUpSomething = true;
                s= s+n.inventory[i]+" "+spi[i]+" ";
                //s.Concat(s, n.inventory[i], " ", spi[i], " ");
            }
        }
        if(!pickedUpSomething) s= "Spices ";
        s="Pick "+s+"Up At Caravan";
        //s.Concat("Pick ", s, "Up At Caravan");
        return s; 
    }
    public override bool checkPreconditions(Node n)
    {
        if (n.atCaravan()) return true; //n,actionIndex != 9 because we don't want the player to pick shit up twice. 
        return false;
    }

    public override void updateGameStateCaravan(int[] currentState, int[] currentInventory) {
        for(int i = 0; i<currentInventory.Length; i++) GameM.inventoryState[i] = currentInventory[i];
        for (int i = 0; i < currentState.Length; i++) GameM.caravanState[i] = currentState[i];

    }
    public override void updateNewInv(int[] newInv)
    {
         
    }

   // public override int[] createNewCaravan(int[] prevState) { return prevState; }
}
