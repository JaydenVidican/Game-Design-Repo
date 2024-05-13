using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sign : Interactable
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string dialogue;
    protected virtual void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInRange)
        {
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
                if (miniMap != null)
                {
                    miniMap.SetActive(true);
                }
            }
            else
            {
                dialogueBox.SetActive(true);
                if (miniMap != null)
                {
                    miniMap.SetActive(false);
                }
                dialogueText.text = dialogue;
            }

        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            context.Raise();
            playerInRange = false;
            dialogueBox.SetActive(false);
            miniMap.SetActive(true);
        }
    }
}
