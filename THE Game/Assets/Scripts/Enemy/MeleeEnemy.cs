using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

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
}
