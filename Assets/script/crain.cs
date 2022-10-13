using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crain : MonoBehaviour
{
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
            if (Input.GetKeyDown(KeyCode.U))
            {
                animator.SetTrigger("up");
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                animator.SetTrigger("down");
            }
        }
    }
}
