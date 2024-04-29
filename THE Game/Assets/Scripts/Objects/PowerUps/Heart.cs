using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUp
{
    public FloatValue playerHealth;
    public float amountToIncrease;
    public FloatValue heartContainers;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth.RuntimeValue += amountToIncrease;
            if (playerHealth.RuntimeValue > heartContainers.RuntimeValue * 4)
            {
                playerHealth.RuntimeValue = heartContainers.RuntimeValue * 4;
            }
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
