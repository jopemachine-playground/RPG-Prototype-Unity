using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stardard Asset의 ThirdPersonControl 수정

public class PlayerControl : MonoBehaviour
{
    public float JumpPower;
    public float MoveSpeed;

    private Vector3 CamForward;
    private Vector3 MoveVector;
    private Vector3 GroundNormal;
    private Vector3 CapsuleCenter;

    private const float MOVING_TURN_SPEED = 360;
    private const float STATIONARY_TURN_SPEED = 180;

    private float GRAVITY_MULTIPLIER = 2f;

    private float OrigGroundCheckDistance;
    private float TurnAmount;
    private float ForwardAmount;
    private float CapsuleHeight;
    private float GroundCheckDistance;

    private bool IsGrounded;
    private bool IsJump;
    private bool IsAttacking;
    private bool IsAirAttacking;
    private bool IsDashAttack;

    private Rigidbody Rigidbody;
    private Animator Animator;
    private CapsuleCollider Capsule;
    public AudioManager voiceAudioManager;
    public AudioManager moveAudioManager;
    private Transform playerTr;
    private Transform cam;

    // 피격 당한 경우 잠시의 무적 시간
    public const float gracePeriod = 0.5f;
    public bool IsGracePeriod;

    #region Sound Processing by Animation State
    private enum Voice
    {
        attack,
        attack2,
        jump,
        yay,
        hehehe,
        hahaha,
        ouch,
        damage,
        hmm,
        wow
    }

    private enum MoveSound
    {

    }

    private void HandleVoiceSoundByAnimation(Voice voice)
    {
        int voiceInt = (int)voice;
        float volume = 1f;

        switch (voice)
        {
            case Voice.attack:
                break;
            case Voice.attack2:
                break;
            case Voice.jump:
                break;
            case Voice.yay:
                break;
            case Voice.hehehe:
                break;
            case Voice.hahaha:
                break;
            case Voice.ouch:
                break;
            case Voice.damage:
                break;
            case Voice.hmm:
                break;
            case Voice.wow:
                break;
            default:
                Debug.Assert(false, "unexpected value");
                break;
        }

        voiceAudioManager.Play(voiceInt, volume);
    }

    private void HandleMoveSoundByAnimation(MoveSound moveSound)
    {
        int moveSoundInt = (int)moveSound;
        float volume = 1f;

        switch (moveSound)
        {
            default:
                Debug.Assert(false, "unexpected value");
                break;
        }

        moveAudioManager.Play(moveSoundInt, volume);
    }


    #endregion

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Transform>();
        voiceAudioManager = transform.Find("Sound").Find("Voice").gameObject.GetComponent<AudioManager>();
        moveAudioManager = transform.Find("Sound").Find("Move").gameObject.GetComponent<AudioManager>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        playerTr = GetComponent<Transform>();
        Capsule = GetComponent<CapsuleCollider>();

        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;

        GroundCheckDistance = 0.1f;
        OrigGroundCheckDistance = GroundCheckDistance;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (IsJump == false)
        {
            IsJump = Input.GetButtonDown("Jump");
        }

        if (IsAttacking == false &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            Input.GetButtonDown("Attack"))
        {
            IsAttacking = true;
        }

        // 기본동작은 걷기
        CamForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        MoveVector = (v * CamForward + h * cam.right) / 2;

        // 지상에서 왼쪽 쉬프트 버튼을 누르면 달리기
        if (Input.GetKey(KeyCode.LeftShift) && IsGrounded)
        {
            MoveVector *= 2;
        }

        // Attack 중이라면 움직일 수 없음
        if (Animator.GetInteger("AttackState") == 0 &&
            (Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") | Animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne") | Animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged")))
        {
            Move(MoveVector, IsJump);
        }

        IsAttacking = false;
        IsAirAttacking = false;
        IsDashAttack = false;
        IsJump = false;
    }

