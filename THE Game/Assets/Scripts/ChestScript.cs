using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public string tagName;
    public GameObject[] objects;
    public Transform spawnPoint;
    private bool isOpened = false;
    public LevelManager levelmanager;

    void OnCollisionEnter2D (Collision2D col)
    {
        if (!isOpened)
            OpenChest();

    }

    void OpenChest()
    {
        GameObject item = Instantiate(objects[Random.Range(0, objects.Length)], spawnPoint.position, spawnPoint.rotation) as GameObject;
        isOpened = true;
        levelmanager.counter();
    }
}
