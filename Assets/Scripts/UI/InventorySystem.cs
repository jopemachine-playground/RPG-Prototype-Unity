using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

// 플레이어 Inventory의 Raw Image 컴포넌트에 접근해 관리

public class InventorySystem : MonoBehaviour
{
    public Image[] itemImage;

    public Text[] itemText;

    public Inventory playerInventory;

    private void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        GameObject[] ItemIcons = GameObject.FindGameObjectsWithTag("ItemIcon");
        GameObject[] ItemIconTexts = GameObject.FindGameObjectsWithTag("ItemIconText");

        itemImage = new Image[ItemIcons.Length];
        itemText = new Text[ItemIconTexts.Length];

        Debug.Assert(ItemIcons != null);

        for (int i = 0; i < ItemIcons.Length; i++)
        {
            for (int j = 0; j < ItemIcons.Length; j++)
            {
                if (ItemIcons[j].transform.parent.name == "Slot (" + i + ")")
                {
                    itemImage[i] = ItemIcons[j].GetComponent<Image>();
                    itemText[i] = ItemIconTexts[j].GetComponent<Text>();
                    break;
                }
            }
        }

        ItemIconUpdate();

    }

    private void Update()
    {

    }

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

