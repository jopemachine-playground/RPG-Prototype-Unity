// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 비활성화된 상태로 시작하거나 초기화 순서에 민감해, 따로 수동으로 초기화 해야 하는 클래스들을 관리함
/// </summary>

namespace UnityChanRPG
{
    public class InitDelegator : MonoBehaviour
    {
        [NonSerialized]
        public IManualInitializeable invInitManager;
        [NonSerialized]
        public IManualInitializeable invSysInitManager;
        [NonSerialized]
        public IManualInitializeable tooltipInitManager;
        [NonSerialized]
        public IManualInitializeable draggingItemInitManager;

        private void Awake()
        {
            invInitManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            invSysInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
            tooltipInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").gameObject.GetComponent<Tooltip>();
            draggingItemInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<DraggingItem>();

            invInitManager.Initialize();
            invSysInitManager.Initialize();
            tooltipInitManager.Initialize();
            draggingItemInitManager.Initialize();
        }

    }
}