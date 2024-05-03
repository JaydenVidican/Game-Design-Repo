using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : DungeonEnemyRoom
{
    public GameObject bossRoom;
    public MovementController player;

    void Start()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], true);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], true);
            }
            virtualCamera.SetActive(true);
            if( !player.boss1Death)
            {
                StartCoroutine(Delay());
            }
        }
        
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        //bossRoom.SetActive(true);
    }

    public void open()
    {
        bossRoom.SetActive(false);
    }
}
