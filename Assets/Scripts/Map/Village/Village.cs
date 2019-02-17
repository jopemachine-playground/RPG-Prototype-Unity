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

            ControlChange(new Vector3(1.5f, 1.5f, 1.5f), cam.transform, 0, 10);
            MoveCharacter();
            Debug.Log(placeName);
            MapNameIndicator.Instance.IndicateMapName(placeName);

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