    public void Move(Vector3 move, bool IsJump)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, GroundNormal);
        TurnAmount = Mathf.Atan2(move.x, move.z);
        ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (IsGrounded == true)
        {
            HandleGroundedMovement(IsJump);
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
        Animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetFloat("Turn", TurnAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetBool("OnGround", IsGrounded);
        Animator.SetBool("IsAirAttack", IsAirAttacking);
        Animator.SetBool("IsJump", IsJump);

        // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
        if (IsAttacking == true &&
            Rigidbody.velocity.magnitude > 1.0f &&
            IsGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            IsDashAttack = true;
        }

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1로 활성화.
        if (IsAttacking == true &&
            IsGrounded &&
            IsDashAttack != true)
        {
            Rigidbody.velocity = Vector3.zero;
            Animator.SetInteger("AttackState", 1);
        }

        Animator.SetBool("IsDashAttack", IsDashAttack);

    }


    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * GRAVITY_MULTIPLIER) - Physics.gravity;
        Rigidbody.AddForce(extraGravityForce);

        GroundCheckDistance = Rigidbody.velocity.y < 0 ? OrigGroundCheckDistance : 0.01f;

        // 공중에서의 키 입력에 따른 캐릭터 조정
        Rigidbody.velocity =
           new Vector3(0.65f * MoveSpeed * MoveVector.x,
           Rigidbody.velocity.y,
           0.65f * MoveSpeed * MoveVector.z);

        // 공중에서 공격 시 더 빨리 떨어지게 조정
        // 특정 높이 이상에서만 공중 공격이 가능하도록 조정할 것
        if (Input.GetButtonDown("Attack"))
        {
            IsAirAttacking = true;
            Rigidbody.AddForce(10 * Physics.gravity);
        }
    }


    void HandleGroundedMovement(bool IsJump)
    {
        // 점프 
        if (IsJump == true &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpPower, Rigidbody.velocity.z);
            IsGrounded = false;

            GroundCheckDistance = 0.1f;
            return;
        }

        // 지상에서의 키 입력에 따른 캐릭터 조정

        Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, new Vector3(MoveSpeed * MoveVector.x, Rigidbody.velocity.y, MoveSpeed * MoveVector.z), Time.deltaTime * 8f);
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, ForwardAmount);
        transform.Rotate(0, TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;

        // 아래의 마지막 인자에서 10은 Floor Layer이다. 따라서, Ray는 Floor Layer만을 감지한다.
        // https://docs.unity3d.com/kr/530/Manual/Layers.html 참고

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, GroundCheckDistance, 1 << 10))
        {
            GroundNormal = hitInfo.normal;
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        // 무적 상태, 죽은 상태에선 공격을 무시함
        if (IsGracePeriod)
        {
            return;
        }

        #region UnderAttack Event

        if (coll.gameObject.tag == "OrcWeapon")
        {
            Animator monsterAnimator = coll.gameObject.GetComponentInParent<Animator>();

            if (monsterAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") |
                monsterAnimator.GetAnimatorTransitionInfo(0).IsUserName("AttackTrans"))
            {
                Animator.SetBool("IsDamaged", true);
                DamageIndicator.mInstance.CallFloatingText(playerTr, Player.mInstance.Damaged(coll.gameObject.GetComponentInParent<MonsterAdapter>().monster.DecideAttackValue()));
                Rigidbody.velocity = Vector3.zero;
                playerTr.LookAt(monsterAnimator.transform);
                IsGracePeriod = true;
                CancelInvoke();
                Invoke("OffGracePeriod", gracePeriod);
            }
        
        }

        // 몬스터 컴포넌트의 Collider의 IsTrigger를 체크할 순 없으니, 자식 컴포넌트에 Collider를 달아 사용했다.
        if (coll.gameObject.tag == "MonsterCollider")
        {
            Animator monsterAnimator = coll.gameObject.GetComponentInParent<Animator>();

            if (monsterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack") |
                monsterAnimator.GetAnimatorTransitionInfo(0).IsName("Dash Attack -> Attack"))
            {
                Animator.SetTrigger("Down");
                DamageIndicator.mInstance.CallFloatingText(playerTr, Player.mInstance.Damaged(coll.gameObject.GetComponentInParent<MonsterAdapter>().monster.DecideAttackValue()));
                playerTr.LookAt(monsterAnimator.transform);
                IsGracePeriod = true;
                CancelInvoke();
                // 일어서는 시간에 공격을 무시하고, gracePeriod는 따로 부여
                Invoke("OffGracePeriod", gracePeriod + monsterAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
        #endregion
    }

    private void OffGracePeriod()
    {
        IsGracePeriod = false;
    }

}


