using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item :  Interactable
{
    [SerializeField]
    string itemName;
    [SerializeField]
    Sprite inventorySprite;

    public string GetName()
    {
        return itemName;
    }

    public Sprite GetInventorySprite()
    {
        return inventorySprite;
    }
}
