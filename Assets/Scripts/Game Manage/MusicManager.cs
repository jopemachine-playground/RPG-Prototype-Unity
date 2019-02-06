using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    static public MusicManager mInstance;

    public AudioClip[] mClips; // 배경음악 파일들

    public AudioSource mSource;

    // 미리 만들어 둬 성능에 영향을 안 끼치게 하자
    private WaitForSeconds FADEINOUT_WAITTIME = new WaitForSeconds(0.01f);

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

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0f; i -= 0.01f)
        {
            mSource.volume = i;
            yield return FADEINOUT_WAITTIME;
        }
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0.0f; i <= 1.0f; i += 0.01f)
        {
            mSource.volume = i;
            yield return FADEINOUT_WAITTIME;
        }
    }

    public void SetVolumn(float volumn)
    {
        mSource.volume = volumn;
    }

    public void Pause()
    {
        mSource.Pause();
    }
}
