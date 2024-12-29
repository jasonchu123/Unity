using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frog : MonoBehaviour
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    private Quaternion fixedRotation;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    private Rigidbody2D rb;


    private bool facingLeft = true;

    private void Start()
    {
        fixedRotation = transform.rotation;
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        transform.rotation = fixedRotation;
        if(facingLeft)
        {
            if(transform.position.x > leftCap)
            {
                if(transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1,1);
                }
                if(coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if(transform.position.x < rightCap)
            {
                if(transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1,1);
                }
                if(coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                }
            }
            else
            {
                facingLeft = true;
            }

        }
        
    }
}