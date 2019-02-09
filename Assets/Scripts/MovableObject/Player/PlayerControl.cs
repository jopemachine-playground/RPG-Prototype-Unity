using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아래 스크립트의 작성은 Stardard Asset의 ThirdPersonControl와
// http://www.yes24.com/24/goods/27894042 도서를 참고함

public class PlayerControl : MonoBehaviour
{
    #region Variables
    private Player player;
    private Status status;

    public float JumpPower;
    public float MoveSpeed;

    private Vector3 CamForward;
    private Vector3 MoveVector;
    private Vector3 GroundNormal;
    private Vector3 CapsuleCenter;

    private const float MOVING_TURN_SPEED = 360;
    private const float STATIONARY_TURN_SPEED = 180;

    private float TurnAmount;
    private float ForwardAmount;
    private float CapsuleHeight;

    private bool IsJump;
    private bool IsKickAttacking;
    private bool IsPunchAttacking;
    private bool IsAirAttacking;
    private bool IsDashAttack;

    private Animator Animator;
    private AudioManager voiceAudioManager;
    private AudioManager moveAudioManager;
    private Transform playerTr;
    private Transform cam;

    // 피격 당한 경우 잠시의 무적 시간
    public const float gracePeriod = 0.5f;
    public bool IsGracePeriod;

    // 스태미나 소모, 회복량의 Default 값 (후에 아이템 사용등으로 변경 가능하므로 const가 아님)
    public float staminaRecoverMultiplier = 15f;
    public float staminaUseMultiplier = 10f;

    // 일정 시간 이상 키보드 Input이 들어오지 않으면 랜덤으로 3개의 Waiting motion 중 하나를 재생
    private const float waitingTimeForWaitingMotion = 15.0f;
    private float waitingTimeForWaitingMotionTimer;

    private AttackArea LeftHand;
    private AttackArea RightHand;
    private AttackArea LeftFoot;
    private AttackArea RightFoot;

    private CharacterController controller;
    private const float gravityValue = 15f;
    public Vector3 currentVelocity;

    #endregion

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

    #region Initialize
    void Start()
    {
        player = GetComponent<Player>();
        status = GetComponent<Status>();
        Animator = GetComponent<Animator>();
        playerTr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();

        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Transform>();
        voiceAudioManager = transform.Find("Sound").Find("Voice").gameObject.GetComponent<AudioManager>();
        moveAudioManager = transform.Find("Sound").Find("Move").gameObject.GetComponent<AudioManager>();

        CapsuleHeight = controller.height;
        CapsuleCenter = controller.center;

        waitingTimeForWaitingMotionTimer = 0;

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
                default:
                    Debug.Assert(false, "PlayerControl.cs Error - Check UnAssigned AttackArea");
                    break;
            }
        }
    }

    #endregion

    #region Handle Move Event

    private void Update()
    {
        waitingTimeForWaitingMotionTimer += Time.deltaTime;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

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

        if (status.currentHP <= 0)
        {
            // 플레이어 사망에 관한 이벤트 처리는 여기서.
            // 지금은 원활한 디버깅을 위해 주석 처리

            // Animator.Play("Death");
        }

        HandleAttackEvent();

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
            status.stamina > 30 &&
            (h != 0 | v != 0))
        {
            status.stamina -= staminaUseMultiplier * Time.deltaTime;
            MoveVector *= 2;
        }
        else if (Input.GetKey(KeyCode.LeftShift) &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
            status.stamina < 30 &&
            (h != 0 | v != 0))
        {
            status.stamina -= staminaUseMultiplier * Time.deltaTime;
            MoveVector *= 0.5f;
        }
        else
        {
            if (status.stamina + staminaRecoverMultiplier * Time.deltaTime < player.StaminaMax)
            {
                status.stamina += staminaRecoverMultiplier * Time.deltaTime;
            }
            else if (status.stamina < player.StaminaMax)
            {
                status.stamina = player.StaminaMax;
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

    private void Move(Vector3 move, bool IsJump)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, GroundNormal);
        TurnAmount = Mathf.Atan2(move.x, move.z);
        ForwardAmount = move.z;

        ApplyExtraTurnRotation();
        
        if (controller.isGrounded == true && player.playerStatus.stamina > 15)
        {
            HandleGroundedMovement(IsJump);
        }
        else if (controller.isGrounded == true && player.playerStatus.stamina <= 15)
        {
            Animator.Play("Refresh");
        }
        else if (controller.isGrounded == false)
        {
            HandleAirborneMovement();
        }

        UpdateAnimator(move);
    }

    private void UpdateAnimator(Vector3 move)
    { 
        Animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetFloat("Turn", TurnAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetBool("OnGround", controller.isGrounded);
        Animator.SetBool("IsAirAttack", IsAirAttacking);
        Animator.SetBool("IsJump", IsJump);

        // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
        if (IsKickAttacking == true &&
            controller.isGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            IsDashAttack = true;
        }

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1, 4로 활성화.
        if (controller.isGrounded &&
            IsDashAttack != true)
        {
            if (IsKickAttacking == true)
            {
                currentVelocity = Vector3.zero;
                Animator.SetInteger("AttackState", 1);
            }
            else if (IsPunchAttacking == true)
            {
                currentVelocity = Vector3.zero;
                Animator.SetInteger("AttackState", 4);
            }
        }

        Animator.SetBool("IsDashAttack", IsDashAttack);
    }


    private void HandleAirborneMovement()
    {
        if (Input.GetButtonDown("KickAttack"))
        {
            IsAirAttacking = true;
        }

        // 입력 값에 따라 캐릭터를 조정
        currentVelocity = Vector3.Lerp(currentVelocity, new Vector3(0.65f * MoveSpeed * MoveVector.x, currentVelocity.y, 0.65f * MoveSpeed * MoveVector.z), Time.deltaTime * 5f);

        // currentVelocity.y 를 중력으로 보정 (양의 값 velocity.y는 위를 향함. Vector3.Down이 (0,-1,0) 이므로 아래와 같이 쓰면 아랫 방향을 가리킴)
        currentVelocity += Vector3.down * gravityValue * Time.smoothDeltaTime;

        // 최종 Move 함수 호출
        controller.Move(currentVelocity * Time.deltaTime);
    }


    private void HandleGroundedMovement(bool IsJump)
    {
        Vector3 snapGround = Vector3.down;

        // 입력 값에 따라 캐릭터를 조정
        currentVelocity = Vector3.Lerp(currentVelocity, new Vector3(MoveSpeed * MoveVector.x, 0, MoveSpeed * MoveVector.z), Time.deltaTime * 5f);

        // 점프 
        if (IsJump == true &&
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            currentVelocity.y = JumpPower;
            controller.Move(currentVelocity * Time.deltaTime);
            return;
        }

        // 최종 Move 함수 호출
        controller.Move(currentVelocity * Time.deltaTime + snapGround);

    }
    
    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, ForwardAmount);
        transform.Rotate(0, TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
    }

    #endregion

    #region Handle Attack Event

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
    #endregion

    #region Handle Attacked Event
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
            currentVelocity = Vector3.zero;
        }

        else if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
        {
            Animator.Play("Down");
            currentVelocity = Vector3.zero;
        }
    }

    #endregion

    #region Handle Other Event

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

    #endregion

}


