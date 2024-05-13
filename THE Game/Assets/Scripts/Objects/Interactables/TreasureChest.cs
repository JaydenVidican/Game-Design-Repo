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
    public GameSignal upgrade;
    public GameObject dialogBox;
    public TMP_Text dialogText;

    [Header("Animation")]
    protected Animator anim;
    

	// Use this for initialization
	protected override void Start () {
        base.Start();
        anim = GetComponent<Animator>();

        isOpen = storedOpen.RuntimeValue;
        if(isOpen)
        {
            anim.SetBool("opened", true);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Interact") && playerInRange)
        {
            if(!isOpen)
            {
                // Open the chest
                OpenChest();
            }else
            {
                // Chest is already open
                ChestAlreadyOpen();
            }
        }
    }

    public void OpenChest()
    {
        // Dialog window on
        dialogBox.SetActive(true);
        if (miniMap != null)
                {
                    miniMap.SetActive(false);
                }
        // dialog text = contents text
        dialogText.text = contents.itemDescription;
        // add contents to the inventory
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        if (contents.isSwordUpgrade)
        {
            upgrade.Raise();
        }
        // Raise the signal to the player to animate
        raiseItem.Raise();
        // raise the context clue
        context.Raise();
        // set the chest to opened
        isOpen = true;
        anim.SetBool("opened", true);
        storedOpen.RuntimeValue = isOpen;
    }

    public void ChestAlreadyOpen()
    {
        // Dialog off
        dialogBox.SetActive(false);
        if (miniMap != null)
                {
                    miniMap.SetActive(true);
                }
        playerInRange = false;
        // raise the signal to the player to stop animating
        raiseItem.Raise();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            context.Raise();
            playerInRange = false;
        }
    }
}