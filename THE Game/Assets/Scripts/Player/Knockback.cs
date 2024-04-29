using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;

    public GameSignal boss2;
    
    void Update()
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            damage = GetComponent<Enemy>().baseAttack;
        }
        else if (this.gameObject.CompareTag("Enemy Sword"))
        {
            damage = this.gameObject.transform.parent.GetComponent<Enemy>().baseAttack;
        }
        else if (this.gameObject.CompareTag("Sword"))
        {
            damage = this.gameObject.transform.parent.GetComponent<MovementController>().baseAttack;
        }
        else if (this.gameObject.CompareTag("Enemy Projectile") || this.gameObject.CompareTag("Player Projectile") || this.gameObject.CompareTag("Boss2"))
        {

        }
        else
        {
            damage = 1;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.CompareTag("Boss2"))
        {
            if (other.gameObject.CompareTag("Reflected Projectile"))
            {
                if(boss2 != null)
                {
                    boss2.Raise();
                }
            }
            else
            {
                doDamage(other);
            }
        }
        else
        {
        if (other.gameObject.CompareTag("Breakable") && this.gameObject.CompareTag("Sword"))
        {
            other.GetComponent<Pot>().Smash();
        }
        if ((other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player")) && (this.gameObject.CompareTag("Sword") || this.gameObject.CompareTag("Enemy Projectile") || this.gameObject.CompareTag("Enemy Sword") || this.gameObject.CompareTag("Enemy") || this.gameObject.CompareTag("Player Projectile")))
        {
            doDamage(other);
        }
        }
    }

    void doDamage(Collider2D other)
    {
        Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if(other.gameObject.CompareTag("Enemy") && other.isTrigger)
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }
                if(other.gameObject.CompareTag("Player"))
                {
                    if (other.GetComponent<MovementController>().currentState != PlayerState.stagger)
                    {
                        hit.GetComponent<MovementController>().currentState = PlayerState.stagger;
                        other.GetComponent<MovementController>().Knock(knockTime, damage);
                    }
                }
            }
    }
}
