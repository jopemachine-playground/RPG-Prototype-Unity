using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 던전 씬에서 시작하면 플레이어의 위치가 어떤 entry Point로 갱신되어야 할 지 모르기 때문에 버그가 발생함에 주의.
/// Playing 씬을 Active Scene으로 해 놓고, 다른 씬으로 이동해도 마찬가지로 버그가 발생한다.
/// </summary>

namespace UnityChanRPG
{
    public class Dungeon : Scene
    {
        public static Scene thisScene;

        public Camera cam;

        [SerializeField]
        public List<EntryPoint> adjacentMap;

        // 클리어된 던전 씬에선 Direct Light를 활성화 해, 편하게 이동할 수 있게한다.
        public bool IsCleared;

        public override void MoveCharacter()
        {
            for (int i = 0; i < adjacentMap.Count; i++)
            {
                // 엔트리 포인트의 이름을 전환할 씬의 이름과 같게할 것
                if (adjacentMap[i].nodeName == Scene.previousScene)
                {
                    Debug.Log("Goto 실행");
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