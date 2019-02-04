using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> Clips; 

    public AudioSource Source;

    void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    public void Play(int audioNumber, float volume = 1f)
    {
        Source.Stop();
        Source.volume = volume;
        Source.clip = Clips[audioNumber];
        Source.Play();
    }

    public void Stop()
    {
        Source.Stop();
    }

    public void Pause()
    {
        Source.Pause();
    }

    public void Mute()
    {
        Source.volume = 0;
    }

}
