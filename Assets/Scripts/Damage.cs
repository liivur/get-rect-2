using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 1;
    [SerializeField] private LayerMask target;                          // A mask determining what is character

    //void Awake()
    //{
    //    BoxCollider2D bc;
    //    bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
    //    bc.size = new Vector2(1.3f, 1.3f);
    //    bc.isTrigger = true;
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
