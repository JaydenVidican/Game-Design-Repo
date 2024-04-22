using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class begin : MonoBehaviour
{
    SceneTransition levelManager = new SceneTransition();
    
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Return))
        {
            levelManager.begin();
        }
    }
}
