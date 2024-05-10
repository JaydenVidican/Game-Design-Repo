using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactChest : TreasureChest
{

    Collider2D[] colliders;
    Collider2D myCol;
    Collider2D trigger;
    SpriteRenderer mySprite;
    void Start () {
        colliders = GetComponents<Collider2D>();
        anim = GetComponent<Animator>();
        myCol = colliders[0];
        trigger = colliders[1];
        mySprite = GetComponent<SpriteRenderer>();

        myCol.enabled = false;
        trigger.enabled = false;
        mySprite.enabled = false;

        isOpen = storedOpen.RuntimeValue;
        if(isOpen)
        {
            anim.SetBool("opened", true);
        }
    }

    public void Show()
    {
        myCol.enabled = true;
        trigger.enabled = true;
        mySprite.enabled = true;
    }
}
