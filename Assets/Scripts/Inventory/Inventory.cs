using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

// 플레이어의 인벤토리

class Inventory : MonoBehaviour
{
    [SerializeField]
    public List<Item> playerItems = new List<Item>();

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
            playerItems[i].IndexItemInList = i;
        }
    }

    public Item getItemByID(int id)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].ID == id)
                return playerItems[i].getCopy();
        }
        return null;
    }

    public Item getItemByName(string name)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].Name.ToLower().Equals(name.ToLower()))
                return playerItems[i].getCopy();
        }
        return null;
    }


}

