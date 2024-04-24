using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    [HideInInspector]
    public Vector2 direction;

    [Header("Lifetime")]
    public float lifetime;
    private float counter;

    [Header("Rigidbody")]
    public Rigidbody2D myRigidbody;

    
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        counter = lifetime;
    }

    
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Launch(Vector2 initialVel)
    {
        myRigidbody.velocity = initialVel * speed;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}
