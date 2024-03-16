using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 5;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator; //new
    private bool canSprint = true;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); //new
        myRigidbody = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (Input.GetKey("left shift") && canSprint == true)
            speed = 10;
        else
            speed = 5;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove() //new
    {
        if(change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true); //new
        }
        else //new
        {
            animator.SetBool("moving", false); //new
        }
    }


    void MoveCharacter()
    {
        myRigidbody.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
        );
    }

/*
     void OnCollisionEnter2D(Collision2D col)
    {
        canSprint = false;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        canSprint = true;
    }
    */
}