using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotat : MonoBehaviour
{
    public bool bl=false;
    public string botType;
    public float rotateSpeed = 500f;
    private GameObject throttleButton;

   PrometeoTouchInput throttlePTI;
    private Canvas canvas;
    private float maxHealth = 100;
    private float currentHealth = 0;
    display Display;


    // Start is called before the first frame update
  /*  void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("Axe"))
        {
            Debug.Log("ai attack: " + collision.relativeVelocity.magnitude);
            *//*if (currentHealth <= maxHealth)
                currentHealth -= 10;*//*
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            rotat pcc = collision.gameObject.GetComponentInChildren<rotat>();
            //Debug.Log("hit");
            *//*if (pcc.currentHealth <= pcc.maxHealth)
                pcc.currentHealth -= (collision.relativeVelocity.magnitude * 0.5f);*//*
            Debug.Log("attacker name: " + collision.gameObject.name);
            Debug.Log("currH: " + pcc.currentHealth);
            Debug.Log("maxH: " + pcc.maxHealth);

        }
        Debug.Log("collider working outside.");
    }
  */  private void Start()
    {
    /*    Display = GetComponentInParent<display>();
        maxHealth = currentHealth = Display.maxHealth;
*/
    }

    void Awake()
    {
        //Debug.Log("plc");

        throttleButton = GameObject.Find("Canvas").transform.GetChild(3).gameObject;


           throttlePTI = throttleButton.GetComponent<PrometeoTouchInput>();
        
      
    }

    public void SetPlayerBot(GameObject btn)
    {
        throttleButton = btn;
 
    }


    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInParent<PrometeoCarController>().AIController)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                bl = true;

                //            transform.Rotate(0, 0, 30);
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                //Debug.LogError("error ");
                bl = false;
                return;
            }
        }
        if(bl)
        {
            if (GetComponentInParent<PrometeoCarController>().botType.Equals("tombstone"))
            {
                transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
                StartCoroutine(wait());
            }
            else
            {
                transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
            }
        }
      /*  if (throttlePTI.buttonPressed)
        {
            transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);

        }
*/

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.left * rotateSpeed * Time.deltaTime);
    }
}
