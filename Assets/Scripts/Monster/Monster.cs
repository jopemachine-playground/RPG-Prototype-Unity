using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 주의 : 이 클래스는 Monobehavior를 상속받지 않아야 함
 그래서 Adapter 클래스를 따로 만들었다.
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

    // 몬스터의 공격력 속성들을 이용해 데미지를 계산하는 함수
    // 데미지 계산공식은 처음부터 복잡하게 만들기보단, 일단 간단하게 해 봤음
    public Damage DecideAttackValue()
    {
        bool isFatalBlow;

        float minDamage = AttackValue - 50;
        float maxDamage = AttackValue + 50;

        float damage = Random.Range(minDamage, maxDamage);

        if (isFatalBlow = DecideFatalBlow())
        {
            damage *= (120 / 100);
        }

        return new Damage((int)(Mathf.Floor(damage)), isFatalBlow);
    }

    // 몬스터가 공격할 때 이번 공격이 치명타인지 결정
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

    // 몬스터의 방어력 속성들을 이용해 최종적인 데미지를 계산
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

        currentHP -= resultDamage;

        return new Damage(resultDamage, playerAtk.IsFatalBlow);
    }

    public Monster getCopy()
    {
        return (Monster)this.MemberwiseClone();
    }

}
