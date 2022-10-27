using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI_Prometeo_Car_Controller : MonoBehaviour
{
    public float AttackingDistance = 2f;
    private PrometeoCarController carController;
    private rotat rotate;
    private Rigidbody rigid;
    // Rigidbody of this vehicle.
    private Transform targetChase;
    // Unity's Navigator.
    private UnityEngine.AI.NavMeshAgent navigator;
    private GameObject navigatorObject;
    private Transform enemy;

    private float rayInput = 0f;
    //private float throttleAxis = 0f;
    private float speed = 25f;
    //private float maxHealth = 100;
    //private float currentHealth = 0;
    //display Display;

    List<PrometeoCarController> lis;

  
    public enum AIstate
    {
        idle,
        persuing,
        attacking
    };
    public AIstate aiState = AIstate.persuing;

    public void FindTarget()
    {
        PrometeoCarController[] allObjects = UnityEngine.Object.FindObjectsOfType<PrometeoCarController>();
        lis = new List<PrometeoCarController>(allObjects);
        foreach (PrometeoCarController go in lis)
        {
            if (this.name.Equals(go.name))
            {
                lis.Remove(go);
                break;
            }
        }

        System.Random randam = new System.Random();
        int Follow = randam.Next(lis.Count);

        targetChase = lis[Follow].transform;
        if (GameObject.FindObjectOfType<CameraFollow>())
        {

            GameObject.FindObjectOfType<CameraFollow>().SetPlayerBotTransform(targetChase);
        }

    }

    public void think()
    {
        switch (aiState)
        {
            case AIstate.idle:
                float dis = Vector3.Distance(targetChase.position, transform.position);
                if (dis > 15)
                {
                    aiState = AIstate.persuing;
                }
                if (dis < AttackingDistance + 2)
                {
                    aiState = AIstate.attacking;
                }
                break;

            case AIstate.persuing:
                dis = Vector3.Distance(targetChase.position, transform.position);
               
                if (dis < 4)
                {
                    aiState = AIstate.idle;
                }
             
                break;

            case AIstate.attacking:
              
                dis = Vector3.Distance(targetChase.position, transform.position);
                if (dis > 15)
                {
                    aiState = AIstate.persuing;
                }
                if ((dis < AttackingDistance) && (dis > 2))
                {
                    aiState = AIstate.attacking;
                }
                if (dis < 1.5f)
                {
                    carController.Brakes();
                }
                break;

            default:
                break;
        }

    }
    void Awake()
    {
        FindTarget();
        /*for (int i = 0; i < lis.Count; i++)
        {
            GameObject duplicate = Instantiate(GameObject.FindWithTag("Health"));
            //duplicate.transform.position = Vector3.zero;
        }*/

        carController = GetComponent<PrometeoCarController>();
        rotate = GetComponentInChildren<rotat>();
        carController.AIController = true;
        rigid = GetComponent<Rigidbody>();

        navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.parent = transform;
        navigatorObject.transform.localPosition = Vector3.zero;
        navigatorObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
        navigator = navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        navigator.radius = 1;
        navigator.speed = 1;
        navigator.angularSpeed = 1000f;
        navigator.height = 1;
        navigator.avoidancePriority = 50;
    }
    void Update()
    {
        think();
    }



    IEnumerator Attack()
    { 
        rotate.bl = true;
        yield return new WaitForSeconds(1);
        rotate.bl = false;
    }

    void FixedUpdate()
    {
        Navigation();
        Movement();
       

        

    }

    void Movement()
    {
        if (aiState == AIstate.attacking)
        {
            speed = 4f;
            StartCoroutine(Attack());
        }
        if (aiState == AIstate.persuing)
        {
            speed = 25f;
        }
        if (aiState == AIstate.idle)
        {
            carController.Brakes();
        }
    }

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        // Project A and B onto the plane orthogonal target axis
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);

        // Find (positive) angle between A and B
        float angle = Vector3.Angle(dirA, dirB);

        // Return angle multiplied with 1 or -1
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }
    void Navigation()
    {
            Vector3 relativePos = (targetChase.position - transform.position).normalized;
        //when navmesh is apply      
        if (navigator.isOnNavMesh)
                   navigator.SetDestination(targetChase.position);
             carController.steeringAxis = Mathf.Clamp(AngleAroundAxis(transform.forward, relativePos, Vector3.up), -1f, 1f);
           
            if (carController.steeringAxis > 0)
            {
                carController.TurnRight();
            }
            else if (carController.steeringAxis < 0)
            {
                carController.TurnLeft();
            }            
            else
            {
                carController.steeringAxis = 0;
            
            }
    }

    void FeedRCC()
    {
        // Feeding gasInput of the RCC.
        if (carController.direction == 1)
        {
            carController.throttleAxis = carController.throttleAxis * Mathf.Clamp01(Mathf.Lerp(10f, 0f, (carController.carSpeed) / 90));

        }
        else
        {
            carController.throttleAxis = 0f;
        }

        if (true)
            carController.steeringAxis = Mathf.Lerp(carController.steeringAxis, 0, Time.deltaTime * 20f);
        /* else
             carController.steeringAxis = steeringAxis;*/
        // Feeding brakeInput of the RCC.
        /*if (carController.direction == 1)
            carController.brakeInput = brakeInput;
        else
            carController.brakeInput = gasInput;
​
    }*/
    }
}