using System;
using UnityEngine;

public class PickUpOBJ : MonoBehaviour
{
    private Rigidbody rb; 
    private Transform grabPointTransform; 
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(Transform grabTransform)
    {
        this.grabPointTransform= grabTransform;
    }

    private void FixedUpdate()
    {
        if (grabPointTransform != null)
        {
            rb.MovePosition(grabPointTransform.position);
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
