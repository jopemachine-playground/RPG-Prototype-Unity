// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    public class UIEmergingPoint : MonoBehaviour
    {
        private BoxCollider UIIndicatingArea;

        public AboveUI UI;

        private void Start()
        {
            UI = GameObject.FindGameObjectWithTag("AboveUI").transform.Find(gameObject.name).gameObject.GetComponent<AboveUI>();

            UIIndicatingArea = GetComponent<BoxCollider>();

            UI.OffUI();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                UI.OnUI();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                UI.OffUI();
            }
        }

        private void Update()
        {
            if (UI.gameObject.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    UI.ButtonClicked();
                }
            }
        }


    }

}