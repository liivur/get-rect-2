using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10;
    public LayerMask ignore;

    IEnumerator RemoveLostBullets(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(RemoveLostBullets(2));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (ignore == (ignore | (1 << collider.gameObject.layer)))
        {
            return;
        }
        Destroy(gameObject);
    }
}
