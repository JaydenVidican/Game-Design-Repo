using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact
}

public class MovementController : MonoBehaviour
{
    public PlayerState currentState;
    public float speed = 5; //player speed
    private Rigidbody2D myRigidbody; //creates rigidbody object
    private Vector3 change; //creates vector object
    private Animator animator; //creates animator object
    private bool canSprint = true; //tells if user can sprint (if stamina added later on)


    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>(); //accesses animations
        myRigidbody = GetComponent<Rigidbody2D>(); //accesses rigig body component of player

    }

    void Update() //checks every frame if player is holding spring key
    {
        if (Input.GetKey("left shift") && canSprint == true) 
            speed = 10;
        else
            speed = 5;
    }

    void FixedUpdate()
    {
        
        change = Vector3.zero; 
        change.x = Input.GetAxisRaw("Horizontal"); //gets horizontal movement
        change.y = Input.GetAxisRaw("Vertical"); //gets vertical movement
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.5f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove() //moves character and updates the animation
    {
        if(change != Vector3.zero) // checks that player speed is not zero (is moving)
        {
            MoveCharacter();

            //sets value for animator to display correct animation
            animator.SetFloat("moveX", change.x); 
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true); //transiton
        }
        else 
        {
            animator.SetBool("moving", false); //transiton
        }
    }


    void MoveCharacter() //calculates player movement
    {
        myRigidbody.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
        );
    }
}