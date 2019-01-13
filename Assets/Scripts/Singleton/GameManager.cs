using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MusicManager bgmManager;
    public int mSelectedMusicNumber;

    private void Awake()
    {
        bgmManager = FindObjectOfType<MusicManager>();

        if (bgmManager != null)
        {
            bgmManager.Play(mSelectedMusicNumber);
        }
        else
        {
            Debug.Assert(false, "Error - Music file not existed");
        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
