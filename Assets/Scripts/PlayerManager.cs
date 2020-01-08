using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    // public Stack<int> plan;

    private Vector3 velocity;

    public static bool pause = false;

    public GameObject TuInv, TuCar, SaInv, SaCar, CaInv, CaCar, CiInv, CiCar, ClInv, ClCar, PeInv, PeCar, SuInv, SuCar;

    private GoapPlanner gp;
    private Stack<Node> plan;
    private Stack<Node> planCopy;

    public Camera cam;
    public NavMeshAgent player;
    private NavMeshAgent thief;
    public GameObject th;

    Node currentAction;
    public List<Vector3> pos;

    public GameObject p1, p2, p3, p4, p5, p6, p7, p8;

    [HideInInspector] public bool stolenFrom = false;
    private bool performingAction = false;
    //private RaycastHit hit;
    TextMesh tm;
    public GameObject Uicontent;
    public GameObject example;
    private GameObject temp, temp2;

    public bool gameEnded = false;

    private GameObject[] textInventory;
    private GameObject[] textCaravan;

    void updateStupidGrid(Node n){
        for(int i=0; i<textInventory.Length; i++){
            textInventory[i].GetComponent<UnityEngine.UI.Text>().text = ""+n.inventory[i];
            textCaravan[i].GetComponent<UnityEngine.UI.Text>().text = ""+n.caravan[i];
        }
        
    }

    void buildScroll()
    {
        Stack<Node> planCopy = new Stack<Node>(plan.Reverse());
        //planCopy = plan.Clone();
        float posY = example.transform.localPosition.y;

        example.SetActive(true);
        Node n;
        int count = 0;
        //Vector3 tempPos;
        temp = Instantiate(example, example.transform.position, Quaternion.identity);
        temp.transform.SetParent(Uicontent.transform);//temp.transform.parent = Uicontent.transform;
        temp.transform.localScale = new Vector3(1, 1, 1);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(temp.GetComponent<RectTransform>().anchoredPosition.x,
                                                            temp.GetComponent<RectTransform>().anchoredPosition.y - count);
        temp.tag = "row";
        count += 2;
        while (planCopy.Count > 0)
        {

            n = planCopy.Pop();

            temp = Instantiate(example, example.transform.position, Quaternion.identity);
            temp.transform.SetParent(Uicontent.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(temp.GetComponent<RectTransform>().anchoredPosition.x,
                                                                temp.GetComponent<RectTransform>().anchoredPosition.y - count);
            //temp.tag = "Action";
            if (n.actionIndex == 9)
            {
                temp.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = gp.actions[n.actionIndex].actionName2(n);
                // temp.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>.text = gp.actions[n.actionIndex].actionName2(n);
            }
            else
            {
                temp.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = gp.actions[n.actionIndex].actionName();
            }
            temp.tag = "row";
            count += 2;
        }
        temp = Instantiate(example, example.transform.position, Quaternion.identity);
        temp.transform.SetParent(Uicontent.transform);
        temp.transform.localScale = new Vector3(1, 1, 1);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(temp.GetComponent<RectTransform>().anchoredPosition.x,
                                                            temp.GetComponent<RectTransform>().anchoredPosition.y - count);
        temp.tag = "row";
        example.SetActive(false);//WILL HAVE TO ENABLE LATER TO REBUILD THIS DUMB SCROLLABLE
    }

    void Start()
    {
        // plan = gameObject.GetComponent<GoapPlanner>().plan; //i do this in goapplanner
       // UIcontent = GameObject.Find("Content");
        gp = GetComponent<GoapPlanner>();
        Node start = new Node(GameM.caravanState, GameM.inventoryState, -1, null);
        plan = gp.buildPlan(start);
      //  planCopy = new Stack<Node>();
      thief = th.GetComponent<NavMeshAgent>();
      textInventory = new GameObject[]{TuInv, SaInv, CaInv, CiInv, ClInv, PeInv, SuInv};
    textCaravan = new GameObject[]{TuCar, SaCar, CaCar, CiCar, ClCar, PeCar, SuCar};

        buildScroll();
        
        
        
        // plan = gp.plan();
        // velocity = player.velocity;
        // int count = 0;
       

       // if (p1== null) Debug.Log("hi");

        List<GameObject> ptexts = new List<GameObject> { p1, p2, p3, p4, p5, p6, p7, p8 };
        //if(ptexts.Count == 0) Debug.Log("hi");
        List<Vector3> posStatic = new List<Vector3> { GameM.pos1, GameM.pos2, GameM.pos3, GameM.pos4, GameM.pos5, GameM.pos6, GameM.pos7, GameM.pos8 };
        pos = new List<Vector3> { GameM.pos1, GameM.pos2, GameM.pos3, GameM.pos4, GameM.pos5, GameM.pos6, GameM.pos7, GameM.pos8 };
        Shuffle(pos);
        pos.Add(GameM.caravanPos);
        pos.Add(GameM.caravanPos);

       // int count = 0;
        for(int i = 0; i<ptexts.Count; i++)
        {
            tm = ptexts[i].GetComponent<TextMesh>();
            tm.text = "T" + (pos.IndexOf(posStatic[i])+1);
            //count++;
        }
      
    }

    // private static Random rng = new Random();

    public void Shuffle<Vector3>(List<Vector3> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            //int k = rng.Next(n + 1);
            Vector3 value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private NavMeshPath lastAgentPath, lastAgentPathThief;
    private Vector3 lastAgentVelocity, lastAgentVelocityThief;

     public void PauseSim() {
         lastAgentVelocity = player.velocity;
         lastAgentVelocityThief = thief.velocity;
         lastAgentPath = player.path;
         lastAgentPathThief = thief.path;
         player.velocity = Vector3.zero;
        thief.velocity = Vector3.zero;
        player.ResetPath();
        thief.ResetPath();
     }
     
     public void resume() {
         player.velocity = lastAgentVelocity;
         thief.velocity = lastAgentVelocity;
         player.SetPath(lastAgentPath);
         thief.SetPath(lastAgentPathThief);
     }
    IEnumerator waitPause()
    {
        yield return new WaitForSeconds(2);
        pause = false;
       // resume();
    }

    public int[] clone(int[] arr)
    {
        int[] cloneArr = new int[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            cloneArr[i] = arr[i];
        }
        return cloneArr;
    }

    bool wait = false;
    // Update is called once per frame
    void Update()
    {

        

        if (wait && !gameEnded)
        {
            StartCoroutine(waitPause());
        }
        if (stolenFrom && !gameEnded)
        {
            // pause = true;
            //change plan
            // Debug.Log("hi");

           // currentAction.inventory = GameM.inventoryState;
            //currentAction.caravan = GameM.caravanState;
            //updateStupidGrid(currentAction);
            Node start = new Node(clone(GameM.caravanState), clone(GameM.inventoryState), -1, null);
            updateStupidGrid(start);
            plan = gp.buildPlan(start);
            // Destroy
            GameObject[] rows = GameObject.FindGameObjectsWithTag("row");
            foreach (GameObject row in rows)
            {
                Destroy(row);
            }
            buildScroll();
            performingAction = false;
            Debug.Log("CHANGE OF PLANS");
            // plan.Pop(); //pop the start node as it is useless
            stolenFrom = false;
            wait = true;
            if (pause)
            {
                //pause = false;
                //wait = true;
                 //resume();
            }

        }

        

        else if (!pause && !gameEnded)
        {
           
            if (!performingAction && !stolenFrom && !gameEnded)
            {
                performingAction = true;
                if (plan.Count == 0) //game has ended
                {

                    currentAction = null;
                    gameEnded = true;
                    return;
                }
                if (plan.Count > 0) currentAction = plan.Pop();
               
                // Debug.Log(currentAction.ToString());

                Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(pos[currentAction.actionIndex]));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && currentAction != null)
                {
                    player.SetDestination(hit.point);
                    //velocity = player.velocity;
                    //StartCoroutine(performAction()); 
                }


            }
            if (!player.pathPending && performingAction && !stolenFrom && !gameEnded)
            {
                if (player.remainingDistance <= player.stoppingDistance)
                {
                    if (!player.hasPath || player.velocity.sqrMagnitude == 0f)
                    {
                        // Done
                       // Debug.Log("remaining distance: " + player.remainingDistance);
                       if(currentAction != null || currentAction.actionIndex != null)
                        {
                            gp.actions[currentAction.actionIndex].updateGameState(currentAction);
                            performingAction = false;
                            updateStupidGrid(currentAction);
                            Destroy(GameObject.Find("Content").transform.GetChild(2).gameObject);
                        }
                        
                        //Debug.Log(currentAction.ToString());
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                Time.timeScale *= 2;

                //velocity *= 2;
                //player.speed *= 2;
            }
            if (Input.GetKeyDown("-"))
            {
                Time.timeScale *= 0.5f;
                //player.velocity *= 0.5f;
            }
        }

        
        if (Input.GetKeyDown("space") && !gameEnded)
        {
            pause = !pause;
            if(pause == false){
                resume();
            }else PauseSim();
            
            //player.velocity *= 0.5f;
        }
        
    }

          

        //on collision with thief //UPDATE PLAN

        //USE ienumerator to go from idle, movestate, doanimation

        IEnumerator performAction()
        {
            //move player to said place //USE ienumerator i think?

            while ((player.destination.x - transform.position.x + player.destination.z - transform.position.z) >= 0.0001f)
            {
                performingAction = true;
                //Debug.Log("remaining distance: " + player.remainingDistance);
               // Debug.Log("hit point: " + hit.point);
                //carry out action
                if (stolenFrom) yield break;
                yield return null;
            }
            
            if (stolenFrom) yield break;
            gp.actions[currentAction.actionIndex].updateGameState(currentAction);
            performingAction = false;
            Debug.Log(currentAction.ToString());



            //change gameState (updategamestate function)

        }
    }

