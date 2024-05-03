using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class BoundedNPC : Sign
{
    Vector3 directionVector;
    Transform myTransform;
    public float speed;
    Rigidbody2D myRigidbody;
    Animator anim;
    public Collider2D bounds;
    bool isMoving;
    public float minMoveTime;
    public float maxMoveTime;
    float moveTimeSeconds;
    public float minWaitTime;
    public float maxWaitTime;
    float waitTimeSeconds;
    public bool canMove;
    void Start()
    {
        moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
        waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (canMove)
        {
            ChangeDirection();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            if (isMoving)
            {
                moveTimeSeconds -= Time.deltaTime;
                if (moveTimeSeconds <= 0)
                {
                    moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                    isMoving = false;
                    anim.SetBool("Moving", false);
                }
                if (!playerInRange)
                {
                    Move();
                }

            }
            else
            {
                waitTimeSeconds -= Time.deltaTime;
                if (waitTimeSeconds <= 0)
                {
                    NewDirection();
                    isMoving = true;
                    waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }
    }

    void NewDirection()
    {
        Vector3 temp = directionVector;
        ChangeDirection();
        while(temp == directionVector)
        {
            ChangeDirection();
        }
    }
    void Move()
    {
        Vector3 temp = myTransform.position + directionVector * speed * Time.deltaTime;
        if(bounds.bounds.Contains(temp))
        {
            myRigidbody.MovePosition(temp);
            anim.SetBool("Moving", true);
        }
        else
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        int directon = Random.Range(0, 4);

        switch (directon)
        {
            case 0:
                directionVector = Vector3.right;
                break;
            case 1:
                directionVector = Vector3.up;
                break;
            case 2:
                directionVector = Vector3.left;
                break;
            case 3:
                directionVector = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnim();
    }

    void UpdateAnim()
    {
        anim.SetFloat("moveX", directionVector.x);
        anim.SetFloat("moveY", directionVector.y);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        NewDirection();
    }
}


