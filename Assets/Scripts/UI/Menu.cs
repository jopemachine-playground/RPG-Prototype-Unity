using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject mMenu;
    public GameObject mInventory;
    public GameObject mEquipment;
    //public AudioManager theAudio;
    public string call_sound;

    private bool isMenuActived;
    private bool isInvenoryActived;
    private bool isEquipmentActived;

    private void Awake()
    {
        isMenuActived = false;
        isInvenoryActived = false;
        isEquipmentActived = false;
        mMenu.SetActive(false);
        mInventory.SetActive(false);
        mEquipment.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuActived = !isMenuActived;

            if (isMenuActived)
            {
                mMenu.SetActive(true);
                //theAudio.Play(call_sound);
            }

            if (!isMenuActived)
            {
                mMenu.SetActive(false);
                //theAudio.Play(call_sound);
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        windowOpenClose(mMenu, ref isMenuActived);
    }

    public void ShowInventory()
    {
        windowOpenClose(mInventory, ref isInvenoryActived);
    }

    public void ShowEquipment()
    {
        windowOpenClose(mEquipment, ref isEquipmentActived);
    }

    private void windowOpenClose(GameObject obj, ref bool actived)
    {
        actived = !actived;
        if (actived)
        {
            obj.SetActive(true);
            //theAudio.Play(call_sound);
        }

        if (!actived)
        {
            obj.SetActive(false);
            //theAudio.Play(call_sound);
        }
    }
}
