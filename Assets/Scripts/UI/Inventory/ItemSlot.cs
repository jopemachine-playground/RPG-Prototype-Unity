// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     1. ItemSlot은 인벤토리 창에서 하나 하나의 슬롯에 대응함. 슬롯을 더블클릭하거나, 우클릭 했을 때의 이벤트 처리 역시 ItemSlot에서 함.
// @ Issue : 
// @     1. 나중에 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해
// @     2. 획득한 경우 슬롯 내용을 바꾸는 형식으로 확장할 생각임.
// @ Ref URLs : 
// @     1. https://www.youtube.com/watch?v=-ow-Dp17mYY
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace UnityChanRPG
{
    [RequireComponent(typeof(Button))]
    public class ItemSlot : MonoBehaviour, IPointerExitHandler, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler
    {
        [SerializeField]
        public Item Item;

        // 슬롯에 아이템이 존재하는가.
        public bool ItemExist;

        public int IndexItemInList;

        // 우클릭 및 더블 클릭 이벤트를 처리하기 위한 버튼 
        [SerializeField]
        public Button button;
        private int clickCounter;
        public float clickTimer = .5f;

        // Dragging Item에서 ItemSlot에 마우스 포인터가 와 있는지 확인하기 위한 자기 자신의 RectTransform
        public RectTransform rect;

        public static ItemDeletePanel panel;
        public static Tooltip tooltip;
        public static DraggingItem draggingItem;

        // 툴팁이 마우스 바로 옆에 뜨면 어색해, 마우스와 약간 거리를 두게 했음.
        private static Vector3 TooltipDistanceFromMouse = new Vector3(20, -40);


        #region Pointer Event
        // ToolTip을 활성화
        public void OnPointerEnter(PointerEventData data)
        {
            if (!ItemExist) return;

            tooltip.CopyItemInfoToTooltip(this.Item);
            tooltip.transform.position = data.position;

        }

        // ToolTip을 비활성화
        public void OnPointerExit(PointerEventData data)
        {
            tooltip.DeactivateTooltip();
        }

        // 오른쪽 클릭을 처리. 오른쪽 클릭 시 아이템을 삭제할 것이냐고 묻는 창을 띄움.
        public void OnPointerClick(PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Right && panel.gameObject.activeSelf == false)
            {
                ShowDeletePanel();
            }
        }

        // Dragging Item으로 복사
        public void OnBeginDrag(PointerEventData data)
        {
            if (ItemExist == false)
            {
                return;
            }
            draggingItem.item = this.Item.getCopy();
            draggingItem.IsExist = this.ItemExist;
            draggingItem.indexInInventory = this.IndexItemInList;
            draggingItem.itemIcon.sprite = draggingItem.item.ItemIcon;
            draggingItem.DragFromItemSlot = true;
        }

        public void OnDrag(PointerEventData data)
        {
            if (ItemExist == false)
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
            draggingItem.DragFromItemSlot = false;
        }

        #endregion

        private void ButtonListener()
        {
            clickCounter++;

            if (clickCounter == 1)
            {
                StartCoroutine("DoubleClickEvent");
            }

        }

        private IEnumerator DoubleClickEvent()
        {
            yield return new WaitForSeconds(clickTimer);

            if (clickCounter > 1)
            {
                this.UseItem();
            }
            yield return new WaitForSeconds(.5f);
            clickCounter = 0;
        }

        void Start()
        {
            rect = GetComponent<RectTransform>();
            button = GetComponent<Button>();

            button.onClick.AddListener(ButtonListener);

            #region Init static Variable
            // 첫 초기화시에만 실행되어 할당됨
            if (panel == null)
            {
                panel = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Check Panel").gameObject.GetComponent<ItemDeletePanel>();
            }

            if (tooltip == null)
            {
                tooltip = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").gameObject.GetComponent<Tooltip>();
            }

            if (draggingItem == null)
            {
                draggingItem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<DraggingItem>();
            }

            #endregion

            #region UNUSED CODE
            //var doubleLeftClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
            //var rightClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));

            //// Update를 돌며 클릭과 더블클릭을 나눠 입력 받으려 했지만, UniRx란 걸 알게 되어 이걸로 구현 (모든 이벤트 처리를 UniRx로 구현하진 않고 더블클릭만 이걸로 구현했음)
            //button.onClick
            //    .AsObservable()
            //    .Buffer(doubleLeftClickStream.Throttle(TimeSpan.FromMilliseconds(100)))
            //    .Where(xs => xs.Count >= 2)
            //    .Subscribe(_ =>
            //    {
            //        this.UseItem();
            //    });
            #endregion
        }

        private void Update()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
            {
                if (!ItemExist) return;

                tooltip.transform.position = Input.mousePosition + TooltipDistanceFromMouse;
                tooltip.ActivateTooltip();
            }

        }

        // 아이템을 오른쪽 클릭하거나 드래그로 바깥에 끌면, 삭제 질의가 나온다.
        public void ShowDeletePanel()
        {
            panel.DeleteSlot = this;
            panel.gameObject.SetActive(true);
        }

        public void UseItem()
        {
            if (Item.ItemType == ItemType.UseAble)
            {
                Item.Consume();
            }

            else if (Item.ItemType == ItemType.EquipAble)
            {
                Item.Equip();
            }

            if (Item.ItemValue == 0) this.ItemExist = false;
        }


    }
}