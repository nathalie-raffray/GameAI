using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThiefManager : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;

    public int waitBeforeNextSteal;

    private bool wander = true;
    private bool goToCaravan = false;
    private bool interceptPlayer = false;
    private float timer = 0;
    private float timeSinceLastSteal = 0;
   // private Vector3 origin = new Vector3(0, 0, 0);

    public Camera cam;
    public NavMeshAgent thief;
    public GameObject player;

    public PlayerManager pm;
    
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Example());
        timer = wanderTimer;
        pm = player.GetComponent<PlayerManager>();

        transform.position = RandomNavSphere(transform.position, 10, -1);
        Debug.Log(transform.position);
    }

    

    void MaybeSteal()
    {
        wander = true;
        //interceptPlayer = false;

        if (timeSinceLastSteal > waitBeforeNextSteal)
        {
          //  Debug.Log("hi");
            int rand = Random.Range(0, 300);
            if (rand <= 100)
            {
                int rand2 = Random.Range(0, 2);
                if (rand2 == 1)
                {
                   // Debug.Log("go to caravan");
                    Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(GameM.caravanPos));
                    RaycastHit hit;
                    

                    if (Physics.Raycast(ray, out hit)) thief.SetDestination(hit.point);
                    goToCaravan = true; //StartCoroutine(GoToCaravan());
                   
                    wander = false;
                }
                else
                {
                    //Debug.Log("intercept player");
                    interceptPlayer = true;//StartCoroutine(InterceptPlayer());
                    wander = false;
                }
            }
            timeSinceLastSteal = 0;
        }
        
    }

    void FixedUpdate()
    {
        timeSinceLastSteal += Time.deltaTime;
        if (wander && !PlayerManager.pause && !pm.gameEnded)
        {

            Wander();
            //StartCoroutine(WaitingToSteal());
            MaybeSteal();
            
        }
        else if (goToCaravan && !PlayerManager.pause && !pm.gameEnded)
        {
           // Debug.Log("hhhhhh");
            GoToCaravan();
        }else if (interceptPlayer && !PlayerManager.pause && !pm.gameEnded)
        {
            InterceptPlayer();
        }

        if (pm.gameEnded)
        {
            thief.velocity = new Vector3(0, 0,0);
        }

    }

    void Wander()
    {
        timer += Time.deltaTime;
      ///  Debug.Log("Trywandering");

        if (timer >= wanderTimer)
        {
            //Debug.Log("wandering");
            Vector3 p = RandomNavSphere(transform.position, wanderRadius, -1);
            thief.SetDestination(p);
           // if(!thief.SetDestination(p)) Debug.Log("shouldnt move");
            timer = 0;
        }        
    }

    public Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask) //will return a random point within the navmesh
    {
        float x = Random.Range(-5, 8);
        float z = Random.Range(-9, 8);
        while (x>-2.2f && x < 4.5f && z>-3.6f && z < 4.25f)
        {
            x = Random.Range(-5, 8);
            z = Random.Range(-9, 8);
        }
        
            
        return new Vector3(x, 0, z);

    }

    void GoToCaravan()
    {

        //once at caravan
        //Debug.Log("I MUST GO TO CARAVAN EE");
        if (!thief.pathPending)
        {
           // Debug.Log("caravan in here? 1");
            if (thief.remainingDistance <= thief.stoppingDistance+0.7f) //i should use collider for this
            {

                Steal(GameM.caravanState);
               // Debug.Log("CARAVANdone");

                Vector3 p = RandomNavSphere(transform.position, wanderRadius, -1);
                thief.SetDestination(p);

                goToCaravan = false;
                wander = true;
                timer = wanderTimer;
                timeSinceLastSteal = 0;

               // Debug.Log("caravan in here? 2");
                if (!thief.hasPath || thief.velocity.sqrMagnitude == 0f)
                {
                    // Done
                   // pm.PauseSim();
                   // PlayerManager.pause = true;
                   

                }
            }
        }

    


    

    }

    

    void Steal(int[] arr)
    {
        int count = 0;
        int rand1 = 0;
        int rand2 = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)
            {
                count += arr[i];
            }
        }
        rand1 = Random.Range(0, count - 1);
        rand2 = Random.Range(0, count - 1);
        int count2 = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != 0)
            {
                for(int j = 0; j<arr[i]; j++)
                {
                    
                    if (count2 == rand1)
                    {
                        arr[i] = arr[i]-1;
                    }
                    if (count2 == rand2 && count > 1 && arr[i] != 0)
                    {
                       // arr[i] = arr[i] - 1;
                    }
                    count2 += 1;
                }
               
               
            }
        }
        if (count > 0)
        {
            for(int i = 0; i<arr.Length; i++)
            {
               // Debug.Log(arr[i] + ",");
            }

            //Debug.Log(arr.ToString());
            pm.stolenFrom = true;
        }
        else
        {
            if (interceptPlayer && PlayerManager.pause)
            {
                 pm.resume(); //BUT IF THIS IS COMMENTED OUT THEN IT PAUSE SIMULATION FOR NO REASON
                PlayerManager.pause = false; //IF IT IS NOT, THEN THIEF JUST BUGS AT TOP LEFT CORNER 
            }
        }

    }

    void InterceptPlayer()
    {

       // Debug.Log("I MUST INTERCEPT PLAYER EE");
        Ray ray = cam.ScreenPointToRay(cam.WorldToScreenPoint(player.transform.position)); //trouble here if they have different y coordinates (maybe make them the same)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))  thief.SetDestination(hit.point);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (interceptPlayer && collision.gameObject.name == "Player")
        {
            interceptPlayer = false;
            pm.PauseSim();
            PlayerManager.pause = true;
            Steal(GameM.inventoryState);
            
            wander = true;
            timer = wanderTimer;

            Vector3 p = RandomNavSphere(transform.position, wanderRadius, -1);
            thief.SetDestination(p);
            interceptPlayer = false;
            timeSinceLastSteal = 0;
            //pm.resume();
            //PlayerManager.pause = false;
        }
        
    }
}
