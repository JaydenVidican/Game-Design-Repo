using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Alter : Interactable
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    string dialogue;
    public Inventory playerInv;
    protected virtual void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInRange)
        {
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
            }
            else
            {
                dialogueBox.SetActive(true);
                if (playerInv.artifactCount == 3)
                {
                    SceneManager.LoadScene("Win");
                }
                else
                {
                    dialogueText.text = "I still need to collect all the artifact pieces";
                }
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
        }
    }
}
