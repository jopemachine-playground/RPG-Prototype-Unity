using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// http://blog.naver.com/PostView.nhn?blogId=zeeoe&logNo=220778889291

// Village에서 사용하는 카메라 컨트롤 스크립트

public class PlayerCamera2 : MonoBehaviour
{
    // 카메라와 타겟 객체
    [NonSerialized]
    public Transform target;
    [NonSerialized]
    public Vector3 DistanceFromCharacterY;
    [NonSerialized]
    public Vector3 DistanceFromCharacterZ;

    // 유니티에서 조정. 각각 카메라와 캐릭터의 거리, 카메라가 위에서 바라보는 각도를 나타냄
    public float distanceValue;
    public float rotation;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        DistanceFromCharacterZ = new Vector3(0, 0.0f, -distanceValue);
        // y값은 Tan를 이용해 초기화했음
        DistanceFromCharacterY = new Vector3(0, Mathf.Tan(rotation) * distanceValue, 0);
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, DistanceFromCharacterY + DistanceFromCharacterZ + target.position, 6f * Time.deltaTime);
    }

}
