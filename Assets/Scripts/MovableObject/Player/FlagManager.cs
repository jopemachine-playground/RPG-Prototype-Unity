// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-07-10, 12:24:22
// ==============================+===============================================================

using UnityEngine;
using LitJson;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace UnityChanRPG
{
    // 플레이어의 던전 클리어 상태, 퀘스트 진행 상태 등의 플래그를 관리하는 싱글톤 클래스들
    // 각 컴포넌트들에 FlagManager를 달아서 enum을 달아 사용한다.
    class FlagManager : MonoBehaviour
    {
        // Key 값을 던전 이름으로, Value 값을 클리어 여부 String으로 한다
        public static FlagManager dungeonCleared;

        public static FlagManager challengeHomework;

        public static FlagManager questCleared;

        [SerializeField]
        public FlagType type;

        [SerializeField]
        public static Hashtable flagList;

        public enum FlagType {
            // 던전 클리어 여부
            DungeonCleared,
            // 도전과제 클리어 여부
            ChallengeHomework,
            // 퀘스트 클리어 여부
            QuestCleared
        }

        #region Data Parsing and Load

        IEnumerator dataload_DungeonCleared()
        {
            string flagJsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/DungeonClearedFlagData.json");

            JsonData flagData = JsonMapper.ToObject(flagJsonString);

            Debug.Assert(flagData != null, "flag Data == null");

            ParsingFlagData(flagData);

            yield return null;
        }

        IEnumerator dataload_ChallengeHomework()
        {
            yield return null;
        }

        IEnumerator dataload_QuestCleared()
        {
            yield return null;
        }

        private void ParsingFlagData(JsonData flagData)
        {
            for (int i = 0; i < flagData.Count; i++)
            {
                flagList.Add(flagData[i]["FlagName"].ToString(), flagData[i]["FlagData"].ToString());
            }
        }

        #endregion

        // 각 싱글톤 객체 초기화
        private void Awake()
        {
            flagList = new Hashtable();

            switch (type)
            {
                case FlagType.DungeonCleared:
                    dungeonCleared = this;
                    StartCoroutine("dataload_DungeonCleared");
                    break;
                case FlagType.ChallengeHomework:
                    challengeHomework = this;
                    StartCoroutine("dataload_ChallengeHomework");
                    break;
                case FlagType.QuestCleared:
                    questCleared = this;
                    StartCoroutine("dataload_QuestCleared");
                    break;
            }

        }

        public bool GetFlag(string key) {
            return string.Equals(flagList[key], "TRUE") ? true : false; 
        }

        public void ToggleFlag(string key)
        {
            if (string.Equals(flagList[key], "TRUE")) {
                flagList[key] = "FALSE";
            }
            else {
                flagList[key] = "TRUE";
            }
        }

        // LitJson에 적절한 기능이 없는 것 같아 만들어 사용했다
        public void saveFlagData(string savePath) {

            string flagData = "";
            int i = 0;
            foreach (string key in flagList.Keys) {
                flagData += "{\"FlagName\":";
                flagData += "\"" + key + "\",";
                flagData += "\"FlagData\":\"" + flagList[key] + "\"";
                flagData += "}";

                i++;
                if(i != flagList.Count)
                {
                    flagData += ",";
                }
            }

            File.WriteAllText(Application.dataPath + savePath, "[" + flagData + "]");
        }
    }
}
