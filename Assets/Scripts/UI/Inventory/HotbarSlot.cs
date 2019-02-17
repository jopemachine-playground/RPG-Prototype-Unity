using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityChanRPG
{
    public class HotbarSlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public Item item;

        public RectTransform rect;

        public static DraggingItem draggingItem;

        public int IndexInHotbar;

        public bool ItemExist;

        private void Start()
        {
            rect = GetComponent<RectTransform>();

            if (draggingItem == null)
            {
                draggingItem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<DraggingItem>();
            }
        }

        public void UseItem()
        {
            item.use();
            if (item.ItemValue == 0) this.ItemExist = false;
        }

        #region Pointer Event

        public void OnBeginDrag(PointerEventData data)
        {
            if (!ItemExist)
            {
                return;
            }
            draggingItem.item = this.item.getCopy();
            draggingItem.indexInInventory = this.IndexInHotbar;
            draggingItem.itemIcon.sprite = draggingItem.item.ItemIcon;
            draggingItem.DragFromHotbarSlot = true;
        }

        public void OnDrag(PointerEventData data)
        {
            if (!ItemExist)
            {
                return;
            }
            draggingItem.itemIcon.enabled = true;
            draggingItem.pointerOffset.position = data.position;
        }

        public void OnEndDrag(PointerEventData data)
        {
            draggingItem.itemIcon.enabled = false;
            draggingItem.item.clean();
            draggingItem.DragFromHotbarSlot = false;
        }

        // 오른쪽 클릭을 처리. 오른쪽 클릭 시 핫바에서 아이템을 해제.
        public void OnPointerClick(PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Right)
            {
                item.clean();
                ItemExist = false;
            }
        }

        #endregion

    }
}
