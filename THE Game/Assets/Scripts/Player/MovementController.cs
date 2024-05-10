using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    Rigidbody2D myRigidbody; //creates rigidbody object
    Vector3 change; //creates vector object
    Animator animator; //creates animator object
    bool canSprint = true; //tells if user can sprint (if stamina added later on)
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
    
    [Header("Boss tracking")]
    [HideInInspector]
    public bool boss1Death;
    [HideInInspector]
    public bool boss2Death;
    [HideInInspector]
    public bool boss3Death;
    public FloatValue bossCount;

    [Header("Footsteps")]
    public AudioClip[] Sounds; // Array to hold footstep sound clips
    public float minTimeBetweenFootsteps = 0.3f; // Minimum time between footstep sounds
    public float maxTimeBetweenFootsteps = 0.6f; // Maximum time between footstep sounds

    private AudioSource audioSource; // Reference to the Audio Source component
    private float timeSinceLastFootstep; // Time since the last footstep sound

    [Header("IFrames")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;
    


    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Get the Audio Source component
    }
    void Start()
    {
        currentState = PlayerState.idle;
        animator = GetComponent<Animator>(); //accesses animations
        myRigidbody = GetComponent<Rigidbody2D>(); //accesses rigig body component of player
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        transform.position = startingPosition.initialValue;
    }

    void Update() //checks every frame if player is holding spring key
    {
        if (Input.GetKey("left shift") && canSprint == true) 
            speed = 8;
        else
            speed = 5;

        // Check if the player is walking
        if (currentState == PlayerState.walk)
        {
            // Check if enough time has passed to play the next footstep sound
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                // Play a random footstep sound from the array
                AudioClip footstepSound = Sounds[0];
                audioSource.PlayOneShot(footstepSound);

                timeSinceLastFootstep = Time.time; // Update the time since the last footstep sound
            }
        }
    }

    void FixedUpdate()
    {
        if (currentState == PlayerState.interact || currentState == PlayerState.attack)
        {
            return;
        }
        change = Vector3.zero; 
        change.x = Input.GetAxisRaw("Horizontal"); //gets horizontal movement
        change.y = Input.GetAxisRaw("Vertical"); //gets vertical movement
        if (change.x != 0 || change.y != 0)
        {
            currentState = PlayerState.walk;
        }
        else if (change.x == 0 && change.y == 0 && currentState != PlayerState.attack && currentState != PlayerState.interact)
        {
            currentState = PlayerState.idle;
        }
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

    IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        AudioClip swordSound = Sounds[1];
        audioSource.PlayOneShot(swordSound);
        yield return new WaitForSeconds(.1f);
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.35f); //modify with animation length
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.idle;
        }
    }

    IEnumerator SecoundAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        AudioClip arrowSound = Sounds[2];
        audioSource.PlayOneShot(arrowSound);
        MakeArrow();
        yield return new WaitForSeconds(.3f); //modify with animation length
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.idle;
        }
    }

    public void Upgrade()
    {
        baseAttack += playerInventory.swordUpgradeCount * 3;
    }
    void MakeArrow()
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
        AudioClip HurtSound = Sounds[3];
        audioSource.PlayOneShot(HurtSound);
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("Lose");
            
        }
    }
    IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            playerHit.Raise();
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    public void updateArtifactCount()
    {
        playerInventory.artifactCount++;
    }

    public void updateBoss()
    {
        Debug.Log("TEst");
        bossCount.RuntimeValue++;
        if (bossCount.RuntimeValue == 1)
        {
            updateBoss1();
        }
        else if (bossCount.RuntimeValue == 2)
        {
            updateBoss2();
        }
        else if (bossCount.RuntimeValue == 3)
        {
            updateBoss3();
        }
    }

    void updateBoss1()
    {
        boss1Death = true;
    }   
    void updateBoss2()
    {
        boss2Death = true;
    }   
    void updateBoss3()
    {
        boss3Death = true;
    }   
}