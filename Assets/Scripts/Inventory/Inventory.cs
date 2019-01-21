using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

// 플레이어의 인벤토리

class Inventory : MonoBehaviour
{
    [SerializeField]
    public List<ItemSlot> playerItems = new List<ItemSlot>();

    private void Awake()
    {
        // FindObjectsOfType는 무작위로 찾기 때문에 정렬해줘야 함.
        ItemSlot[] temp = FindObjectsOfType<ItemSlot>();

        Debug.Assert(temp != null);

        for (int i = 0; i < temp.Length; i++)
        {
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j].name == "Slot" + (i + 1))
                {
                    playerItems[i] = temp[j];
                    break;
                }
            }
        }
    }

    private void Update()
    {
        updateItemIndex();
    }

    private void ConsumeItem(Item item)
    {
        Debug.Assert(item.ItemType == ItemType.UseAble, "Error - Item Use Bug Occured. - ConsumeItem() In Inventory.cs");

    }

    private void EquipItem(Item item)
    {
        Debug.Assert(item.ItemType == ItemType.EquipAble, "Error - Item Use Bug Occured. - EquipItem() In Inventory.cs");

    }

    private void UnEquipItem(Item item)
    {
        Debug.Assert(item.ItemType == ItemType.EquipAble, "Error - Item Use Bug Occured. - UnEquipItem() In Inventory.cs");


    }

    public void ItemPickup(Item item)
    {
        int checkIndex = 0;
        bool check;
        check = false;

        for (int i = 0; i < playerItems.Count; i++)
        {
            if ((check == false) && playerItems[i].ItemExist == false)
            {
                checkIndex = i;
                check = true;
            }

            if (playerItems[i].Item.ID == item.ID)
            {
                playerItems[i].Item.ItemValue++;
                return;
            }
            else
            {
                Debug.Assert(check == true, "Error - Pickup Bug Occured. - ItemPickup() In Inventory.cs");
                playerItems[checkIndex].Item = item;
                playerItems[checkIndex].ItemExist = true;
            }
        }
    }

    public void updateItemIndex()
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].Item.IndexItemInList = i;
        }
    }

    public Item getItemByID(int id)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].Item.ID == id)
                return playerItems[i].Item.getCopy();
        }
        return null;
    }

    public Item getItemByName(string name)
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            if (playerItems[i].Item.Name.ToLower().Equals(name.ToLower()))
                return playerItems[i].Item.getCopy();
        }
        return null;
    }


}

