using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBoss : Boss
{
    public GameObject projectile;
    public float fireDelay;
    float delayCounter;
    bool canFire = true;
    Vector3 tempVector;
    GameObject current;

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
            if(currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                if(canFire)
                {
                tempVector = target.transform.position - transform.position;
                current = Instantiate(projectile, transform.position, Quaternion.identity);
                current.GetComponent<Projectile>().Launch(tempVector);
                canFire = false;
                ChangeState(EnemyState.walk);
                }
            }
        }
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
            
		}
	}
}
