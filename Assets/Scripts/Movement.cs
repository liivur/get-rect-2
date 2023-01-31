using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;
    public float attackCooldown = 1;
    public float rangedAttackCooldown = 1;

    float horizontalMove = 0f;
    bool attack = false;
    bool rangedAttack = false;
    bool jump = false;
    Animator animator;
    bool canAttack = true;
    bool canRangedAttack = true;

    IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    IEnumerator StartRangedAttackCooldown()
    {
        canRangedAttack = false;
        yield return new WaitForSeconds(rangedAttackCooldown);

        canRangedAttack = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("Walking", horizontalMove != 0 && controller.IsGrounded());
        controller.Move(horizontalMove * Time.deltaTime, false, jump, animator);
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

        if (canAttack && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(StartAttackCooldown());
            animator.SetTrigger("Attack");
            attack = true;
        }

        if (canRangedAttack && Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(StartRangedAttackCooldown());
            animator.SetTrigger("RangedAttack");
            controller.RangedAttack(true);
            rangedAttack = true;
        }
    }
}
