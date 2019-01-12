using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject mMenu;
    public GameObject mInventory;
    //public AudioManager theAudio;
    public string call_sound;

    private bool isMenuActived;
    private bool isInvenoryActived;

    private void Awake()
    {
        isInvenoryActived = false;
    }

    // Update is called once per frame
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

    public void ShowInventory()
    {
        isInvenoryActived = !isInvenoryActived;
        if (isInvenoryActived)
        {
            mInventory.SetActive(true);
            //theAudio.Play(call_sound);
        }

        if (!isInvenoryActived)
        {
            mInventory.SetActive(false);
            //theAudio.Play(call_sound);
        }
    }

}
