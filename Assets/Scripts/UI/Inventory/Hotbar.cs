// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityChanRPG
{
    public class Hotbar : MonoBehaviour, IDropHandler
    {
        public HotbarSlot[] hotbarItems;

        public Image[] itemImage;

        public Text[] itemText;

        public KeyCode HOTKEY1 = KeyCode.Alpha1;
        public KeyCode HOTKEY2 = KeyCode.Alpha2;
        public KeyCode HOTKEY3 = KeyCode.Alpha3;
        public KeyCode HOTKEY4 = KeyCode.Alpha4;
        public KeyCode HOTKEY5 = KeyCode.Alpha5;
        public KeyCode HOTKEY6 = KeyCode.Alpha6;
        public KeyCode HOTKEY7 = KeyCode.Alpha7;
        public KeyCode HOTKEY8 = KeyCode.Alpha8;

        public void Start()
        {
            Transform slots = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("Hotbar").Find("Slots");

            hotbarItems = new HotbarSlot[slots.childCount];
            itemImage = new Image[slots.childCount];
            itemText = new Text[slots.childCount];

            for (int i = 0; i < slots.childCount; i++)
            {
                hotbarItems[i] = slots.GetChild(i).gameObject.GetComponent<HotbarSlot>();
                itemImage[i] = slots.GetChild(i).Find("Image").gameObject.GetComponent<Image>();
                itemText[i] = slots.GetChild(i).Find("Text").gameObject.GetComponent<Text>();
            }
        }

        public void OnDrop(PointerEventData data)
        {

        }

        public void Update()
        {
            ItemIconUpdate();
        }

        public void ItemIconUpdate()
        {
            for (int i = 0; i < hotbarItems.Length; i++)
            {
                if (hotbarItems[i].ItemExist == true)
                {
                    int index = Inventory.mInstance.getItemIndexByID(hotbarItems[i].item.ID);

                    // 아이템이 인벤토리에 더 이상 존재하지 않는다면 슬롯의 이미지를 비활성화
                    if (index == -1)
                    {
                        hotbarItems[i].ItemExist = false;
                        itemImage[i].enabled = false;
                        itemText[i].enabled = false;
                    }

                    // 아이템이 여전히 인벤토리에 존재한다면 존재하는 아이템의 ID로 Inventory에서 아이템 정보를 가져온다.
                    else
                    {
                        itemImage[i].enabled = true;
                        itemImage[i].sprite = Inventory.mInstance.playerItems[index].Item.ItemIcon;
                        itemText[i].enabled = true;
                        itemText[i].text = "" + Inventory.mInstance.playerItems[index].Item.ItemValue;
                    }
                }

            }

        }
    }

}