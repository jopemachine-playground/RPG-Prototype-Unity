using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityChanRPG
{
    public class ItemDeletePanel : ICheckPanel
    {

        private InventorySystem invSystem;

        public ItemSlot DeleteSlot;

        public void Start()
        {
            invSystem = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
        }

        public override void yesButtonClicked()
        {
            DeleteSlot.ItemExist = false;
            DeleteSlot.Item.ID = 0;
            DeleteSlot.Item.clean();
            gameObject.SetActive(false);
            invSystem.ItemIconUpdate();
        }


    }

}