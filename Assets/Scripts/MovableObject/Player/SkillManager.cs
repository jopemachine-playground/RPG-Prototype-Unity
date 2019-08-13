// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

namespace UnityChanRPG
{
    public class SkillManager : MonoBehaviour
    {
        static public SkillManager Instance;

        [SerializeField]
        public static List<Skill> skills;

        // 더 빠르게 skills 인덱스에 접근하기 위해 읽어오는 값
        public static List<int> indexForUserID = new List<int>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }

            StartCoroutine("LoadCoroutine");
        }

        IEnumerator LoadCoroutine()
        {
            string JsonString_Skills = File.ReadAllText(Application.dataPath + "/Custom/Resources/Skills.json");

            JsonData SkillData = JsonMapper.ToObject(JsonString_Skills);

            Debug.Assert(SkillData != null, "Skill Data read fail");

            ParsingJsonSkillInfo(SkillData);

            yield return null;
        }

        private void ParsingJsonSkillInfo(JsonData playerSkillInfoData)
        {
            skills = new List<Skill>(playerSkillInfoData.Count);

            for (int i = 0; i < playerSkillInfoData.Count; i++)
            {
                skills.Add(new Skill(
                    (int)(playerSkillInfoData[i]["Skill_ID"]),
                    (int)(playerSkillInfoData[i]["User_ID"]),
                    (playerSkillInfoData[i]["Desc"]).ToString(),
                    (playerSkillInfoData[i]["Name"]).ToString(),
                    ((int)(playerSkillInfoData[i]["Coefficient"])),
                    (playerSkillInfoData[i]["Animation"]).ToString(),
                    (int)(playerSkillInfoData[i]["EmittingParticleID"])
                    ));

                indexForUserID.Add((int)(playerSkillInfoData[i]["(Record Value)"]));
                
            }
        }

        // 한 개의 USER의 두 개 이상의 애니메이션 클립이 같은 이름을 갖지 않는다고 가정함.
        // Skill_ID 대신 Animation Name이란 인자를 넣은 이유는, 이렇게 하는 편이 가독성이 높을 거라 생각했기 때문.
        // 플레이어의 ID는 0 에 해당.
        public static Skill GetSkill(int USER_ID, string AnimationName)
        {
            for (int i = 0; i < skills.Count; i += indexForUserID[i])
            {
                if (skills[i].USER_ID == USER_ID)
                {
                    for (int j = 0; j < skills.Count; j++)
                    {
                        if (skills[j].AnimationClipName == AnimationName)
                        {
                            return skills[j];
                        }
                    }
                }
            }

            Debug.Assert(false, "Wrong Skill Name");

            return null;
        }
    }
}
