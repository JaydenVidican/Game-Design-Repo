using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLineNPC : BoundedNPC
{
    public String[] lines;
    MovementController player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindWithTag("Player").GetComponent<MovementController>();
    }

    protected override void Update()
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
                talk();
            }
        }

    }
    void talk()
    {

        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(2f);
        }
    }

}
