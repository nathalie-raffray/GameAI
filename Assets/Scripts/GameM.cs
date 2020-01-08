using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameM
{
    /*{turmeric, saffron, cardamom, cinnamon, clove, pepper, sumac} */
    public static int[] caravanState = { 0, 0, 0, 0, 0, 0, 0 }; //keeps track of what is in caravan
    public static int[] inventoryState = { 0, 0, 0, 0, 0, 0, 0 };  //keeps track of what is in inventory
    public static float[] scalars = { 0.5f, 2, 5, 3, 7, 8, 11 };
    //public static Vector3 caravanPos = new Vector3(2.2f, 1.3f, 0.42f); //y will depend on the thief or the player
    public static Vector3 caravanPos = new Vector3(4.7f, 1.4f, -0.16f); //y will depend on the thief or the player
    public static Vector3 pos1 = new Vector3(-3.71f, 1.4f, -10.44f);
    public static Vector3 pos2 = new Vector3(-7.75f, 1.4f, -4.64f);
    public static Vector3 pos3 = new Vector3(-7.59f, 1.4f, 4.41f);
    public static Vector3 pos4 = new Vector3(-3.06f, 1.4f, 8.36f);
    public static Vector3 pos5 = new Vector3(6.79f, 1.4f, 8.65f);
    public static Vector3 pos6 = new Vector3(10.82f, 1.4f, 4.04f);
    public static Vector3 pos7 = new Vector3(10.9f, 1.4f, -4.97f);
    public static Vector3 pos8 = new Vector3(5.73f, 1.4f, -10.24f);

    public List<Vector3> pos = new List<Vector3> { pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8 };

   // public Vector3[] pos = new Vector3[8];
   
    

    //public static Vector3[] pos = { pos1, pos1, pos1, pos1, pos1, pos1, pos1, pos1, caravanPos, caravanPos };
    //public static int[] scalars = { 1, 2, 5, 3, 7, 8, 11 };
    // public static bool atCaravan;

    public static int[] goalState = { 2, 2, 2, 2, 2, 2, 2 };

    private static List<GoapAction> actions = new List<GoapAction>();

    public GameM()
    {
        actions.Add(new T1());
    }

    

}
