using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    public GameObject target;
    public CharacterController2D controller;
    public bool checkFacing = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target = FindObjectOfType<Character>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!checkFacing || controller.IsFacingRight())
        {
            transform.right = target.transform.position - transform.position;
        } else
        {
            transform.right = (target.transform.position - transform.position) * -1;
        }
    }
}
