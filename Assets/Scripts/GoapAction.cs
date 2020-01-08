using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual string actionName(){ return "";}
    public virtual string actionName2(Node n){ return "";}

    public abstract bool checkPreconditions(Node n); //returns true if the inventory contains the necessary elements to trade AND/OR other preconditions
    public virtual bool checkPreconditionsAtCaravan(Node n) { return false;  } //returns true if the caravan contains the necessary elements to trade (will return false for 
                                                                                //'drop shit off' and 'pick shit up' actions)

    public virtual int[] createNewInventoryDropOff() { return null; }
    public virtual int[] createNewCaravanDropOff(Node current) { return null; }

    public virtual void updateNewInv(int[] newInv) { }
    public virtual void fillInventoryToTrade(Node n) { }
    public virtual void updateGameState(Node n) {
        GameM.caravanState = n.caravan;
        GameM.inventoryState = n.inventory;
    }
    public virtual void updateGameStateCaravan(int[] currentState, int[] currentInventory) { }

   // public virtual int[] createNewCaravan(int[] prevState) { return prevState; }
    public int[] createNewInv(int[] prevInv)
    {
        int[] newInv = new int[prevInv.Length];
        for (int i = 0; i < prevInv.Length; i++)
        {
            newInv[i] = prevInv[i];
        }
        updateNewInv(newInv);

        return newInv;
    }

    

    public bool inventoryFull(int[] currentInventory)
    {
        int qty = 0;
        for(int i = 0; i < currentInventory.Length; i++)
        {
            qty += currentInventory[i];
        }
        if (qty > 3) return true;
        return false;
    }

    public bool inventoryFullTum(int[] currentInventory)
    {
        int qty = 0;
        for (int i = 0; i < currentInventory.Length; i++)
        {
            qty += currentInventory[i];
        }
        if (qty > 2) return true;
        return false;
    }



}
