using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Boss : Enemy
{
    bool enraged;
    public GameSignal bossSignal;

    [Header("IFrames")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    float wait = .5f;

    public override void CheckDistance()
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
        else if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            if(currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                StartCoroutine(AttackCo(wait));
            }
        }
    }

    public IEnumerator AttackCo(float waitTime)
    {
        currentState = EnemyState.attack;
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(.5f);
        currentState = EnemyState.stagger;
        anim.SetBool("attack", false);
        yield return new WaitForSeconds(2f);
        currentState = EnemyState.walk;
    }


	public override void takeDamage(float damage)
	{
        health -= damage;

        if (health <= 0)
		{
			Die();
            bossSignal.Raise();
		}
		else if (health <= 30) //enraged
		{
            if (!enraged)
            {
                moveSpeed *= 2;
                baseAttack *= 2;
                wait /= 2;
            }
            else if (enraged)
            {
                health -= damage;
            }
            enraged = true;

		}
	}
    public bool checkIfDead()
    {
        return isDead;
    }

    protected override IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    protected IEnumerator FlashCo()
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
}
