using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// 비활성화된 상태로 시작해, Awake()로 초기화하면, 게임 시작시 초기화되지 않는 객체들 을 대신 초기화 함
// 활성화된 상태로 시작하는 컴포넌트들의 경우 컴포넌트의 Awake에서 스스로 초기화 하도록 했음.

public class InitManager : MonoBehaviour
{
    [NonSerialized]
    public MusicManager bgmManager;
    [NonSerialized]
    public Inventory invInitManager;
    [NonSerialized]
    public InventorySystem invSysInitManager;
    [NonSerialized]
    public Tooltip tooltipInitManager;
    [NonSerialized]
    public DraggingItem draggingItemInitManager;
    [NonSerialized]
    public LevelInfo levelInfoInitManager;

    private void Awake()
    {
        invInitManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
        invSysInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
        tooltipInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Tooltip").gameObject.GetComponent<Tooltip>();
        draggingItemInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("DraggingItem").gameObject.GetComponent<DraggingItem>();
        levelInfoInitManager = GameObject.FindGameObjectWithTag("Manager").transform.Find("PlayerInfo Manager").gameObject.GetComponent<LevelInfo>();
        bgmManager = FindObjectOfType<MusicManager>();

        invInitManager.Initialize();
        invSysInitManager.Initialize();
        tooltipInitManager.Initialize();
        draggingItemInitManager.Initialize();
        levelInfoInitManager.Initialize();
    }

}
