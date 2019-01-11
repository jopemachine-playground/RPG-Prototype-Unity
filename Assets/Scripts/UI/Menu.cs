using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public GameObject go;
    //public AudioManager theAudio;
    public string call_sound;

    private bool isActived;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActived = !isActived;

            if (isActived)
            {
                go.SetActive(true);
                //theAudio.Play(call_sound);
            }

            if (!isActived)
            {
                go.SetActive(false);
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
        isActived = !isActived;

        if (isActived)
        {
            go.SetActive(true);
            //theAudio.Play(call_sound);
        }

        if (!isActived)
        {
            go.SetActive(false);
            //theAudio.Play(call_sound);
        }
    }

}
