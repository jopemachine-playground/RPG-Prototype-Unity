using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using LitJson;
using System.IO;

public class LevelInfo: MonoBehaviour
{
    static public LevelInfo mInstance;

    public int PLAYER_MAX_LEVEL;
    public static int[] MAX_HP;
    public static int[] MAX_MP;
    public static int[] MAX_ExperienceValue;

    public void Initialize()
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

        StartCoroutine("LoadCoroutine");
    }

    IEnumerator LoadCoroutine()
    {
        string JsonString_item = File.ReadAllText(Application.dataPath + "/Custom/Resources/LevelInfoData.json");

        JsonData playerInfoData = JsonMapper.ToObject(JsonString_item);

        Debug.Assert(playerInfoData != null, "playerInfo Data == null");

        ParsingJsonLevelInfo(playerInfoData);

        yield return null;
    }

    private void ParsingJsonLevelInfo(JsonData name)
    {
        PLAYER_MAX_LEVEL = name.Count;
        MAX_HP = new int[PLAYER_MAX_LEVEL];
        MAX_MP = new int[PLAYER_MAX_LEVEL];
        MAX_ExperienceValue = new int[PLAYER_MAX_LEVEL];

        for (int i = 0; i < PLAYER_MAX_LEVEL; i++)
        {
            MAX_HP[i] = (int)(name[i]["MaxHP"]);
            MAX_MP[i] = (int)(name[i]["MaxMP"]);
            MAX_ExperienceValue[i] = (int)(name[i]["MaxExperienceValue"]);
        }
    }

    public static int getMAXHP(int _Level)
    {
        // Level은 1부터, 배열은 0부터 시작함
        return MAX_HP[_Level - 1];
    }

    public static int getMAXMP(int _Level)
    {
        // Level은 1부터, 배열은 0부터 시작함
        return MAX_MP[_Level - 1];
    }

    public static int getMaxExp(int _Level)
    {
        // Level은 1부터, 배열은 0부터 시작함
        return MAX_ExperienceValue[_Level - 1];
    }


}

