using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// SpawnPoint는 몬스터와 아이템을 랜덤으로 생성할 장소 역할을 함. Monster를 생성하는 경우에만 patrolArea를 가져와 사용한다.
/// SpawnPoint를 추가한 게임오브젝트에 MonsterPatrolArea 이외의 자식컴포넌트를 추가하지 말 것
/// </summary>

namespace UnityChanRPG
{
    [Serializable]
    public class SpawnPoint : MonoBehaviour
    {
        private Transform point;

        public List<SpawnObject> SpawnObjects = new List<SpawnObject>();

        // 아이템 역시 SpawnPoint에서 생성되지만, 이 경우엔 MonsterPatrolArea를 사용하지 않는다.
        [NonSerialized]
        public MonsterPatrolArea patrolArea;

        private void Awake()
        {
            point = this.GetComponent<Transform>();

            if (transform.childCount > 0)
            {
                patrolArea = GetComponentInChildren<MonsterPatrolArea>();
            }
        }

        [Serializable]
        public class SpawnObject
        {
            public int ID;
            public float SpawnProbablity;
            // 오브젝트 풀링으로 미리 생성해 놓을 디폴트 오브젝트 갯수. 유니티 에디터에서 셋팅
            // 몬스터 종류에 따라, 미리 생성해 놓는 메리트가 적은 (보스 몬스터, 1개만 존재하는 퀘스트 아이템 등) 경우 1로 셋팅.
            public int PoolingValue;

        }
    }
}