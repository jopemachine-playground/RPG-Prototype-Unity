using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해
// 획득한 경우 슬롯 내용을 바꾸는 형식으로 구현할 것.

public class ItemSlot : MonoBehaviour
{
    public int IconSize = 45;

    public Item Item;

    public Text ItemCountText;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public bool ItemExist;


    // Use this for initialization
    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (ItemExist == false)
        {
            return;
        }


    }
}
