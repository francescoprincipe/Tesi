using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class InventoryItem
{
    public string name;
    public Sprite sprite;
    public int count;

    public InventoryItem (string NAME, Sprite SPRITE)
    {
        name = NAME;
        sprite = SPRITE;
        count = 1;
    }
}



[System.Serializable]
public class Inventory
{
    List<InventoryItem> itemList = new List<InventoryItem>();

    [SerializeField]
    public const int inventorySize = 4;

    [SerializeField]
    GameObject[] itemSlots = new GameObject[inventorySize];

    [SerializeField]
    GameObject[] itemCounters = new GameObject[inventorySize];

    public void AddItem(string name, Sprite sprite)
    {
        bool alreadyHave = false;
        int c = 0;

        //Check if already have
        foreach (InventoryItem item in itemList)
        {
            //If already have just update the count
            if (item.name == name)
            {
                item.count++;
                itemCounters[c].GetComponent<TextMeshProUGUI>().text = (item.count).ToString();
                alreadyHave = true;
                break;
            }

            c++;
        }

        //Otherwise insert new item
        if (!alreadyHave)
        {
            InventoryItem newItem = new InventoryItem(name, sprite);
            itemList.Add(newItem);
            itemSlots[itemList.IndexOf(newItem)].GetComponent<Image>().sprite = sprite;
            itemSlots[itemList.IndexOf(newItem)].GetComponent<Image>().color = Color.white;
            itemCounters[itemList.IndexOf(newItem)].GetComponent<TextMeshProUGUI>().text = "1";
        }
    }

    public void RemoveItem(string name)
    {
        int c = 0;

        //Remove all sprites
        foreach (InventoryItem item in itemList)
        {
           
            if (item.name == name)
            {
                item.count--;
                if (item.count == 0)
                {
                    itemSlots[c].GetComponent<Image>().sprite = null;
                    itemSlots[c].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
            }

            c++;
        }

        //Remove items from list
        itemList.RemoveAll(x => x.name == name && x.count == 0);
    }
}
