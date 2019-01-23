using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public Image[] itemImage;

    public Text[] itemText;

    public Inventory playerInventory;

    // GameManager에서 초기화
    public void Initialize()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        Debug.Assert(playerInventory != null);

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

    }

    private void Start()
    {
        ItemIconUpdate();
    }

    // 변경 필요
    public void ItemIconUpdate()
    {
        for (int i = 0; i < playerInventory.playerItems.Count; i++)
        {
            // 아이템이 해당 슬롯에 존재하지 않는다면 슬롯의 이미지를 비활성화
            if (playerInventory.playerItems[i].ItemExist == false)
            {
                itemImage[i].enabled = false;
                itemText[i].enabled = false;
            }

            // 아이템이 해당 슬롯에 존재한다면 존재하는 아이템의 ID로 ItemPool에서 Sprite를 가져온다.
            else
            {
                itemImage[i].sprite = ItemPool.mInstance.getItemIcon(playerInventory.playerItems[i].Item.ID);
                itemImage[i].enabled = true;

                if(playerInventory.playerItems[i].Item.ItemValue == 1)
                {
                    itemText[i].enabled = false;
                }
                else
                {
                    itemText[i].enabled = true;
                    itemText[i].text = "" + playerInventory.playerItems[i].Item.ItemValue;
                }
                
            }
        }

    }




}

