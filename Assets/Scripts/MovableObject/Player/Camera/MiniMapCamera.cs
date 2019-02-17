using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MiniMap 에서 사용하는 카메라 컨트롤 스크립트
namespace UnityChanRPG
{
    public class MiniMapCamera : MonoBehaviour
    {
        // 카메라와 타겟 객체
        [NonSerialized]
        public Transform target;
        [NonSerialized]
        public Vector3 DistanceFromCharacterY;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            DistanceFromCharacterY = new Vector3(0f, 10f, 0f);
        }

        private void Update()
        {
            transform.position = DistanceFromCharacterY + target.position;
        }

    }
}
