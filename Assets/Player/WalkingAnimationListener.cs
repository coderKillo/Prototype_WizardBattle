using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingAnimationListener : MonoBehaviour
{
    private Animator animator;
    private Rigidbody body;
    private FirstPersonController controller;

    private int walking = Animator.StringToHash("walking");

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody>();
        controller = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (controller.IsGrounded && body.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool(walking, true);
        }
        else
        {
            animator.SetBool(walking, false);
        }
    }
}
