// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

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

        // 던젼 씬에서 시작할 때 시작할 기본 위치
        public GameObject defaultPosition;

        public Camera cam;

        [SerializeField]
        public List<EntryPoint> adjacentMap;

        // 클리어된 던전 씬에선 Direct Light를 활성화 해, 편하게 이동할 수 있게한다.
        public bool IsCleared;

        public override void MoveCharacter()
        {
            // 디버깅, 단위 테스트 등의 이유로 던전에서 시작하면 previousScene이 null이므로 Goto를 defaultPosition에 적용해야 한다
            if (previousScene == null) 
            {
                Goto(defaultPosition.transform.position);
                return;
            }

            // previousScene에 해당하는 Entrance 포인트를 찾아, Goto로 캐릭터를 이동시킴
            for (int i = 0; i < adjacentMap.Count; i++)
            {
                // 엔트리 포인트의 이름을 전환할 씬의 이름과 같게할 것
                if (adjacentMap[i].nodeName == Scene.previousScene)
                {
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
            base.CinemachineCamOn();

            // 비를 활성화 한다
            GameObject.FindGameObjectWithTag("DungeonMap").transform.Find("Rain").gameObject.SetActive(true);

            IsCleared = FlagManager.dungeonCleared.GetFlag(SceneManager.GetActiveScene().name);

            // 클리어 된 던전이라면 Directional Light를 활성화 해 준다. (디폴트 값은 비활성화)
            if (IsCleared) {
                transform.Find("Dungeon Directional Light").gameObject.SetActive(true);
            }

            FadeIn();
            MusicManager.mInstance.Play(backGroundMusic[0]);
            playerControl.NoInputMode = false;

            cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Camera>();

            ControlChange(CHARACTER_DEFAULT_SCALE, cam.transform);

            MoveCharacter();
            
            MapNameIndicator.Instance.IndicateMapName(placeName);
        }
    }
}