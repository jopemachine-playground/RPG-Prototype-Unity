// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    public class Village : Scene
    {
        // 유니티에서 셋팅
        public GameObject EntryPointFromDungeon;
        public GameObject EntryPointFromHouse;
        public Camera cam;

        private void Start()
        {
            base.PlayerInit();
            base.ScreenCoverInit();

            // Village, My House 에선 고정된 각도의 카메라를 사용할 것이므로
            // Cinemachine Cam은 꺼 둔다
            base.CinemachineCamOff();

            FadeIn();
            playerControl.NoInputMode = false;

            ControlChange(new Vector3(1.5f, 1.5f, 1.5f), cam.transform, 0, 10);
            MoveCharacter();
            MapNameIndicator.Instance.IndicateMapName(placeName);
            MusicManager.mInstance.Play(backGroundMusic[0]);
        }

        public override void MoveCharacter()
        {
            switch (previousScene)
            {
                case "MyHouse":
                    Goto(EntryPointFromHouse.transform.position);
                    break;
                case "Dungeon1":
                    Goto(EntryPointFromDungeon.transform.position);
                    break;
                default:
                    player.transform.position = EntryPointFromDungeon.transform.position;
                    break;
            }
        }

        #region Handle A Button Clicked Events At UI Emerging Points
        // 아래의 메서드 네임은 UI Emerging Points 내 자식 컴포넌트들의 이름과 동일해야 함

        public void GotoDungeon()
        {
            MoveScene("Dungeon1");
        }

        public void GotoMyHouse()
        {
            MoveScene("MyHouse");
        }

        public void Shop()
        {
            SceneManager.LoadSceneAsync("Shop", LoadSceneMode.Additive);
        }

        #endregion



    }

}