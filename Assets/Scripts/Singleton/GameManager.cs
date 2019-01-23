using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MusicManager bgmManager;
    public Inventory invInitManager;
    public InventorySystem invSysInitManager;

    public int mSelectedMusicNumber;

    private void Awake()
    {
        invInitManager = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>(); 
        invSysInitManager = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Inventory").gameObject.GetComponent<InventorySystem>();
        bgmManager = FindObjectOfType<MusicManager>();

        if (bgmManager != null)
        {
            bgmManager.Play(mSelectedMusicNumber);
        }
        else
        {
            Debug.Assert(false, "Error - Music file not existed");
        }

        GameInitialize();

    }

    // 비활성화된 상태로 시작해, Awake()로 초기화하면, 게임 시작시 초기화되지 않는 컴포넌트들을 대신 초기화 함
    // 활성화된 상태로 시작하는 컴포넌트들의 경우 컴포넌트의 Awake에서 스스로 초기화 하도록 했음.
    private void GameInitialize()
    {
        invInitManager.Initialize();
        invSysInitManager.Initialize();
    } 

}
