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
    private float speed = 2.5f;
    //private float maxHealth = 100;
    //private float currentHealth = 0;
    //display Display;
    private GameObject[] healthObjs;
    List<PrometeoCarController> lis;

    /*private void Start()
    {
        Display = GetComponent<display>();
        maxHealth = currentHealth = Display.maxHealth;
    }*/

    public enum AIstate
    {
        idle,
        persuing,
        attacking
    };
    public AIstate aiState = AIstate.persuing;

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentHealth <= maxHealth)
                currentHealth -= (collision.relativeVelocity.magnitude * 0.5f);
            FindTarget();
        }
    }*/

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
        /*while(true)
        {*/
        switch (aiState)
        {
            case AIstate.idle:
                //Vector3 direction = (target.position - transform.position).normalized;
                //float angle = Vector3.Angle(transform.forward, direction);
                float dis = Vector3.Distance(targetChase.position, transform.position);
                if (dis > 15)
                {
                    //  transform.LookAt(target);
                    aiState = AIstate.persuing;
                }
                if (dis < AttackingDistance + 2)
                {
                    aiState = AIstate.attacking;
                }
                //navigator.SetDestination(transform.position);
                break;

            case AIstate.persuing:
                // direction = (target.position - transform.position).normalized;
                //angle = Vector3.Angle(transform.forward, direction);
                dis = Vector3.Distance(targetChase.position, transform.position);
                //   transform.LookAt(target);
                // controller.GoForward();
                if (dis < 4)
                {
                    aiState = AIstate.idle;
                }
                //navigator.SetDestination(targetChase.position);
                break;

            case AIstate.attacking:
                //navigator.SetDestination(transform.position);
                //    transform.LookAt(target);
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
      //  Display.slider.value = currentHealth;
        //d.SetHealth(currentHealth);
        //	navigator.transform.localPosition = new Vector3(0, carController.frontLeftCollider.transform.localPosition.y, carController.frontLeftCollider.transform.localPosition.z);
        //	
        //	navigator.SetDestination(targetChase.position);
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
        Navigation();       // Feeds steerInput based on navigator.
        // FixedRaycasts();        // Affects steerInput if one of raycasts detects an object front of our AI car.
        //FeedRCC();      // Feeds motorInput.
        //Resetting();        // Was used for deciding go back or not after crashing.
        Movement();
       

        

    }

    void Movement()
    {
        if (aiState == AIstate.attacking)
        {
            speed = 4f;
            carController.accelerationMultiplier = 1;
            StartCoroutine(Attack());
        }
        if (aiState == AIstate.persuing)
        {
            speed = 17f;
            carController.accelerationMultiplier = 4;
            carController.GoForward();
        }
        if (aiState == AIstate.idle)
        {
            carController.Brakes();
        }
    }
  
    
    void Navigation()
    {
        Vector3 relativePos = targetChase.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
      
        Quaternion current = transform.localRotation;
        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * speed);

        Debug.Log("rotation = " + rotation.z);
        //transform.LookAt(targetChase, Vector3.forward);
        //  navigator.SetDestination(targetChase.position);
        // If our scene doesn't have a Waypoint Container, return with error.
        //if(_AIType == AIType.ChasePlayer && !targetChase){
        //	Debug.LogError("Target Chase Couldn't Found!");
        //	enabled = false;
        //	return;
        //	}

        // Navigator Input is multiplied by 1.5f for fast reactions.
        float navigatorInput = Mathf.Clamp(transform.InverseTransformDirection(navigator.desiredVelocity).x, -1f, 1f);
        //Debug.Log("inp: " + navigatorInput);

        // Setting destination of the Navigator. 

        if (navigator.isOnNavMesh)
            navigator.SetDestination(targetChase.position);

        //Steer Input.
        if (carController.direction == 1)
        {
            if (true)
                carController.steeringAxis = Mathf.Clamp((navigatorInput + rayInput), -1f, 1f);
            /*else
                carController.steeringAxis = Mathf.Clamp(rayInput, -1f, 1f);*/

            //Debug.Log("steering: " + carController.steeringAxis);
            //carController.TurnRight();
        }
        else
        {
            carController.steeringAxis = Mathf.Clamp((-navigatorInput - rayInput), -1f, 1f);
            //carController.TurnLeft();
        }
        /*
                // Brake Input.
                if (!inBrakeZone)
                {
                    if (carController.speed >= 25)
                    {
                        brakeInput = Mathf.Lerp(0f, .85f, (Mathf.Abs(steerInput)));
                    }
                    else
                    {
                        brakeInput = 0f;
                    }
                }
                else
                {
                    brakeInput = Mathf.Lerp(0f, 1f, (carController.speed - maximumSpeedInBrakeZone) / maximumSpeedInBrakeZone);
                }
        */
        // Gas Input.
        //if(!inBrakeZone){

        if (carController.carSpeed >= 10)
        {
            if (true)
                carController.throttleAxis = Mathf.Clamp((1f - (Mathf.Abs(navigatorInput / 10f) - Mathf.Abs(rayInput / 10f))), 0.75f, 1f);
            /*else
                carController.throttleAxis = 0f;*/
        }
        else
        {
            if (true)
                carController.throttleAxis = 1f;
            /*else
                carController.throttleAxis = 0f;  */
        }
        carController.GoForward();

        /*		}else{

                    if(!carController.changingGear)
                        gasInput = Mathf.Lerp(1f, 0f, (carController.speed) / maximumSpeedInBrakeZone);
                    else
                        gasInput = 0f;
        ​
            }
        */

    }

    void FeedRCC()
    {
        // Feeding gasInput of the RCC.
        if (carController.direction == 1)
        {
            /* if (!limitSpeed)
             {
                 carController.throttleAxis = gasInput;
             }
             else
             {*/
            carController.throttleAxis = carController.throttleAxis * Mathf.Clamp01(Mathf.Lerp(10f, 0f, (carController.carSpeed) / 90));
            //}
        }
        else
        {
            carController.throttleAxis = 0f;
        }
        // Feeding steerInput of the RCC.
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