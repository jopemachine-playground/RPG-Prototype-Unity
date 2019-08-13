// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// patrol 할 영역을 유니티 에디터에서 미리 정해놓는다. 영역의 각 꼭지점이 되는 부분에 빈 오브젝트를 놓고, 부모 오브젝트에
/// MonsterPatrolArea를 추가하도록 했음. 몬스터가 이동할 수 없는 영역에 꼭지점 (자식오브젝트)을 두면 몬스터가 이동하지 않는 것처럼 보이는 버그가 생길 수 있다.
/// MonsterPatrolArea는 SpawnPoint의 자식으로 들어가, 종속된다. (SpawnPoint가 MonsterPatrolArea를 결정하도록 함)
/// </summary>

// 몬스터가 공중도 함께 Patrol 할 수 있게 하려면 y좌표도 넣으면 된다.
// 그 경우 NavMesh의 높이를 충분히 늘려야 함에 주의

namespace UnityChanRPG
{
    public class MonsterPatrolArea : MonoBehaviour
    {
        [NonSerialized]
        private List<Transform> patrolPoints;

        [NonSerialized]
        public float minX;
        [NonSerialized]
        public float maxX;
        [NonSerialized]
        public float minZ;
        [NonSerialized]
        public float maxZ;

        private void Start()
        {
            patrolPoints = new List<Transform>(transform.childCount);

            for (int i = 0; i < transform.childCount; i++)
            {
                patrolPoints.Add(transform.GetChild(i).GetComponent<Transform>());
            }

            minX = patrolPoints[0].position.x; maxX = minX;
            minZ = patrolPoints[0].position.z; maxZ = minZ;

            for (int i = 0; i < patrolPoints.Count; i++)
            {
                if (minX > patrolPoints[i].position.x)
                {
                    minX = patrolPoints[i].position.x;
                }
                if (maxX < patrolPoints[i].position.x)
                {
                    maxX = patrolPoints[i].position.x;
                }
                if (minZ > patrolPoints[i].position.z)
                {
                    minZ = patrolPoints[i].position.z;
                }
                if (maxZ < patrolPoints[i].position.z)
                {
                    maxZ = patrolPoints[i].position.z;
                }

            }


        }

    }
}

