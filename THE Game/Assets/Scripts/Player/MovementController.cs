using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public float baseAttack;
    private Rigidbody2D myRigidbody; //creates rigidbody object
    private Vector3 change; //creates vector object
    private Animator animator; //creates animator object
    private bool canSprint = true; //tells if user can sprint (if stamina added later on)
    public FloatValue currentHealth;
    public GameSignal playerHealthSignal;
    public VectorValue startingPosition;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;
    public GameSignal playerHit;
    public GameSignal reduceMagic;
    [Header("Projectile Info")]
    public GameObject projectile;
    public Item bow;
    SceneTransition levelManager;


    void Start()
    {
        levelManager = GameObject.FindObjectOfType<SceneTransition>();
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
        else if (Input.GetButtonDown("Second Weapon") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if(playerInventory.CheckForItem(bow))
            {
                if(playerInventory.currentMagic > 0)
                {
                    StartCoroutine(SecoundAttackCo());
                }
            }
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
        yield return new WaitForSeconds(.1f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f); //modify with animation length
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecoundAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        yield return new WaitForSeconds(.3f); //modify with animation length
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private void MakeArrow()
    {
            Vector2 temp = new Vector2 (animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.reduceMagic(arrow.magicCost);
            reduceMagic.Raise();
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if(currentState != PlayerState.interact)
            {
                animator.SetBool("recieve item", true);
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("recieve item", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
            }
        }
    }
    void UpdateAnimationAndMove() //moves character and updates the animation
    {
        if(change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
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
            levelManager.lose();
            
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            playerHit.Raise();
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}