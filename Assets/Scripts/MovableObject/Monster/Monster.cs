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
    public int ID;
    public string Name;
    public string Description;
    public int MaxHP;
    public int ExperienceValue;
    public int Speed;
    public MonsterType Type;
    public GameObject MonsterModel;

    public Status monsterStatus;

    // spawn Area에 생성될 수 있는 최대 몬스터 갯수.
    // spawn Area에 따라 달라지므로, Spawn Manager에서 지정.
    public int MaxGenerateNumber;


    public Monster getCopy()
    {
        return (Monster)this.MemberwiseClone();
    }

}
