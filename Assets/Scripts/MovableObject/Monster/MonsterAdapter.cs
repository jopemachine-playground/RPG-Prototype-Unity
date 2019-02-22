using System;
using UnityEngine;

/// <summary>
/// Monster가 Monobehavior를 상속할 수 없으므로 Adapter 클래스를 만들었다. 
/// 몬스터 컴포넌트에 Monster.cs를 추가하기 위해 만든 클래스 일 뿐이지만, Status의 경우 플레이어, 몬스터 등에 모두 들어갈 수 있기 때문에
/// 스스로를 초기화하지 않으므로, MonsterAdapter가 초기화한다.
/// </summary>

namespace UnityChanRPG
{
    public class MonsterAdapter : MonoBehaviour
    {
        public Monster monster;

        public void Awake()
        {
            monster.monsterStatus = GetComponent<Status>();
        }

    }
}
