using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    public class TitleMenu : Scene
    {
        private const int MENU_NUMBER = 4;

        public Button[] mButton;
        public Text[] mText;
        private int mSelected = 0;

        private void Start()
        {
            base.ScreenCoverInit();
            ChangeTextColor(MENU_NUMBER);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (mSelected)
                {
                    // 새로 시작
                    case 0:
                        SceneManager.LoadSceneAsync("Playing", LoadSceneMode.Additive);
                        MoveScene("MyHouse");
                        break;

                    // 이어 하기
                    case 1:
                        Debug.Log("옵션");
                        break;

                    // 옵션
                    case 2:
                        break;

                    // 나가기
                    case 3:
                        Application.Quit();
                        break;

                    default:
                        Debug.Assert(false, "Error - Wrong selected value");
                        break;
                }

                ChangeTextColor(MENU_NUMBER);
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (mSelected <= 0)
                {
                    mSelected = MENU_NUMBER - 1;
                }
                else
                {
                    mSelected--;
                }

                ChangeTextColor(MENU_NUMBER);
            }

            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (mSelected >= MENU_NUMBER - 1)
                {
                    mSelected = 0;
                }
                else
                {
                    mSelected++;
                }

                ChangeTextColor(MENU_NUMBER);
            }

        }

        private void ChangeTextColor(int menuNumber)
        {
            for (int i = 0; i < menuNumber; i++)
            {
                mText[i].color = Color.black;
            }

            mText[mSelected].color = Color.cyan;
        }

        public override void MoveCharacter()
        {

        }
    }
}