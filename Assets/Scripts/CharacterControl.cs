using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stardard Asset의 ThirdPersonControl 수정

public class CharacterControl : MonoBehaviour
{

    private Transform m_Cam;
    private Vector3 m_CamForward;
    private Vector3 m_Move;
    private Vector3 m_GroundNormal;
    private Vector3 m_CapsuleCenter;

    private const float MOVING_TURN_SPEED = 360;
    private const float STATIONARY_TURN_SPEED = 180;

    private const float GRAVITY_MULTIPLIER = 2f;
    private const float RUN_CYCLE_LEG_OFFSET = 0.2f;
    // specific to the character in sample assets, will need to be modified to work with others
    private const float ANIM_SPEED_MULTIPLIER = 1f;
    private const float K_HALF = 0.5f;

    private float m_OrigGroundCheckDistance;
    private float m_TurnAmount;
    private float m_ForwardAmount;
    private float m_CapsuleHeight;
    private float m_GroundCheckDistance;

    private bool mb_Crouching;
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

    private void Update()
    {
        if (mb_Jump == false)
        {
            mb_Jump = Input.GetButtonDown("Jump");
        }

        if (mb_IsAttacking == false && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") && Input.GetButtonDown("Attack"))
        {
            mb_IsAttacking = true;
        }
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        Debug.Assert(m_Cam != null, "Error: m_Cam == null");

        // 기본동작은 걷기
        m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        m_Move = (v * m_CamForward + h * m_Cam.right) / 2;

        // 지상에서 왼쪽 쉬프트 버튼을 누르면 달리기
        if (Input.GetKey(KeyCode.LeftShift) && mb_IsGrounded) m_Move *= 2;

        // Attack 중이라면 움직일 수 없음
        if (m_Animator.GetInteger("AttackState") == 0) Move(m_Move, crouch, mb_Jump);


        mb_IsAttacking = false;
        mb_Jump = false;
        mb_IsAirAttacking = false;
        mb_IsDashAttack = false;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
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
            HandleGroundedMovement(crouch, jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }


    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (mb_IsGrounded && crouch)
        {
            if (mb_Crouching) return;
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
            mb_Crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * K_HALF, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * K_HALF;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * K_HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                mb_Crouching = true;
                return;
            }
            m_Capsule.height = m_CapsuleHeight;
            m_Capsule.center = m_CapsuleCenter;
            mb_Crouching = false;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (mb_Crouching == false)
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * K_HALF, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * K_HALF;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * K_HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                mb_Crouching = true;
            }
        }
    }


    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.smoothDeltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.smoothDeltaTime);
        m_Animator.SetBool("Crouch", mb_Crouching);
        m_Animator.SetBool("OnGround", mb_IsGrounded);
        m_Animator.SetBool("IsAirAttack", mb_IsAirAttacking);
        

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1로 활성화.
        if (mb_IsAttacking == true &&
            mb_IsGrounded &&
            m_Rigidbody.velocity.magnitude < 2.0f)
        {
            m_Animator.SetInteger("AttackState", 1);
        }

        // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
        if (mb_IsAttacking == true &&
            mb_IsGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            mb_IsDashAttack = true;
        }

        m_Animator.SetBool("IsDashAttack", mb_IsDashAttack);

        if (mb_IsGrounded == false)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)

        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + RUN_CYCLE_LEG_OFFSET, 1);
        float jumpLeg = (runCycle < K_HALF ? 1 : -1) * m_ForwardAmount;

        if (mb_IsGrounded == true)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (mb_IsGrounded == true && move.magnitude > 0)
        {
            m_Animator.speed = ANIM_SPEED_MULTIPLIER;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
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

        // 공중에서 공격
        if (Input.GetButtonDown("Attack"))
        {
            mb_IsAirAttacking = true;
        }

    }


    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // 점프 
        if (jump == true &&
            crouch == false &&
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            mb_IsGrounded = false;

            m_GroundCheckDistance = 0.1f;
            return;
        }

        // 지상에서의 키 입력에 따른 캐릭터 조정
        m_Rigidbody.velocity =
           new Vector3(m_MoveSpeed * m_Move.x,
           m_Rigidbody.velocity.y,
           m_MoveSpeed * m_Move.z);
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

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
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


