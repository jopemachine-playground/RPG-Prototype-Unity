// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 주의 : 이 클래스는 Monobehavior를 상속받지 않아야 함
/// 그래서 Adapter 클래스를 따로 만들었다.
/// </summary>

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

        // monsterDropItems는 몬스터가 드롭할 수 있는 아이템들과 확률. dropItemProbAccum는 그 확률들을 누적해 더해 놓은 것으로, MonsterParser에서 계산.
        public List<MonsterDropItem> monsterDropItems = new List<MonsterDropItem>();
        public float[] dropItemProbAccum;

        public Monster getCopy()
        {
            return (Monster)this.MemberwiseClone();
        }
        

    }
}