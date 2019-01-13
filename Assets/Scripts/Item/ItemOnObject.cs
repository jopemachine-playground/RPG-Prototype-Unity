using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemOnObject : MonoBehaviour                   //Saves the Item in the slot
{
    public Item mItem;                                       //Item 
    private Text mValueText;                                      //text for the itemValue
    private Image mImage;

    void Update()
    {
        mValueText.text = mItem.ItemValue.ToString();                     //sets the itemValue         
        mImage.sprite = mItem.ItemIcon;
        //GetComponent<ConsumeItem>().item = mItem;
    }

    void Start()
    {
        mImage = transform.GetChild(0).GetComponent<Image>();
        transform.GetChild(0).GetComponent<Image>().sprite = mItem.ItemIcon;                 //set the sprite of the Item 
        mValueText = transform.GetChild(1).GetComponent<Text>();                                  //get the text(itemValue GameObject) of the item
    }
}
