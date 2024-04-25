using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLog : Log
{
    public GameObject projectile;
    public float fireDelay;
    private float delayCounter;
    private bool canFire = true;
    private Vector3 tempVector;
    private GameObject current;

    void Update()
    {
        delayCounter -= Time.deltaTime;
        if (delayCounter <= 0)
        {
            canFire = true;
            delayCounter = fireDelay;
        }
    }

    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {  
            Debug.Log("Hi");
            if(currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                if(canFire)
                {
                tempVector = target.transform.position - transform.position;
                current = Instantiate(projectile, transform.position, Quaternion.identity);
                current.GetComponent<Projectile>().Launch(tempVector);
                canFire = false;
                ChangeState(EnemyState.walk);
                anim.SetBool("wakeUp", true);
                }
            }
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            anim.SetBool("wakeUp", false);
        }
    }
}
