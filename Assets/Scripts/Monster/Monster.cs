using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 주의 : Monobehavior를 상속받지 않아야 함
 */

[System.Serializable]
public class Monster
{
    public int currentHP;

    public int ID;
    public string Name;
    public string Description;
    public int MaxHP;
    public int AttackValue;
    public int DefenceValue;
    public int ExperienceValue;
    public int Speed;
    public MonsterType Type;
    public GameObject MonsterModel;

    // spawn Area에 생성될 수 있는 최대 몬스터 갯수.
    // spawn Area에 따라 달라지므로, Spawn Manager에서 지정.
    public int MaxGenerateNumber;

    // 데미지 계산공식은 처음부터 복잡하게 만들기보단, 일단 간단하게 해 봤음
    public int DecideAttackValue()
    {
        float minDamage = AttackValue - 50;
        float maxDamage = AttackValue + 50;

        float damage = Random.Range(minDamage, maxDamage);

        if (DecideFatalBlow())
        {
            damage *= (20 / 100);
        }

        return (int)(Mathf.Floor(damage));
    }

    // 이번 공격이 치명타인지 결정
    public bool DecideFatalBlow()
    {
        int prob = Random.Range(0, 100);

        if (prob > 20)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public int Damaged(int playerAtk)
    {
        int resultDamage;

        if (DefenceValue >= playerAtk)
        {
            resultDamage = 1;
        }
        else
        {
            resultDamage = playerAtk - DefenceValue;
        }

        currentHP -= resultDamage;

        return resultDamage;
    }

    public Monster getCopy()
    {
        return (Monster)this.MemberwiseClone();
    }

}
