// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     플레이어, 몬스터, 총알 등 소리를 내는 컴포넌트의 자식에 부착해 사용
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
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
}