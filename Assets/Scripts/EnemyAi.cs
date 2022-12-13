using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public CharacterController2D controller;
    public float visionRadius = 7;
    public float attackRadius = 4;
    public float runSpeed = 8;
    public float currentHealth = 10;
    public float maxHealth = 10;
    public bool hasRangedAttack = false;

    private int? playerDirection = null;
    private bool canAttack = true;
    Animator animator;
    [SerializeField] private LayerMask m_WhatIsCharacter;                          // A mask determining what is character

    IEnumerator StartRangedAttackCooldown(float cooldown)
    {
        canAttack = false;
        controller.RangedAttack();

        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

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
            if (runSpeed == 0)
            {
                controller.FaceDirection((int)playerDirection);
            }
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
            if (canAttack && Mathf.Abs(transform.position.x - playerPosition.position.x) < attackRadius)
            {
                animator.SetTrigger("Attack");
                if (hasRangedAttack)
                {
                    StartCoroutine(StartRangedAttackCooldown(1));
                }
            }
        } else
        {
            playerDirection = null;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("damage");
        Debug.Log(col.gameObject.name);
        Damage damage = col.GetComponent<Damage>();
        //if (damage && gameObject.layer == damage.target)
        if (damage && damage.MatchesDamageLayer(gameObject.layer))
        {
            TakeDamage(damage.damage);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 1)
        {
            Destroy(gameObject);
        }
    }
}
