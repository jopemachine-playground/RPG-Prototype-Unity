using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stardard Asset의 ThirdPersonControl 수정

public class PlayerControl : MonoBehaviour
{

    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private Vector3 m_GroundNormal;
    private Vector3 m_CapsuleCenter;

    private const float MOVING_TURN_SPEED = 360;
    private const float STATIONARY_TURN_SPEED = 180;

    private float GRAVITY_MULTIPLIER = 2f;
    private const float RUN_CYCLE_LEG_OFFSET = 0.2f;
    // specific to the character in sample assets, will need to be modified to work with others
    private const float ANIM_SPEED_MULTIPLIER = 1f;

    private float m_OrigGroundCheckDistance;
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private float m_CapsuleHeight;
    private float m_GroundCheckDistance;

    private bool mb_IsGrounded;
    private bool mb_Jump;
    private bool mb_IsAttacking;
    private bool mb_IsAirAttacking;
    private bool mb_IsDashAttack;

    public Rigidbody m_Rigidbody;
    public Animator m_Animator;
    public CapsuleCollider m_Capsule;
    public float m_JumpPower;
    public float m_MoveSpeed;

    void Start()
    {

        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.Assert(false, "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }

        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_GroundCheckDistance = 0.1f;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (mb_Jump == false)
        {
            mb_Jump = Input.GetButtonDown("Jump");
        }

        if (mb_IsAttacking == false &&
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            Input.GetButtonDown("Attack"))
        {
            mb_IsAttacking = true;
        }

        Debug.Assert(m_Cam != null, "Error: m_Cam == null");

        // 기본동작은 걷기
        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = (v * m_CamForward + h * m_Cam.right) / 2;

        // 지상에서 왼쪽 쉬프트 버튼을 누르면 달리기
        if (Input.GetKey(KeyCode.LeftShift) && mb_IsGrounded)
        {
            m_Move *= 2;
        }

        // Attack 중이라면 움직일 수 없음
        if (m_Animator.GetInteger("AttackState") == 0 &&
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") |
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne"))
        {
            Move(m_Move, mb_Jump);
        }

        mb_IsAttacking = false;
        mb_IsAirAttacking = false;
        mb_IsDashAttack = false;
        mb_Jump = false;
    }

    public void Move(Vector3 move, bool jump)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (mb_IsGrounded == true)
        {
            HandleGroundedMovement(jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }



    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.smoothDeltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.smoothDeltaTime);
        m_Animator.SetBool("OnGround", mb_IsGrounded);
        m_Animator.SetBool("IsAirAttack", mb_IsAirAttacking);
        m_Animator.SetBool("IsJump", mb_Jump);

        // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
        if (mb_IsAttacking == true &&
            m_Rigidbody.velocity.magnitude > 1.0f &&
            mb_IsGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            mb_IsDashAttack = true;
        }

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1로 활성화.
        if (mb_IsAttacking == true &&
            mb_IsGrounded &&
            mb_IsDashAttack != true)
        {
            m_Rigidbody.velocity = Vector3.zero;
            m_Animator.SetInteger("AttackState", 1);
        }

        m_Animator.SetBool("IsDashAttack", mb_IsDashAttack);

    }


    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * GRAVITY_MULTIPLIER) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;

        // 공중에서의 키 입력에 따른 캐릭터 조정
        m_Rigidbody.velocity =
           new Vector3(0.65f * m_MoveSpeed * m_Move.x,
           m_Rigidbody.velocity.y,
           0.65f * m_MoveSpeed * m_Move.z);

        // 공중에서 공격 시 더 빨리 떨어지게 조정
        // 특정 높이 이상에서만 공중 공격이 가능하도록 조정할 것
        if (Input.GetButtonDown("Attack"))
        {
            mb_IsAirAttacking = true;
            m_Rigidbody.AddForce(10 * Physics.gravity);
        }
    }


    void HandleGroundedMovement(bool jump)
    {
        // 점프 
        if (jump == true &&
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            mb_IsGrounded = false;

            m_GroundCheckDistance = 0.1f;
            return;
        }

        // 지상에서의 키 입력에 따른 캐릭터 조정

        m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, new Vector3(m_MoveSpeed * m_Move.x, m_Rigidbody.velocity.y, m_MoveSpeed * m_Move.z), Time.deltaTime * 8f);
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        // 10은 Floor Layer이다. 따라서, Ray는 Floor Layer만을 감지한다.
        // https://docs.unity3d.com/kr/530/Manual/Layers.html 참고

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, 1 << 10))
        {
            m_GroundNormal = hitInfo.normal;
            mb_IsGrounded = true;
        }
        else
        {
            mb_IsGrounded = false;
            m_GroundNormal = Vector3.up;
        }
    }
}


