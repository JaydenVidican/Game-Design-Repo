using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : Interactable
{
    [Header("Contents")]
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;
    public BoolValue storedOpen;

    [Header("Signals and Dialog")]
    public GameSignal raiseItem;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;

    [Header("Animation")]
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = storedOpen.RuntimeValue;
        if (isOpen)
        {
            anim.SetBool("opened", true);
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (!isOpen)
            {
                OpenChest();
            }
            else
            {
                isOpened();
            }
        }
    }

    public void OpenChest()
    {
        dialogueBox.SetActive(true);
        dialogueText.text = contents.itemDescription;
        playerInventory.currentItem = contents;
        playerInventory.AddItem(contents);
        raiseItem.Raise();
        context.Raise();
        isOpen = true;
        anim.SetBool("opened", true);
        storedOpen.RuntimeValue = isOpen;
    }
    public void isOpened()
    {
        dialogueBox.SetActive(false);
        
        raiseItem.Raise();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}
