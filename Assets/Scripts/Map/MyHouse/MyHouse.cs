using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    public class MyHouse : Scene
    {
        // 유니티에서 셋팅
        public GameObject myHouseEntryPoint;
        public GameObject sleepingRoomEntryPoint;
        public GameObject livingRoomEntryPoint;
        public Camera livingRoomCam;
        public Camera sleepingRoomCam;

        private void Start()
        {
            // Scene의 Init 를 호출해, 먼저 부모의 변수부터 초기화 시키고 진행
            base.PlayerInit();
            base.ScreenCoverInit();

            FadeIn();
            playerControl.NoInputMode = false;

            ControlChange(CHARACTER_DEFAULT_SCALE, livingRoomCam.transform, 0, 5);
            MoveCharacter();
            MapNameIndicator.Instance.IndicateMapName(placeName);
        }

        public override void MoveCharacter()
        {
            player.transform.position = myHouseEntryPoint.transform.position;
        }

        #region Handle A Button Clicked Events At UI Emerging Points
        // 아래의 메서드 네임은 UI Emerging Points 내 자식 컴포넌트들의 이름과 동일해야 함

        public void GotoSleepingRoom()
        {
            Goto(sleepingRoomEntryPoint.transform.position);
            playerControl.CameraChange(sleepingRoomCam.transform);
            CameraMove(livingRoomCam, sleepingRoomCam);
        }

        public void GotoLivingRoom()
        {
            Goto(livingRoomEntryPoint.transform.position);
            playerControl.CameraChange(livingRoomCam.transform);
            CameraMove(sleepingRoomCam, livingRoomCam);
        }

        public void GotoVillage()
        {
            MoveScene("Village");
        }

        public void WareHouse()
        {

        }

        public void Sleeping()
        {

        }

        #endregion


    }
}

