using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

namespace UnityChanRPG
{
    public class PlayerSkillManager : MonoBehaviour
    {
        static public PlayerSkillManager Instance;

        [SerializeField]
        public static List<PlayerSkill> skills;

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
            string JsonString_playerSkills = File.ReadAllText(Application.dataPath + "/Custom/Resources/PlayerSkill.json");

            JsonData playerSkillData = JsonMapper.ToObject(JsonString_playerSkills);

            Debug.Assert(playerSkillData != null, "Player Skill Data read fail");

            ParsingJsonPlayerSkillInfo(playerSkillData);

            yield return null;
        }

        private void ParsingJsonPlayerSkillInfo(JsonData playerSkillInfoData)
        {
            skills = new List<PlayerSkill>(playerSkillInfoData.Count);

            for (int i = 0; i < playerSkillInfoData.Count; i++)
            {
                PlayerSkill skill = new PlayerSkill();

                skill.ID = (int)(playerSkillInfoData[i]["ID"]);
                skill.Name = (playerSkillInfoData[i]["Name"]).ToString();
                skill.Description = (playerSkillInfoData[i]["Desc"]).ToString();
                skill.AttackValueCoefficient = ((int)(playerSkillInfoData[i]["Coefficient"]));
                skill.AnimationClipName = (playerSkillInfoData[i]["Animation"]).ToString();
                skill.EmittingParticleID = (int)(playerSkillInfoData[i]["EmittingParticleID"]);

                skills.Add(skill);
            }
        }

        public static PlayerSkill GetSkill(string AnimationName)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].AnimationClipName == AnimationName)
                {
                    return skills[i];
                }
            }

            Debug.Assert(false, "Wrong Skill Name");

            return null;
        }
    }
}
