// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    public class Window : MonoBehaviour
    {
        private WindowObject[] windowObjects;

        private void Awake()
        {
            windowObjects = GameObject.FindObjectsOfType<WindowObject>();

            WindowObject[] swap = new WindowObject[windowObjects.Length];

            for (int i= 0; i< windowObjects.Length; i++)
            {
                swap[i] = IndexSet(i);
                windowObjects[i].gameObject.SetActive(windowObjects[i].isActived);
                windowObjects[i].gameObject.transform.localPosition = windowObjects[i].startPosition;
            }

            windowObjects = swap;
        }

        private WindowObject IndexSet(int type)
        {
            for (int i = 0; i < windowObjects.Length; i++)
            {
                if ((WindowObject.windowType) type == windowObjects[i].objectIndex)
                {
                    return windowObjects[i];
                }
            }

            Debug.Assert(false, "Wrong Index for window index search");

            return null;
        }

        public void OpenCloseButtonClicked(string type)
        {
            WindowObject.windowType typeIndex = (WindowObject.windowType) Enum.Parse(typeof(WindowObject.windowType) ,type);

            int index = (int) typeIndex;

            windowObjects[index].isActived = !windowObjects[index].isActived;

            if (windowObjects[index].isActived == true)
            {
                windowObjects[index].gameObject.SetActive(true);
            }

            if (windowObjects[index].isActived == false)
            {
                windowObjects[index].gameObject.SetActive(false);
            }
        }

        #region Other Button Click Event Handling

        public void ExitButtonClicked()
        {
            Application.Quit();
        }

        public void SaveButtonClicked()
        {
            PlayerInfo.mInstance.SaveData();
        }

        public void MuteButtonClicked()
        {

        }

        #endregion
    }
}