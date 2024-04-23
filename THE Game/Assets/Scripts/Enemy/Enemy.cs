using System.Collections;
using System.Collections.Generic;
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
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Vector2 homePosition;

    [Header("Death Effects")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;

    [Header("Death Signals")]
    public GameSignal roomSignal;
    protected bool isDead;

    private void Awake()
    {
        homePosition = transform.position;
        health = maxHealth.initialValue;
        isDead = false;
    }

    private void OnEnable()
    {
        if (isDead)
        {
            this.gameObject.SetActive(false);
        }
        transform.position = homePosition;
    }
    
    private void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
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

    private void MakeLoot()
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
    private void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }
}
