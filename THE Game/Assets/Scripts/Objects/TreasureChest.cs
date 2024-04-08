using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : Interactable
{
    public Item contents;
    public Inventory playerInventory;
    public bool isOpen;
    public GameSignal raiseItem;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
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
        isOpen = true;
        context.Raise();
    }
    public void isOpened()
    {
        dialogueBox.SetActive(false);
        playerInventory.currentItem = null;
        raiseItem.Raise();
    }
}
