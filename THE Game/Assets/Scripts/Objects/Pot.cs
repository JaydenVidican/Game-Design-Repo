using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator anim;
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
    private void MakeLoot()
    {
        Debug.Log("Step 1");
        if (thisLoot != null)
        {
            Debug.Log("Step 2");
            PowerUp current = thisLoot.LootPowerup();
            if (current != null)
            {
                Debug.Log("Step 3");
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
