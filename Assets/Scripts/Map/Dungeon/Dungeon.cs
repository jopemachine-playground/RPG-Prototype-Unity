using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    public class Dungeon : Scene
    {
        public static Scene thisScene;

        public Camera cam;

        [SerializeField]
        public List<EntryPoint> adjacentMap;

        public override void MoveCharacter()
        {
            //Debug.Log("MoveCharacter 실행");
            //Debug.Log(Scene.previousScene);

            for (int i = 0; i < adjacentMap.Count; i++)
            {
                //Debug.Log(i);
                // 엔트리 포인트의 이름을 전환할 씬의 이름과 같게할 것
                if (adjacentMap[i].nodeName == Scene.previousScene)
                {
                    //Debug.Log("Goto 실행");
                    Goto(adjacentMap[i].entrance.transform.position);
                    return;
                }
            }
        }

        private void Start()
        {
            thisScene = this;
            // Scene의 Init 를 호출해, 먼저 부모의 변수부터 초기화 시키고 진행
            base.PlayerInit();
            base.ScreenCoverInit();

            FadeIn();
            playerControl.NoInputMode = false;

            cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();

            ControlChange(CHARTER_DEFAULT_SCALE, cam.transform);

            MoveCharacter();
            MapNameIndicator.Instance.IndicateMapName(placeName);
        }
    }
}