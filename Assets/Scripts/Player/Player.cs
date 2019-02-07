using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    static public Player mInstance;

    public PlayerState state;

    #region Variables irrelevant level (including level)

    [SerializeField]
    public string Name;
    [SerializeField]
    public int Money;
    [SerializeField]
    public int currentHP;
    [SerializeField]
    public int currentMP;
    [SerializeField]
    public int Level;
    [SerializeField]
    public int ExperienceValue;

    #endregion

    #region Variables deciding by level in the first place

    public int MaxHP;
    public int MaxMP;
    public int FatalBlowValue;
    public int FatalBlowProb;
    public int AttackValue;
    public int DefenceValue;

    #endregion

    #region Variables except level

    public int MaxHPIncrement;
    public int MaxMPIncrement;
    public int FatalBlowValueIncrement;
    public int FatalBlowProbIncrement;
    public int AttackValueIncrement;
    public int DefenceValueIncrement;

    public float currentStamina;
    public float StaminaMax;
    public float[] skillCoefficient;

    #endregion

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

        StaminaMax = 100;
        currentStamina = StaminaMax;

        playerInfoUpdate();
    }

    // 항상 변하는 변수가 아닌, 아이템 착탈의, 레벨 업시에만 변하는 변수들이므로 성능을 위해 update에 넣지 않았다
    // 후에 update에 넣어야 한다면 아래 함수를 그대로 Update로 바꿀 것
    public void playerInfoUpdate()
    {
        MaxHP = LevelInfo.getMaxHP(Level) + MaxHPIncrement;
        MaxMP = LevelInfo.getMaxMP(Level) + MaxMPIncrement;
        FatalBlowValue = LevelInfo.getDefaultFatalBlowValue(Level) + FatalBlowValueIncrement;
        FatalBlowProb = LevelInfo.getDefalutFatalBlowProb(Level) + FatalBlowProbIncrement;
        AttackValue = LevelInfo.getDefaultAttackValue(Level) + AttackValueIncrement;
        DefenceValueIncrement = LevelInfo.getDefaultDefenceValue(Level) + DefenceValueIncrement;
    }

    #region Interaction Method With Monster

    // 플레이어의 방어력 속성들을 이용해 최종적인 데미지를 계산
    public Damage Damaged(Damage playerAtk)
    {
        int resultDamage;

        if (DefenceValue >= playerAtk.value)
        {
            resultDamage = 1;
        }
        else
        {
            resultDamage = playerAtk.value - DefenceValue;
        }

        if (currentHP - resultDamage >= 0)
        {
            currentHP -= resultDamage;
        }
        else
        {
            currentHP = 0;
        }

        return new Damage(resultDamage, playerAtk.IsFatalBlow);
    }

    // 데미지 계산공식은 처음부터 복잡하게 만들기보단, 일단 간단하게 해 봤음
    public Damage DecideAttackValue()
    {
        bool isFatalBlow;

        float minDamage = AttackValue - 50;
        float maxDamage = AttackValue + 50;

        float damage = Random.Range(minDamage, maxDamage);

        if (isFatalBlow = DecideFatalBlow())
        {
            damage *= (FatalBlowValue / 100);
        }

        return new Damage((int)(Mathf.Floor(damage)), isFatalBlow);
    }

    // 이번 공격이 치명타인지 결정
    public bool DecideFatalBlow()
    {
        int prob = Random.Range(0, 100);

        if (prob > FatalBlowProb)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    #endregion

}

