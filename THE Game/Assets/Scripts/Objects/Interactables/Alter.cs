using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using System;

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
                if (playerInv.artifactCount >= 3)
                {
                    SceneManager.LoadScene("Win");
                }
                else
                {
                    String[] lines = new string[2];
                    lines[0] = "I still need to collect all the artifact pieces";
                    lines[1] = "I need " + (3 - playerInv.artifactCount) + " pieces";
                    StartCoroutine(ShowDialogue(lines));
                }
                
            }

        }
    }
    IEnumerator ShowDialogue(String[] lines)
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(2f);
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
