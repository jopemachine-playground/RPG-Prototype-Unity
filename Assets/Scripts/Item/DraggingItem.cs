using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggingItem : MonoBehaviour, IDropHandler
{
    public int indexInInventory;
    public Item item;
    public Image itemIcon;

    // 마우스 포인터 위치
    public RectTransform pointerOffset;

    public InventorySystem invSystem;

    // ItemSlot들에 대한 참조
    public ItemSlot[] itemSlots;

    // Inventory 창의 Rect 객체
    public RectTransform invRect;

    public void Initialize()
    {
        itemIcon = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<Image>();
        pointerOffset = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<RectTransform>();
        invSystem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
        invRect = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<RectTransform>();

        int Length = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").childCount;
        itemSlots = new ItemSlot[Length];

        for (int i = 0; i< Length; i++)
        {
            itemSlots[i] = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").Find("Item Grid Slot").Find("Slot (" + i + ")").gameObject.GetComponent<ItemSlot>();
        }
    }

    // 처음엔 각 개체에서 OnDrop을 구현하려 했으나, 그렇게하면 Dragging Item이 마우스를 따라다니게 하면 안 됨.
    // 따라서, DraggingItem에서 모든 개체의 OnDrop을 처리하도록 함.
    public void OnDrop(PointerEventData data)
    {

        // 드래그가 인벤토리에 된 경우
        if (RectTransformUtility.RectangleContainsScreenPoint(invRect, data.position)) {

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(itemSlots[i].rect, data.position))
                {
                    // 빈 슬롯이라면 dragging Item을 그대로 대입.
                    if (itemSlots[i].ItemExist == false)
                    {
                        itemSlots[i].Item = item;
                        itemSlots[i].ItemExist = true;
                        Inventory.mInstance.playerItems[indexInInventory].Item.clean();
                        Inventory.mInstance.playerItems[indexInInventory].ItemExist = false;
                        Inventory.mInstance.updateItemIndex();
                        invSystem.ItemIconUpdate();
                        break;
                    }
                    // 아이템이 있는 슬롯이라면 dragging Item과 ItemSlot의 Item을 교체
                    else
                    {
                        Item temp = itemSlots[i].Item.getCopy();
                        itemSlots[i].Item = item;
                        Inventory.mInstance.playerItems[indexInInventory].Item = temp;
                        Inventory.mInstance.updateItemIndex();
                        invSystem.ItemIconUpdate();
                        break;
                    }
                }

            }
        }

        // 드래그가 인벤토리 바깥 쪽에 된 경우 아이템을 버릴 것이냐는 질의를 띄운다.
        else
        {
            itemSlots[indexInInventory].ShowDeletePanel();
        }
        

        
    }
}
