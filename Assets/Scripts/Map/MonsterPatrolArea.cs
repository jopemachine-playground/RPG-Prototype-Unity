using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 몬스터가 공중도 함께 Patrol 할 수 있게 하려면 y좌표도 넣으면 된다.
// 그 경우 NavMesh의 높이를 충분히 늘려야 함에 주의

namespace UnityChanRPG
{
    public class MonsterPatrolArea : MonoBehaviour
    {
        public GameObject parent;
        private List<Transform> patrolPoints;

        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;

        private void Start()
        {
            patrolPoints = new List<Transform>(parent.transform.childCount);

            for (int i = 0; i < parent.transform.childCount; i++)
            {
                patrolPoints.Add(parent.transform.GetChild(i).GetComponent<Transform>());
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

