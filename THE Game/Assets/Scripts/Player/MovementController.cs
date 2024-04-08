using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class MovementController : MonoBehaviour
{
    public PlayerState currentState;
    public float speed = 5; //player speed
    private Rigidbody2D myRigidbody; //creates rigidbody object
    private Vector3 change; //creates vector object
    private Animator animator; //creates animator object
    private bool canSprint = true; //tells if user can sprint (if stamina added later on)
    public FloatValue currentHealth;
    public GameSignal playerHealthSignal;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;


    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>(); //accesses animations
        myRigidbody = GetComponent<Rigidbody2D>(); //accesses rigig body component of player
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
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
        if (currentState == PlayerState.interact)
        {
            return;
        }
        change = Vector3.zero; 
        change.x = Input.GetAxisRaw("Horizontal"); //gets horizontal movement
        change.y = Input.GetAxisRaw("Vertical"); //gets vertical movement
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
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
        yield return new WaitForSeconds(.4f); //modify with animation length
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }


    public void RaiseItem()
    {
        animator.SetBool("recieve item", true);
        currentState = PlayerState.interact;
        receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
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

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}