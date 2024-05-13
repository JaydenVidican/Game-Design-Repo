using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerup : PowerUp
{
    public Inventory playerInventory;
    public float magicValue;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerInventory.currentMagic += magicValue;
            if (playerInventory.currentMagic > 10)
            {
                playerInventory.currentMagic = 10;
            }
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
