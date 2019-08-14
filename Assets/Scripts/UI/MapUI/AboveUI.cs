// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    public class AboveUI : MonoBehaviour
    {
        // 게임오브젝트의 이름을 메서드 이름과 같게 함
        private string MethodName;

        // place는 씬의 장소를 가리킴. 현재 MyHouse 씬이라면 place는 MyHouse 
        public GameObject place;

        private void Start()
        {
            MethodName = gameObject.name;
        }

        public void ButtonClicked()
        {
            place.SendMessage(MethodName);
        }

        public void OnUI()
        {
            gameObject.SetActive(true);
        }

        public void OffUI()
        {
            gameObject.SetActive(false);
        }



    }

}