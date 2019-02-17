using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

// 인벤토리 관련 클래스들은 https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310 를 많이 참고해 작성함

namespace UnityChanRPG
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory mInstance;

        private void Awake()
        {
            if (mInstance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                mInstance = this;
            }
        }

        [SerializeField]
        public List<ItemSlot> playerItems = new List<ItemSlot>();

        public InventorySystem invSystem;


        public void Initialize()
        {
            // 비활성화 되어 있는 컴포넌트에 접근할 경우, 우선 활성화된 부모 컴포넌트에 접근해야 한다.
            invSystem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();

            int SlotValue = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").childCount;

            for (int i = 0; i < SlotValue; i++)
            {
                ItemSlot itemSlot = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").GetChild(i).gameObject.GetComponent<ItemSlot>();
                itemSlot.IndexItemInList = i;
                playerItems[i] = itemSlot;
            }
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
                    invSystem.ItemIconUpdate();
                    return;
                }
            }
            // 슬롯을 모두 뒤졌는데, 같은 물품이 없다면 체크해둔 슬롯에 아이템을 넣음
            playerItems[checkIndex].Item = item;
            playerItems[checkIndex].ItemExist = true;

        }

        #region Get Item Info

        public int getItemIndexByID(int id)
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i].Item.ID == id)
                    return playerItems[i].IndexItemInList;
            }

            return -1;
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
        #endregion


    }

}