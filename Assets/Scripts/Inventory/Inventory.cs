using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

class Inventory : MonoBehaviour
{
    [SerializeField]
    public List<Item> playerItems = new List<Item>();


    private void Start()
    {
        
    }

    private void Update()
    {
        updateItemIndex();
    }

    private void ConsumeItem(Item item)
    {

    }

    private void EquipItem(Item item)
    {

    }

    private void UnEquipItem(Item item)
    {

    }




    public void ItemPickup(Item item)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].ID == item.ID)
            {
                playerItems[i].ItemValue++;
                return;
            }
            else
            {
                playerItems.Add(item);
            }
        }
    }

    public void updateItemIndex()
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].indexItemInList = i;
        }
    }

}

