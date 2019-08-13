// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 싱글톤으로 전역에서 접근 가능하며, 장소에 따라 배경음악을 바꿈
/// 음악 파일에 해당하는 AudioClip은 장소에 해당하는 컴포넌트들에서 갖고 있음
/// </summary>

namespace UnityChanRPG
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        static public MusicManager mInstance;

        private AudioSource mSource;

        private WaitForSeconds FADEINOUT_WAITTIME = new WaitForSeconds(0.01f);

        private void Awake()
        {
            if (mInstance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                mInstance = this;
                mSource = GetComponent<AudioSource>();
            }
        }

        public void Play(AudioClip selectedMusic, bool ApplyFadeIn = true)
        {
            mSource.volume = 1f;
            mSource.clip = selectedMusic;

            if (ApplyFadeIn == true)
            {
                FadeInMusic();
            }

            mSource.Play();
        }

        public void Stop(bool ApplyFadeOut = true)
        {
            if (ApplyFadeOut == true)
            {
                FadeOutMusic();
            }
            else
            {
                mSource.Stop();
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

        public void FadeInMusic()
        {
            StopAllCoroutines();
            StartCoroutine(FadeInMusicCoroutine());
        }

        public void FadeOutMusic()
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutMusicCoroutine());
        }

        private IEnumerator FadeInMusicCoroutine()
        {
            for (float i = 0.0f; i <= 1.0f; i += 0.01f)
            {
                mSource.volume = i;
                yield return FADEINOUT_WAITTIME;
            }
        }

        // 음악을 페이드 아웃하며, Stop 시킴
        private IEnumerator FadeOutMusicCoroutine()
        {
            for (float i = 1.0f; i >= 0f; i -= 0.01f)
            {
                mSource.volume = i;
                yield return FADEINOUT_WAITTIME;
            }

            mSource.Stop();
        }
    }
}