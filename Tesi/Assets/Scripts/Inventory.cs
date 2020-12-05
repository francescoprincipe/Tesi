using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public Sprite sprite;
    public int count;
}



[System.Serializable]
public class Inventory
{
    List<InventoryItem> itemList = new List<InventoryItem>();

    [SerializeField]
    GameObject[] itemSlots = new GameObject[4];
}
