using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}
public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;
    public GameObject trigger;
    public AudioClip[] Sounds; // Array to hold footstep sound clips
    private AudioSource audioSource; // Reference to the Audio Source component

    void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Get the Audio Source component
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if(playerInRange && thisDoorType == DoorType.key)
            {
                if (playerInventory.numberOfKeys > 0)
                {
                    playerInventory.numberOfKeys--;
                    Open();
                }

            }
        }
        if (open)
        {
            StartCoroutine(DeactivateAfterDelay(1f)); // delay of 0.1 seconds
        }
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        trigger.SetActive(false);
    }

    public void Open()
    {
        AudioClip doorSound = Sounds[0];
        audioSource.PlayOneShot(doorSound);
        doorSprite.enabled = false;
        open = true;
        physicsCollider.enabled = false;
    }
    public void Close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
    }
}
