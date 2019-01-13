using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    private Vector2 pointerOffset;
    private RectTransform rectTransform;
    private RectTransform rectTransformSlot;
    private CanvasGroup canvasGroup;
    private GameObject oldSlot;
    private Inventory inventory;
    private Transform draggedItemBox;

    public delegate void ItemDelegate();
    public static event ItemDelegate updateInventoryList;

    void Start()
    {

    }


    public void OnDrag(PointerEventData data)
    {

    }



    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out pointerOffset);
            oldSlot = transform.parent.gameObject;
        }
        if (updateInventoryList != null)
            updateInventoryList();
    }

    public void createDuplication(GameObject Item)
    {

    }

    public void OnEndDrag(PointerEventData data)
    {

    }

}
