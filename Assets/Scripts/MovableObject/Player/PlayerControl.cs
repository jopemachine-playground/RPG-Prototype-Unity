using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stardard Asset의 ThirdPersonControl 수정

public class PlayerControl : MonoBehaviour
{
    private Player player;

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
    private bool IsKickAttacking;
    private bool IsPunchAttacking;
    private bool IsAirAttacking;
    private bool IsDashAttack;

    private Rigidbody Rigidbody;
    private Animator Animator;
    private CapsuleCollider Capsule;
    private AudioManager voiceAudioManager;
    private AudioManager moveAudioManager;
    private Transform playerTr;
    private Transform cam;

    // 피격 당한 경우 잠시의 무적 시간
    public const float gracePeriod = 0.5f;
    public bool IsGracePeriod;

    // 스태미나 소모, 회복량의 Default 값
    private float staminaRecoverMultiplier = 10f;
    private float staminaUseMultiplier = 10f;

    // 일정 시간 이상 키보드 Input이 들어오지 않으면 랜덤으로 3개의 Waiting motion 중 하나를 재생
    private const float waitingTimeForWaitingMotion = 15.0f;
    private float waitingTimeForWaitingMotionTimer;

    private AttackArea LeftHand;
    private AttackArea RightHand;
    private AttackArea LeftFoot;
    private AttackArea RightFoot;

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
        walk
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
            case MoveSound.walk:
                volume = 35;
                break;
            default:
                Debug.Assert(false, "unexpected value");
                break;
        }

        moveAudioManager.Play(moveSoundInt, volume);
    }

    #endregion

    void Start()
    {
        player = GetComponent<Player>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Transform>();
        voiceAudioManager = transform.Find("Sound").Find("Voice").gameObject.GetComponent<AudioManager>();
        moveAudioManager = transform.Find("Sound").Find("Move").gameObject.GetComponent<AudioManager>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        playerTr = GetComponent<Transform>();
        Capsule = GetComponent<CapsuleCollider>();

        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;

        waitingTimeForWaitingMotionTimer = 0;
        GroundCheckDistance = 0.1f;
        OrigGroundCheckDistance = GroundCheckDistance;

        AttackArea[] area = gameObject.GetComponentsInChildren<AttackArea>();

        for (int i = 0; i< area.Length; i++)
        {
            switch (area[i].name)
            {
                case "Character1_RightFoot":
                    RightFoot = area[i];
                    break;
                case "Character1_LeftFoot":
                    LeftFoot = area[i];
                    break;
                case "Character1_LeftHand":
                    LeftHand = area[i];
                    break;
                case "Character1_RightHand":
                    RightHand = area[i];
                    break;
            }

        }
        
    }

    // 프레임 단위로 입력이 들어오는지 검사해야 하는 Jump와 Attack 입력의 경우 Update에서 따로 입력을 검사한다.
    private void Update()
    {
        if (IsJump == false)
        {
            IsJump = Input.GetButtonDown("Jump");
        }

        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") == true)
        {
            if (Input.GetButtonDown("KickAttack"))
            {
                IsKickAttacking = true;
                BreakRestTime();
            }

            else if (Input.GetButtonDown("PunchAttack"))
            {
                IsPunchAttacking = true;
                BreakRestTime();
            }
        }

        if (Player.mInstance.playerStatus.currentHP <= 0)
        {
            // 플레이어 사망에 관한 이벤트 처리는 여기서.
            // 지금은 원활한 디버깅을 위해 주석 처리

            // Animator.Play("Death");
        }

        HandleAttackEvent();

    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        waitingTimeForWaitingMotionTimer += Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if ((h != 0 | v != 0))
        {
            BreakRestTime();
        }

        // 기본동작은 걷기
        CamForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        MoveVector = (v * CamForward + h * cam.right) / 2;

        // 지상에서 왼쪽 쉬프트 버튼을 누르면 달리기를 하며 속도가 두 배가 되지만, 스태미나를 소모한다.
        // 스태미나 부족 상태에서 달리려하면 속도가 느려진다.
        if (Input.GetKey(KeyCode.LeftShift) &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            player.playerStatus.stamina > 30 &&
            (h != 0 | v != 0))
        {
            player.playerStatus.stamina -= staminaUseMultiplier * Time.deltaTime;
            MoveVector *= 2;
        }
        else if (Input.GetKey(KeyCode.LeftShift) &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            player.playerStatus.stamina < 30 &&
            (h != 0 | v != 0))
        {
            player.playerStatus.stamina -= staminaUseMultiplier * Time.deltaTime;
            MoveVector *= 0.5f;
        }
        else
        {
            if (player.playerStatus.stamina + staminaRecoverMultiplier * Time.deltaTime < Player.mInstance.StaminaMax)
            {
                player.playerStatus.stamina += staminaRecoverMultiplier * Time.deltaTime;
            }
            else if (player.playerStatus.stamina < Player.mInstance.StaminaMax)
            {
                player.playerStatus.stamina = Player.mInstance.StaminaMax;
            }

        }

        // Attack 중이라면 움직일 수 없음
        if (Animator.GetInteger("AttackState") == 0 &&
            (Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") | Animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne")))
        {
            Move(MoveVector, IsJump);
        }

        if (waitingTimeForWaitingMotionTimer > waitingTimeForWaitingMotion)
        {
            RandomDecideRestType();
            waitingTimeForWaitingMotionTimer = 0;
        }

        IsKickAttacking = false;
        IsPunchAttacking = false;
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
        if (IsGrounded == true && player.playerStatus.stamina > 15)
        {
            HandleGroundedMovement(IsJump);
        }
        else if (IsGrounded == true && player.playerStatus.stamina < 15)
        {
            Animator.Play("Refresh");
            Rigidbody.velocity = Vector3.zero;
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
        if (IsKickAttacking == true &&
            Rigidbody.velocity.magnitude > 1.0f &&
            IsGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            IsDashAttack = true;
        }

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1로 활성화.
        if (IsGrounded &&
            IsDashAttack != true)
        {
            if (IsKickAttacking == true)
            {
                Rigidbody.velocity = Vector3.zero;
                Animator.SetInteger("AttackState", 1);
            }
            else if (IsPunchAttacking == true)
            {
                Rigidbody.velocity = Vector3.zero;
                Animator.SetInteger("AttackState", 4);
            }
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
        if (Input.GetButtonDown("KickAttack"))
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

    private bool HandleAttackEvent()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsTag("DamageAttack") |
            Animator.GetCurrentAnimatorStateInfo(0).IsTag("DownAttack"))
        {
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Kick Attack1"))
            {
                LeftFoot.OnAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OffAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Kick Attack2"))
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OnAttack();
                RightHand.OffAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Kick Attack3"))
            {
                LeftFoot.OnAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OffAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack1"))
            {
                LeftFoot.OffAttack();
                LeftHand.OnAttack();
                RightFoot.OffAttack();
                RightHand.OffAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack2"))
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OnAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack3"))
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OnAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack4"))
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OnAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OnAttack();
                RightHand.OffAttack();
                return true;
            }

            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Air Attack"))
            {
                return true;
            }

        }

        else
        {
            LeftFoot.OffAttack();
            LeftHand.OffAttack();
            RightFoot.OffAttack();
            RightHand.OffAttack();
            return false;
        }

        return false;
    }

    private void RandomDecideRestType()
    {
        Animator.SetInteger("RestType", UnityEngine.Random.Range(1, 4));
    }

    private void BreakRestTime()
    {
        // 키보드 입력이 들어왔을 때, 데미지를 받았을 때 호출되어 RestType을 0으로 만든다.
        Animator.SetInteger("RestType", 0);
        waitingTimeForWaitingMotionTimer = 0;
    }

    private void HandleGracePeriod(float time)
    {
        IsGracePeriod = true;
        CancelInvoke();
        // 일어서는 시간에 공격을 무시하고, gracePeriod는 따로 부여
        Invoke("OffGracePeriod", time);
    }

    private void OffGracePeriod()
    {
        IsGracePeriod = false;
    }

    public void Damaged(Damage damage)
    {
        // IsGracePeriod 동안엔 더 공격을 맞아도 애니메이션을 적용하지 않음
        // 데미지 계산은 적용됨
        if (IsGracePeriod)
        {
            return;
        }

        BreakRestTime();
        HandleGracePeriod(gracePeriod);

        if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Animator.Play("Damaged");
            Rigidbody.velocity = Vector3.zero;
        }

        else if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
        {
            Animator.Play("Down");
            Rigidbody.velocity = Vector3.zero;
        }
    }

}


