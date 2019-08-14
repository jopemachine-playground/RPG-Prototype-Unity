// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 인벤토리 관련 클래스들은 https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310 를 많이 참고해 작성함

/// <summary>
/// 슬롯에서 드래그 이벤트가 일어나면 DraggingItem으로 아이템의 정보를 복사함. 
/// 드래그가 끝난 후의 이벤트 처리 역시 DraggingItem에서 처리함에 주의할 것
/// </summary>

namespace UnityChanRPG
{
    public class DraggingItem : MonoBehaviour, IDropHandler, IManualInitializeable
    {
        public int indexInInventory;
        public Item item;
        public Image itemIcon;
        public bool IsExist;

        // 드래그가 어디에서 시작되었는지 나타낼 bool 변수
        public bool DragFromHotbarSlot;
        public bool DragFromItemSlot;

        // 마우스 포인터 위치
        public RectTransform pointerOffset;

        public InventorySystem invSystem;

        // ItemSlot들에 대한 참조
        public ItemSlot[] itemSlots;
        public HotbarSlot[] hotbarSlots;

        // Inventory 창의 Rect 객체
        public RectTransform invRect;
        public RectTransform hotbarRect;

        public void Initialize()
        {
            itemIcon = gameObject.GetComponent<Image>();
            pointerOffset = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<RectTransform>();
            invSystem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
            invRect = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<RectTransform>();
            hotbarRect = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("Hotbar").gameObject.GetComponent<RectTransform>();

            int itemSlotsLength = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").childCount;
            itemSlots = new ItemSlot[itemSlotsLength];
            int hotbarSlotsLength = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("Hotbar").Find("Slots").childCount;
            hotbarSlots = new HotbarSlot[hotbarSlotsLength];

            for (int i = 0; i < itemSlotsLength; i++)
            {
                itemSlots[i] = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").GetChild(i).gameObject.GetComponent<ItemSlot>();
            }

            for (int i = 0; i < hotbarSlotsLength; i++)
            {
                hotbarSlots[i] = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("Hotbar").Find("Slots").GetChild(i).gameObject.GetComponent<HotbarSlot>();
            }
        }

        // Dragging Item이 마우스를 따라다니게 하려면, OnDrop을 Dragging Item에 구현해 놓아야 한다.
        // 따라서, DraggingItem에서 모든 개체의 OnDrop 이벤트를 처리한다.
        public void OnDrop(PointerEventData data)
        {
            if (IsExist == false)
            {
                return;
            }

            if (DragFromItemSlot == true)
            {
                // 드래그가 인벤토리에 된 경우
                if (RectTransformUtility.RectangleContainsScreenPoint(invRect, data.position))
                {
                    for (int i = 0; i < itemSlots.Length; i++)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(itemSlots[i].rect, data.position))
                        {
                            // 빈 슬롯이라면 dragging Item을 그대로 대입.
                            if (itemSlots[i].ItemExist == false)
                            {
                                itemSlots[i].Item = item.getCopy();
                                itemSlots[i].ItemExist = true;
                                Inventory.mInstance.playerItems[indexInInventory].Item.clean();
                                Inventory.mInstance.playerItems[indexInInventory].ItemExist = false;
                                break;
                            }
                            // 아이템이 있는 슬롯이라면 dragging Item과 ItemSlot의 Item을 교체
                            else
                            {
                                Item temp = itemSlots[i].Item.getCopy();
                                itemSlots[i].Item = item.getCopy();
                                Inventory.mInstance.playerItems[indexInInventory].Item = temp.getCopy();
                                break;
                            }
                        }
                    }
                }

                // 드래그가 핫바에 된 경우, 핫바에 아이템을 등록
                else if (RectTransformUtility.RectangleContainsScreenPoint(hotbarRect, data.position))
                {
                    for (int i = 0; i < hotbarSlots.Length; i++)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(hotbarSlots[i].rect, data.position))
                        {
                            hotbarSlots[i].item = itemSlots[indexInInventory].Item.getCopy();
                            hotbarSlots[i].ItemExist = true;
                        }
                    }
                }

                // 드래그가 인벤토리 바깥 쪽에 된 경우 아이템을 버릴 것이냐는 질의를 띄운다.
                else
                {
                    itemSlots[indexInInventory].ShowDeletePanel();
                }

            }

            if (DragFromHotbarSlot == true)
            {

            }

        }
    }
}