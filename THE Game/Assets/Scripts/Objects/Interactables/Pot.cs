using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    Animator anim;
    public LootTable thisLoot;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Smash()
    {
        anim.SetBool("smash", true);
        StartCoroutine(breakCo());
    }
    void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerup();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    IEnumerator breakCo()
    {
        MakeLoot();
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}
