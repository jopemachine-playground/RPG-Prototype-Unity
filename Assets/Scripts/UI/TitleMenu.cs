// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;

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
                        DeleteObsoleteData();
                        SceneManager.LoadSceneAsync("Playing", LoadSceneMode.Additive);
                        SceneManager.LoadScene("Village", LoadSceneMode.Single);
                        break;

                    // 이어 하기 (DataLoad Scene을 사용하려 했으나, 우선 이어하기 데이터를 한 개로 고정하기로 하고,
                    // 아래 같이 구현했다.)
                    case 1:
                        SceneManager.LoadSceneAsync("Playing", LoadSceneMode.Additive);
                        SceneManager.LoadScene("Village", LoadSceneMode.Single);
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

        // 새로 게임을 시작할 때 호출해, 기존 데이터를 지움
        public void DeleteObsoleteData()
        {
            JsonData playerInfo = new JsonData();
            playerInfo["Name"] = "UnityChan";
            playerInfo["Money"] = 0;
            playerInfo["currentHP"] = 40;
            playerInfo["currentMP"] = 25;
            playerInfo["Level"] = 1;
            playerInfo["Experience"] = 0;

            File.WriteAllText(Application.dataPath + "/Custom/Resources/PlayerInfoData.json", "[" + playerInfo.ToJson() + "]");
            clearFlagData();
        }

        public void clearFlagData() {
            // Dungeon Cleared Flag를 초기화 (TRUE를 모두 false로 바꿈)
            string flagJsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/DungeonClearedFlagData.json");
            JsonData flagData = JsonMapper.ToObject(flagJsonString);

            for (int i = 0; i < flagData.Count; i++) {
                if (string.Equals(flagData[i]["FlagData"].ToString(), "TRUE")) {
                    flagData[i]["FlagData"] = "FALSE";
                }
            }

            string flagDataStr = "";

            for (int j = 0; j < flagData.Count; j++)
            {
                flagDataStr += "{\"FlagName\":";
                flagDataStr += "\"" + flagData[j]["FlagName"] + "\",";
                flagDataStr += "\"FlagData\":\"" + flagData[j]["FlagData"] + "\"";
                flagDataStr += "}";

                if (j != flagData.Count - 1)
                {
                    flagDataStr += ",";
                }

            }

            File.WriteAllText(Application.dataPath + "/Custom/Resources/DungeonClearedFlagData.json", "[" + flagDataStr + "]");

        }

    }
}