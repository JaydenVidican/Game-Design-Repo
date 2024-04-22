using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DungeonEnemyRoom : DungeonRoom
{
    public Door[] doors;

    void Start()
    {
        OpenDoors();
    }

    public int EnemiesActive()
    {
        int activeEnemies = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.activeInHierarchy)
            {
                activeEnemies++;
            }
        }
        return activeEnemies;
    }

    public void CheckEnemies()
    {
        if (EnemiesActive() == 1)
            {
                OpenDoors();
            }
        
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
            CloseDoors();
        }
        
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                ChangeActivation(enemies[i], false);
            }
            for (int i = 0; i < pots.Length; i++)
            {
                ChangeActivation(pots[i], false);
            }
        }
    }

    public void CloseDoors()
    {
        for(int i = 0; i < doors.Length; i++)
        {
            Debug.Log(i);
            doors[i].Close();
        }
        
    }
    public void OpenDoors()
    {
        Debug.Log("Test");
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].Open();
        }
    }
}
