// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    public class LoadingScene : Scene
    {
        public Camera cam;

        private void Start()
        {
            base.PlayerInit();
            base.ScreenCoverInit();
            screenCover.gameObject.SetActive(false);
            playerControl.CameraChange(cam.transform);
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            yield return null;

            Cursor.visible = false;

            AsyncOperation load = SceneManager.LoadSceneAsync(destinationScene, LoadSceneMode.Single);

            Time.timeScale = 1;

            Cursor.visible = true;
        }

        public override void MoveCharacter()
        {

        }
    }
}