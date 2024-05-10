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
    public Collider2D teleportArea;

    void Start()
    {
        currentState = EnemyState.walk;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        projectile.GetComponent<Projectile>().speed = 8;
    }
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
            Debug.Log("Step 1");
		}
		if (health % 4 == 0)
        {
            projectile.GetComponent<Projectile>().speed++;
            teleportArea.GetComponent<CircleCollider2D>();

            
            Bounds bounds = teleportArea.bounds;

        
            float randomX = UnityEngine.Random.Range(bounds.min.x + 5, bounds.max.x - 5);
            float randomY = UnityEngine.Random.Range(bounds.min.y + 5, bounds.max.y - 5);

            
            transform.position = new Vector3(randomX, randomY, transform.position.z);
        }
	}
}
