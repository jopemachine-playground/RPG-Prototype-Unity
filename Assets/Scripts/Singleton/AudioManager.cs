using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    static public AudioManager mInstance;

    public AudioClip[] mClips; // 배경음악 파일들

    public AudioSource mSource;

    void Start()
    {
        mSource = GetComponent<AudioSource>();

        if (mSource == null) Debug.Log("Error - mSource not initated");

    }

    private void Awake()
    {
        if (mInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            mInstance = this;
        }
    }

    public void Play(int musicNumber)
    {
        mSource.volume = 1f;
        mSource.clip = mClips[musicNumber];
        mSource.Play();
    }

    public void Stop()
    {
        mSource.Stop();
    }

    public void Pause()
    {
        mSource.Pause();
    }

    public void Mute()
    {
        mSource.volume = 0;
    }

}
