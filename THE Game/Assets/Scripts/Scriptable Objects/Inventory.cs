using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
[System.Serializable]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int coins;
    public float maxMagic = 10;
    public float currentMagic;
    public int artifactCount;
    public int swordUpgradeCount;

    public void OnEnable()
    {
        currentMagic = maxMagic;
    }

    public void reduceMagic(float magicCost)
    {
        currentMagic -= magicCost;
    }

    public bool CheckForItem(Item item)
    {
        if(items.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddItem(Item itemToAdd)
    {
        if (itemToAdd.isKey)
        {
            numberOfKeys++;
        }
        else if (itemToAdd.isArtifact)
        {
            artifactCount++;
        }
        else if (itemToAdd.isSwordUpgrade)
        {
            swordUpgradeCount++;
        }
        else
        {
            if (!items.Contains(itemToAdd))
            {
                items.Add(itemToAdd);
            }
        }
    }
    public void Reset()
    {
        numberOfKeys = 0;
        coins = 0;
        items.Clear();
        currentMagic = maxMagic;
        artifactCount = 0;
        swordUpgradeCount = 0;
    }
}
