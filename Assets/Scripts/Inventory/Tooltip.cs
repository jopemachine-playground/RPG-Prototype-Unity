using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public Item item;
    public Text itemNameText;
    public Text itemDescText;
    public Text itemRarityText;
    public Image itemIcon;

    public void Initialize()
    {
        itemNameText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemName").gameObject.GetComponent<Text>();
        itemDescText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemDesc").gameObject.GetComponent<Text>();
        itemRarityText = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemDetail").gameObject.GetComponent<Text>();
        itemIcon = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").Find("ItemIcon").gameObject.GetComponent<Image>();
    }

    public void ActivateTooltip()               
    {
        GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").gameObject.SetActive(true);
    }

    public void DeactivateTooltip()             
    {
        this.enabled = false;
    }



}
