using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Boss : Enemy
{
    bool enraged;
	public bool isInvulnerable = false;
    public GameSignal bossSignal;

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
                StartCoroutine(AttackCo());
            }
        }
    }

    public IEnumerator AttackCo()
    {
        currentState = EnemyState.attack;
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
}
