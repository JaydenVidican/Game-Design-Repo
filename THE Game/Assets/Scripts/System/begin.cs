using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class begin : MonoBehaviour
{
    LevelManager levelManager = new LevelManager();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Return))
        {
            levelManager.LoadLevel("Room 1");
        }
    }
}
