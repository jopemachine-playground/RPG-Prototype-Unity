using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UnityChanRPG
{
    public class InventorySystem : MonoBehaviour
    {

        public Image[] itemImage;
        public Text[] itemText;

        public Text moneyText;

        // GameManager에서 초기화
        public void Initialize()
        {
            int SlotValue = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").childCount;

            itemImage = new Image[SlotValue];
            itemText = new Text[SlotValue];

            for (int i = 0; i < SlotValue; i++)
            {
                Image itemIcon = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").GetChild(i).Find("Image").gameObject.GetComponent<Image>();
                Text itemIconText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").GetChild(i).Find("Text").gameObject.GetComponent<Text>();
                itemImage[i] = itemIcon.GetComponent<Image>();
                itemText[i] = itemIconText.GetComponent<Text>();
            }

            moneyText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Money Text").Find("Text").gameObject.GetComponent<Text>();

        }

        private void Start()
        {
            ItemIconUpdate();
        }

        public void ItemIconUpdate()
        {
            for (int i = 0; i < Inventory.mInstance.playerItems.Count; i++)
            {
                // 아이템이 해당 슬롯에 존재하지 않는다면 슬롯의 이미지를 비활성화
                if (Inventory.mInstance.playerItems[i].ItemExist == false)
                {
                    itemImage[i].enabled = false;
                    itemText[i].enabled = false;
                }

                // 아이템이 해당 슬롯에 존재한다면 존재하는 아이템의 ID로 ItemPool에서 Sprite를 가져온다.
                else
                {
                    itemImage[i].sprite = ItemPool.mInstance.getItemIcon(Inventory.mInstance.playerItems[i].Item.ID);
                    itemImage[i].enabled = true;

                    if (Inventory.mInstance.playerItems[i].Item.ItemValue == 1)
                    {
                        itemText[i].enabled = false;
                    }
                    else
                    {
                        itemText[i].enabled = true;
                        itemText[i].text = "" + Inventory.mInstance.playerItems[i].Item.ItemValue;
                    }

                }
            }

        }

        public void moneyUpdate()
        {
            moneyText.text = PlayerInfo.mInstance.player.Money.ToString();
        }

        private void Update()
        {
            ItemIconUpdate();
            moneyUpdate();
        }

    }
}

