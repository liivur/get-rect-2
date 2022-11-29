using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public CharacterController2D controller;
    public float visionRadius = 7;
    public float attackRadius = 4;
    public float runSpeed = 8;

    private int? playerDirection = null;
    Animator animator;
    [SerializeField] private LayerMask m_WhatIsCharacter;                          // A mask determining what is character

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (playerDirection != null)
        {
            controller.Move((float) playerDirection * runSpeed * Time.deltaTime, false, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform? playerPosition = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, visionRadius, m_WhatIsCharacter);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                playerPosition = colliders[i].transform;
            }
        }

        if (playerPosition)
        {
            if (transform.position.x > playerPosition.position.x)
            {
                playerDirection = -1;
            } else
            {
                playerDirection = 1;
            }
            if (Mathf.Abs(transform.position.x - playerPosition.position.x) < attackRadius)
            {
                animator.SetTrigger("Attack");
            }
        } else
        {
            playerDirection = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        print("enemy");
        print(col);
    }
}
