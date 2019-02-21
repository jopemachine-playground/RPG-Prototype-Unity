using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 주의 : 이 클래스는 Monobehavior를 상속받지 않아야 함
 그래서 Adapter 클래스를 따로 만들었다.
 */

namespace UnityChanRPG
{
    [Serializable]
    public class Monster
    {
        public int ID;
        public string Name;
        public string Description;
        public int MaxHP;
        public int MaxMP;
        public int ExperienceValue;
        public int Speed;
        public MonsterType Type;
        public GameObject MonsterModel;
        public Status monsterStatus;
        public List<MonsterDropItem> monsterDropItems = new List<MonsterDropItem>();

        public Monster getCopy()
        {
            return (Monster)this.MemberwiseClone();
        }
        

    }
}