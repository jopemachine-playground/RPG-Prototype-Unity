using System;
using UnityEngine;

namespace UnityChanRPG
{
    // Monster가 Monobehavior를 상속할 수 없으므로 Adapter를 만듬
    public class MonsterAdapter : MonoBehaviour
    {
        public Monster monster;

        public void Awake()
        {
            monster.monsterStatus = GetComponent<Status>();
        }

    }
}
