using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanRPG
{
    public class Tooltip : MonoBehaviour, IManualInitializeable
    {
        public Item item;
        public Text itemNameText;
        public Text itemDescText;
        public Text itemRarityText;
        public Image itemIcon;

        public GameObject tooltipGameobj;

        public WaitForSeconds showingTime;
        public float showingTimeForInit;

        public void Initialize()
        {
            showingTime = new WaitForSeconds(showingTimeForInit);
            tooltipGameobj = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").gameObject;
            itemNameText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemName").gameObject.GetComponent<Text>();
            itemDescText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemDesc").gameObject.GetComponent<Text>();
            itemRarityText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemDetail").gameObject.GetComponent<Text>();
            itemIcon = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemIcon").gameObject.GetComponent<Image>();
        }

        public void CopyItemInfoToTooltip(Item _item)
        {
            itemNameText.text = _item.Name;
            itemDescText.text = _item.Description;
            itemRarityText.text = "레어도 : " + _item.Rarity;
            itemIcon.sprite = _item.ItemIcon;
        }

        public void ActivateTooltip()
        {
            tooltipGameobj.SetActive(true);
        }

        public void DeactivateTooltip()
        {
            tooltipGameobj.SetActive(false);
        }

    }
}