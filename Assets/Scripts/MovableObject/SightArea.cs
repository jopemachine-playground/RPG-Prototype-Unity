// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Issue : 
// @     벡터의 내적을 이용해 시야에 있는지를 판단함
// @ Ref URLs : 
// @     1. https://tenlie10.tistory.com/137
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace UnityChanRPG
{
    public class SightArea : MonoBehaviour
    {
        // 시야 거리, 각
        public float viewAngle;
        public float viewDistance;

        // SightArea 내 원하는 targetLayer 객체가 있다면 true를 반환. 
        public bool Detect(LayerMask targetLayer)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, viewDistance, targetLayer);

            for (int i = 0; i < targets.Length; i++)
            {
                Transform target = targets[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Dot(transform.forward, dirToTarget) > Mathf.Cos((viewAngle / 2) * Mathf.Deg2Rad))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Detect(Vector3 target)
        {
            Vector3 dirToTarget = (target - transform.position);

            Debug.Log(dirToTarget.magnitude);

            float angle = Vector3.Angle(target, transform.forward);

            Debug.Log(angle);

            if (angle < viewAngle)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}