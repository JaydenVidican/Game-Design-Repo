using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLog : Log
{
    public Collider2D boundry;

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius 
        && Vector3.Distance(target.position, transform.position) > attackRadius 
        && boundry.bounds.Contains(target.transform.position))
        {
            if(currentState == EnemyState.idle || currentState == EnemyState.walk 
            && currentState != EnemyState.stagger)
            {
                anim.SetBool("wakeUp", true);
                //yield return new WaitForSeconds(.3f);
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius || !boundry.bounds.Contains(target.transform.position))
        {
            anim.SetBool("wakeUp", false);
        }
    }
}
