using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    public Vector2 cameraMax;
    public Vector2 cameraMin;
    public Vector3 playerChange;
    private CameraMovement cam;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            cam.minPosition = cameraMin;
            cam.maxPosition = cameraMax;
            other.transform.position += playerChange;
        }
    }
}
