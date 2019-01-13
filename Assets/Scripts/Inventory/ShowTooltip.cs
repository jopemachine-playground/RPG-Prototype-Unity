using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Item item;

    public Tooltip tooltip;                                     //The tooltip script
    public GameObject tooltipGameObject;                        //the tooltip as a GameObject
    public RectTransform canvasRectTransform;                    //the panel(Inventory Background) RectTransform
    public RectTransform tooltipRectTransform;                  //the tooltip RectTransform

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Tooltip") != null)
        {
            tooltip = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>();
            tooltipGameObject = GameObject.FindGameObjectWithTag("Tooltip");
            tooltipRectTransform = tooltipGameObject.GetComponent<RectTransform>() as RectTransform;
        }
        canvasRectTransform = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>() as RectTransform;
    }

    
    public void OnPointerEnter(PointerEventData data)                               //if you hit a item in the slot
    {

    }

    public void OnPointerExit(PointerEventData data)                //if we go out of the slot with the item
    {
        if (tooltip != null)
            tooltip.deactivateTooltip();            //the tooltip getting deactivated
    }


}