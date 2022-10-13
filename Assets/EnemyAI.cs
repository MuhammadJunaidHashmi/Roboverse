using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
 
    public Transform target;
    NavMeshAgent agent;
    public float health= 15;
    PrometeoCarController controller;
    public enum AIstate{
        idle,
        persuing,
        attacking
    };
    public AIstate aiState = AIstate.persuing;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<PrometeoCarController>();
       // StartCoroutine(think());
    }
    public void Update()
    {
        think();
    }
    public void TakeDamage(int damage)
    {
        health -=damage;
        if (health <= 0)
            Invoke(nameof(DestroyEnemy), 0.5f);   
    }
    public void DestroyEnemy()
    {
        Destroy(gameObject);
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
                    float dis = Vector3.Distance(target.position, transform.position);
                    if(dis<10)
                    {
                      //  transform.LookAt(target);

                        aiState = AIstate.persuing;
                    }

                    agent.SetDestination(transform.position);
                    break;
                case AIstate.persuing:
                   // direction = (target.position - transform.position).normalized;
                    //angle = Vector3.Angle(transform.forward, direction);
                    dis = Vector3.Distance(target.position, transform.position);
                 //   transform.LookAt(target);
             
                  // controller.GoForward();
                    if (dis > 10)
                    {
                        aiState = AIstate.idle;
                    }
                    if (dis < 2)
                    {
                        aiState = AIstate.attacking;
                    }
                    agent.SetDestination(target.position);
                    break;
                case AIstate.attacking:
                    agent.SetDestination(transform.position);
                //    transform.LookAt(target);
                    dis = Vector3.Distance(target.position, transform.position);
                    if (dis > 2)
                    {
                        aiState = AIstate.persuing;
                    }

               
                    break;
                default:
                    break;
            }
           
     /*       yield return new WaitForSeconds(1f);*/

      //  }
    }
}
