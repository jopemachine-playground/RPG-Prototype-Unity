// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// http://blog.naver.com/PostView.nhn?blogId=zeeoe&logNo=220778889291 참고
// 일반적인 상황에서 사용할 카메라 조정 스크립트

namespace UnityChanRPG
{
    public class PlayerCamera : MonoBehaviour
    {
        // 카메라 Rig의 회전 각도
        private float CameraRotateValueX;
        private float CameraRotateValueY;

        // 카메라 수동 회전 속도
        private const float X_SPEED = 180.0f;
        private const float Y_SPEED = 100.0f;

        // 카메라 수동 회전 y값 제한
        private const float Y_MinAngle = 10f;
        private const float Y_MaxAngle = 80f;

        // 카메라와 타겟 객체
        [NonSerialized]
        public Transform target;
        [NonSerialized]
        public Vector3 DistanceFromCharacter;

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            CameraRotateValueX = 175.0f;
            CameraRotateValueY = 10.0f;
            DistanceFromCharacter = new Vector3(0, 0.0f, -4f);
            transform.position = target.position + new Vector3(0, 10.0f, 10f);
        }

        private void Update()
        {
            // Q가 입력되면 캐릭터가 바라보는 방향으로 카메라를 맞춤.
            if (Input.GetKey(KeyCode.Q))
            {
                CameraRotateValueX = target.eulerAngles.y;
                CameraRotateValueY = target.eulerAngles.x;
            }

            CameraRotateValueX += Input.GetAxis("CamHorizontal") * X_SPEED * Time.smoothDeltaTime;
            CameraRotateValueY -= Input.GetAxis("CamVertical") * Y_SPEED * Time.smoothDeltaTime;

            // 보간을 통한 카메라의 위치와 각도 변경

            CameraRotateValueY = ClampAngle(CameraRotateValueY, Y_MinAngle, Y_MaxAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(CameraRotateValueY, CameraRotateValueX, 0), Time.smoothDeltaTime * 8.0f);
            transform.position = Vector3.Lerp(transform.position, transform.rotation * DistanceFromCharacter + target.position, Time.smoothDeltaTime * 15.0f);
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

    }
}