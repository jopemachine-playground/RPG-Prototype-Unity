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
        // FindObjectsOfType는 무작위로 찾기 때문에 정렬해줘야 함
        ItemSlot[] temp = FindObjectsOfType<ItemSlot>();

        Debug.Assert(temp != null);

        for (int i = 0; i < temp.Length; i++)
        {
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j].name == "Slot (" + i + ")")
                {
                    playerItems[i] = temp[j];
                    break;
                }
            }
        }
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
            // 처음으로 비어 있는 슬롯을 발견하면 check하고 index를 기억해놓음.
            if ((check == false) && playerItems[i].ItemExist == false)
            {
                checkIndex = i;
                check = true;
            }

            // 슬롯을 뒤져 같은 물품이 있다면 주운 PickUpItem의 Value만큼 증가시킴.
            if (playerItems[i].Item.ID == item.ID)
            {
                playerItems[i].Item.ItemValue += item.ItemValue;
                return;
            }
        }
        // 슬롯을 모두 뒤졌는데, 같은 물품이 없다면 체크해둔 슬롯에 아이템을 넣음
        playerItems[checkIndex].Item = item;
        playerItems[checkIndex].ItemExist = true;

        updateItemIndex();
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

