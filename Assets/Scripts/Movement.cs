using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool attack = false;
    bool rangedAttack = false;
    bool jump = false;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.deltaTime, false, jump);
        jump = false;

        if (attack)
        {
            
            attack = false;
        }

        if (rangedAttack)
        {
            rangedAttack = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            attack = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("RangedAttack");
            controller.RangedAttack(true);
            rangedAttack = true;
        }
    }
}
