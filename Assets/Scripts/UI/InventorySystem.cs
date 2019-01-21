using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

// 플레이어 Inventory의 Raw Image 컴포넌트에 접근해 관리

class InventorySystem : MonoBehaviour
{
    public RawImage[] itemImage;

    public Inventory playerInventory;

    private void Awake()
    {       
        GameObject[] temp = GameObject.FindGameObjectsWithTag("ItemIcon");

        itemImage = new RawImage[temp.Length];

        Debug.Assert(temp != null);

        for (int i = 0; i < temp.Length; i++)
        {
            for (int j = 0; j < temp.Length; j++)
            {
                if (temp[j].transform.parent.name == "Slot (" + i + ")")
                {
                    itemImage[i] = temp[j].GetComponent<RawImage>();
                    break;
                }
            }
        }

    }

    private void Update()
    {

    }




}

