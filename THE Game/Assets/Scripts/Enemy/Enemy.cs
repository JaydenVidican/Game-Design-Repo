using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    public FloatValue maxHealth;
    protected float health;
    public int baseAttack;
    public float moveSpeed;
    [HideInInspector]
    public Vector2 homePosition;

    [Header("Death Effects")]
    public GameObject deathEffect;
    protected float deathEffectDelay = 1f;
    public LootTable thisLoot;

    [Header("Death Signals")]
    public GameSignal roomSignal;
    protected bool isDead;
    protected Rigidbody2D myRigidbody;
    [HideInInspector]
    public Transform target;
    [Header("Target Variables")]
    public float chaseRadius;
    public float attackRadius;
    [HideInInspector]
    public Animator anim;

    private void Awake()
    {
        homePosition = transform.position;
        health = maxHealth.initialValue;
        isDead = false;
    }

    void Start()
    {
        currentState = EnemyState.walk;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        CheckDistance();
    }

    private void OnEnable()
    {
        if (isDead)
        {
            this.gameObject.SetActive(false);
        }
        transform.position = homePosition;
    }
    
    public virtual void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
           Die();
        }
    }

    protected void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerup();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(KnockCo(myRigidbody, knockTime));
            takeDamage(damage);
        }
    }
    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    public virtual void CheckDistance()
    {
         if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
    }

    public void setAnimFLoat(Vector2 setVector)
    {
        anim.SetFloat("moveX", setVector.x);
        anim.SetFloat("moveY", setVector.y);
    }
    public void changeAnim(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                setAnimFLoat(Vector2.right);
            }
            else if (direction.x < 0)
            {
                setAnimFLoat(Vector2.left);
            }
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            if (direction.y > 0)
            {
                setAnimFLoat(Vector2.up);
            }
            else if (direction.y < 0)
            {
                setAnimFLoat(Vector2.down);
            }
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

    protected void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    protected void Die()
	{
		DeathEffect();
        MakeLoot();
        if (roomSignal != null)
        {
            roomSignal.Raise();
        }
        isDead = true;
        this.gameObject.SetActive(false);
	}
}
