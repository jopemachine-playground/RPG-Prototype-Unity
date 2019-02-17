using System;
using UnityEngine;

namespace UnityChanRPG
{
    // Monster가 Monobehavior를 상속할 수 없으므로 Adapter를 만듬
    public class MonsterAdapter : MonoBehaviour
    {
        public Monster monster;

        private int ID;

        public void Start()
        {
            ID = Int32.Parse(gameObject.name);
            monster = MonsterPool.mInstance.getMonsterByID(ID);
            monster.monsterStatus = GetComponent<Status>();
        }
    }
}
