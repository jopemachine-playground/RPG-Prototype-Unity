using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

// 플레이어의 Inventory를 띄우는 창

class InventorySystem : MonoBehaviour
{
    public List<ItemSlot> SlotList = new List<ItemSlot>();

    public Inventory playerInventory;

    private void Awake()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void Update()
    {
       
    }




}

