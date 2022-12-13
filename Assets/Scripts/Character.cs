using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float currentHealth = 100;
    public float maxHealth = 100;
    public float invincibilityCooldown = 1;

    bool isInvincible = false;
    IEnumerator StartInvincibilityCooldown()
    {
        isInvincible = true;
        SetColor(Color.blue);
        yield return new WaitForSeconds(invincibilityCooldown);

        isInvincible = false;
        SetColor(Color.white);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damage damage = col.GetComponent<Damage>();
        
        //if (damage && damage.target == (damage.target | (1 << gameObject.layer)))
        if (damage && damage.MatchesDamageLayer(gameObject.layer))
        {
            TakeDamage(damage.damage);
        }
    }

    void SetColor(Color c)
    {
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.material.color = c;
        }
    }

    void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }
        currentHealth -= damage;

        StartCoroutine(StartInvincibilityCooldown());
    }
}
