using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ax1 : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    void Update()
    {

        if (animator != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                animator.SetTrigger("leftOn");
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                animator.SetTrigger("leftOff");
            }
        }
    }
}
