using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// http://blog.naver.com/PostView.nhn?blogId=zeeoe&logNo=220778889291

public class PlayerCamera : MonoBehaviour
{
    // 카메라 Rig의 회전 각도
    private float mCameraRotateValueX;
    private float mCameraRotateValueY;

    // 카메라 수동 회전 속도
    private const float X_SPEED = 180.0f;
    private const float Y_SPEED = 100.0f;

    // 카메라 수동 회전 y값 제한
    private const float Y_MinAngle = 10f;
    private const float Y_MaxAngle = 80f;

    // 카메라와 타겟 객체
    public Transform mTarget;
    public Camera mCam;

    public Vector3 DistanceFromCharacter;

    private void Awake()
    {
        //mTarget = GetComponent<Transform>();
        mCameraRotateValueX = 175.0f;
        mCameraRotateValueY = 10.0f;
        DistanceFromCharacter = new Vector3(0, 0.0f, -4f);
    }

    private void Update()
    {

        // Q가 입력되면 캐릭터가 바라보는 방향으로 카메라를 맞춤.
        if (Input.GetKey(KeyCode.Q))
        {
            mCameraRotateValueX = mTarget.eulerAngles.y;
            mCameraRotateValueY = mTarget.eulerAngles.x;
        }

        // 카메라의 회전각도
        mCameraRotateValueX += Input.GetAxis("CamHorizontal") * X_SPEED * Time.smoothDeltaTime;
        mCameraRotateValueY -= Input.GetAxis("CamVertical") * Y_SPEED * Time.smoothDeltaTime;

        //앵글값 정하기
        //y값의 Min과 MaX 없애면 y값이 360도 계속 돎
        //x값은 계속 돌고 y값만 제한
        mCameraRotateValueY = ClampAngle(mCameraRotateValueY, Y_MinAngle, Y_MaxAngle);

        // 카메라의 위치와 각도 변경
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(mCameraRotateValueY, mCameraRotateValueX, 0), Time.smoothDeltaTime * 8.0f);
        transform.position = Vector3.Lerp(transform.position, transform.rotation * DistanceFromCharacter + mTarget.position, Time.smoothDeltaTime * 15.0f);

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
