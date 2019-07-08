using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Scene은 맵들의 스크립트들에서 사용할 abstract class.

namespace UnityChanRPG
{
    abstract public class Scene : MonoBehaviour
    {
        // 장소에 따라 달라지는, 플레이어의 관련된 정보에 접근해 변경 가능
        [NonSerialized]
        public GameObject player;
        [NonSerialized]
        public PlayerControl playerControl;
        protected Vector3 CHARACTER_DEFAULT_SCALE = new Vector3(1f, 1f, 1f);

        // 유니티 에디터에서 셋팅해 줘야함
        public GameObject AboveUIParent;
        public List<AudioClip> backGroundMusic;
        public string placeName;

        protected static Image screenCover;
        // 각각 페이드 인, 아웃이 일어나는 시간과, 명암도 조절 시간
        private const float SCENE_MOVE_WAITTIME = 0.75f;
        private const float FADEINOUT_SPEED_MULTIPLIER = 1.5f;
        private WaitForSeconds FADEINOUT_WAITTIME = new WaitForSeconds(0.001f);
        private Color FADEIN_INITIAL = new Color(0, 0, 0, 1f);
        private Color FADEOUT_INITIAL = new Color(0, 0, 0, 0f);

        // 로딩 씬을 거쳐 도착할 씬의 string
        protected static string previousScene;
        protected static string destinationScene;

        protected GameObject cineCam;

        // Player에 대한 조작이 필요한 경우, Start들에서 호출해 사용
        protected void PlayerInit()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cineCam = GameObject.FindGameObjectWithTag("Cinemachine Cam");
            playerControl = player.GetComponent<PlayerControl>();
            Debug.Assert(player != null);
            Debug.Assert(playerControl != null);
        }

        protected void ScreenCoverInit()
        {
            if (screenCover == null)
            {
                screenCover = GameObject.FindGameObjectWithTag("UI").transform.Find("Screen Cover").gameObject.GetComponent<Image>();
            }
        }

        protected void ControlChange(Vector3 characterScale, Transform camTr, float jumpPower = 10, float moveSpeed = 10)
        {
            player.transform.localScale = characterScale;
            playerControl.MoveSpeed = moveSpeed;
            playerControl.JumpPower = jumpPower;
            playerControl.CameraChange(camTr);
        }

        abstract public void MoveCharacter();

        public void MoveScene(string sceneName)
        {
            previousScene = SceneManager.GetActiveScene().name;
            destinationScene = sceneName;
            playerControl.NoInputMode = true;
            FadeOut();
            MusicManager.mInstance.Stop();
            Invoke("LoadingSceneLoad", SCENE_MOVE_WAITTIME);
        }

        private void LoadingSceneLoad()
        {
            SceneManager.LoadScene("Loading", LoadSceneMode.Single);
        }

        protected void FadeIn()
        {
            StopAllCoroutines();
            StartCoroutine("FadeInScreen");
        }

        protected void FadeOut()
        {
            StopAllCoroutines();
            StartCoroutine("FadeOutScreen");
        }

        protected IEnumerator FadeInScreen()
        {
            screenCover.gameObject.SetActive(true);

            screenCover.color = FADEIN_INITIAL;

            while (screenCover.color.a >= 0)
            {
                screenCover.color =
                    new Color(
                        screenCover.color.r,
                        screenCover.color.g,
                        screenCover.color.b,
                        screenCover.color.a - FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
                     );

                yield return FADEINOUT_WAITTIME;
            }

            screenCover.gameObject.SetActive(false);

        }

        protected IEnumerator FadeOutScreen()
        {
            screenCover.gameObject.SetActive(true);

            screenCover.color = FADEOUT_INITIAL;

            while (screenCover.color.a <= 1)
            {
                screenCover.color =
                    new Color(
                        screenCover.color.r,
                        screenCover.color.g,
                        screenCover.color.b,
                        screenCover.color.a + FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
                    );

                yield return FADEINOUT_WAITTIME;
            }
        }

        protected void CinemachineCamOn() {

            var obj = cineCam.transform.Find("Cinemachine Cam").gameObject;

            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
            }
        }
        protected void CinemachineCamOff()
        {
            var obj = cineCam.transform.Find("Cinemachine Cam").gameObject;

            if (obj.activeSelf == true)
            {
                obj.SetActive(false);
            }
        }

        #region Move In Current Scene

        protected void Goto(Vector3 dest)
        {
            playerControl.controller.enabled = false;
            player.transform.position = dest;
            playerControl.controller.enabled = true;

            if (AboveUIParent != null)
            {
                AboveUIParent.BroadcastMessage("OffUI");
            }
        }

        protected void Goto(Vector3 dest, Vector3 lookAt)
        {
            playerControl.controller.enabled = false;
            player.transform.position = dest;

            playerControl.controller.enabled = true;
            playerControl.LookAtAndInitAngle(lookAt);

            if (AboveUIParent != null)
            {
                AboveUIParent.BroadcastMessage("OffUI");
            }
        }
        protected void CameraMove(Camera prev, Camera dest)
        {
            dest.gameObject.SetActive(true);
            prev.gameObject.SetActive(false);
        }

        #endregion

    }
}