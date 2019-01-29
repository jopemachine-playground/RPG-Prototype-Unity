using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using LitJson;
using System.IO;

// 플레이어의 정보 중 레벨에 의해 가장 먼저 결정되는 속성들을 관리함.

public class LevelInfo: MonoBehaviour
{
    static public LevelInfo mInstance;

    public int PLAYER_MAX_LEVEL;

    // 레벨 당 최대 HP, MP, 다음 레벨까지의 경험치량
    public static int[] MAX_HP;
    public static int[] MAX_MP;
    public static int[] MAX_ExperienceValue;

    // 레벨 당 기본 공격, 방어력, 치명타 데미지, 치명타 확률
    public static int[] DefaultAttackValue;
    public static int[] DefaultDefenceValue;
    public static int[] DefalutFatalBlowProb;
    public static int[] DefaultFatalBlowValue;

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
        DefaultAttackValue = new int[PLAYER_MAX_LEVEL];
        DefaultDefenceValue = new int[PLAYER_MAX_LEVEL];
        DefalutFatalBlowProb = new int[PLAYER_MAX_LEVEL];
        DefaultFatalBlowValue = new int[PLAYER_MAX_LEVEL];

        for (int i = 0; i < PLAYER_MAX_LEVEL; i++)
        {
            MAX_HP[i] = (int)(name[i]["MaxHP"]);
            MAX_MP[i] = (int)(name[i]["MaxMP"]);
            MAX_ExperienceValue[i] = (int)(name[i]["MaxExperienceValue"]);
            DefaultAttackValue[i] = (int)(name[i]["DefaultAttackValue"]);
            DefaultDefenceValue[i] = (int)(name[i]["DefaultDefenceValue"]); 
            DefalutFatalBlowProb[i] = (int)(name[i]["DefalutFatalBlowProb"]);
            DefaultFatalBlowValue[i] = (int)(name[i]["DefaultFatalBlowValue"]);
        }
    }

    #region Getter

    // Level은 1부터, 배열은 0부터 시작함

    public static int getDefaultAttackValue(int _Level)
    {
        return DefaultAttackValue[_Level - 1];
    }

    public static int getDefaultDefenceValue(int _Level)
    {
        return DefaultDefenceValue[_Level - 1];
    }

    public static int getDefalutFatalBlowProb(int _Level)
    {
        return DefalutFatalBlowProb[_Level - 1];
    }

    public static int getDefaultFatalBlowValue(int _Level)
    {
        return DefaultFatalBlowValue[_Level - 1];
    }

    public static int getMaxHP(int _Level)
    {
        return MAX_HP[_Level - 1];
    }

    public static int getMaxMP(int _Level)
    {
        return MAX_MP[_Level - 1];
    }

    public static int getMaxExp(int _Level)
    {
        return MAX_ExperienceValue[_Level - 1];
    }

    #endregion

}

