using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    [Serializable]
    public class WindowObject : MonoBehaviour
    {
        [NonSerialized]
        public GameObject obj;

        // isActived == true라면 기본 설정으로 창을 켜 놓음
        public bool isActived;

        // windowType은 컴포넌트들끼리 겹치지 않는다고 가정함
        public windowType objectIndex;

        // startPosition은 WindowObject를 처음 호출했을 때, 창의 위치
        public Vector3 startPosition;

        private void Start()
        {
            obj = gameObject;
        }

        public enum windowType
        {
            Menu = 0,
            Inventory = 1,
            Equipment = 2,
            Option = 3,
            Map = 4,
        }

    }
}
