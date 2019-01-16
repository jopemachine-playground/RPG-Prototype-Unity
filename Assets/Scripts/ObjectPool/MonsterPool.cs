using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

class MonsterPool : MonoBehaviour
{

    public static MonsterPool mInstance;

    public List<Monster> entireMonsterList = new List<Monster>();

    public List<GameObject> objModel;

    private void Awake()
    {
        if (mInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            mInstance = this;
        }
        Load();
    }

    public void Load()
    {
        StartCoroutine("LoadCoroutine");
    }

    IEnumerator LoadCoroutine()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/MonsterData.json");

        JsonData monsterData = JsonMapper.ToObject(JsonString);

        ParsingJsonMonster(monsterData);

        yield return null;
    }

    private void ParsingJsonMonster(JsonData name)
    {
        for (int i = 0; i < name.Count; i++)
        {
            entireMonsterList[i].ID = (int)(name[i]["ID"]);
            entireMonsterList[i].Name = (name[i]["Name"]).ToString();
            entireMonsterList[i].Description = (name[i]["Description"]).ToString();
            entireMonsterList[i].Type = (MonsterType)((int)(name[i]["MonsterType"]));
            entireMonsterList[i].MaxHP = (int)(name[i]["MaxHP"]);
            entireMonsterList[i].currentHP = entireMonsterList[i].MaxHP;
            entireMonsterList[i].AttackValue = (int)(name[i]["AttackValue"]);
            entireMonsterList[i].DefenceValue = (int)(name[i]["DefenceValue"]);
            entireMonsterList[i].ExperienceValue = (int)(name[i]["ExperienceValue"]);
            entireMonsterList[i].Speed = (int)(name[i]["Speed"]);
            entireMonsterList[i].MonsterModel = objModel[i];
        }

    }

    public void Spawn()
    {

    }

}

