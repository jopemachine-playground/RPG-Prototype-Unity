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
    private bool IsOnGrounded;
    private bool IsKickAttacking;
    private bool IsPunchAttacking;
    private bool IsAirAttacking;
    private bool IsDashAttack;
    private bool IsFalling;

    private Animator Animator;
    private AudioManager voiceAudioManager;
    private AudioManager moveAudioManager;
    private Transform playerTr;
    private Transform cam;
    private new Rigidbody rigidbody;

    // 스태미나 소모, 회복량의 Default 값 (후에 아이템 사용등으로 변경 가능하므로 const가 아님)
    public float staminaRecoverMultiplier = 15f;
    public float staminaUseMultiplier = 10f;

    // 일정 시간 이상 키보드 Input이 들어오지 않으면 랜덤으로 3개의 Waiting motion 중 하나를 재생
    private const float waitingTimeForWaitingMotion = 15.0f;
    private float waitingTimeForWaitingMotionTimer;

    // 레이 캐스팅을 통해 측정된 거리가 GroundCheckDistance 보다 작다면 지면에 있는 것으로 처리
    private const float GroundCheckDistance = 0.2f;
    private float DistanceFromGround;

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
        rigidbody = GetComponent<Rigidbody>();

        cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Transform>();
        voiceAudioManager = transform.Find("Sound").Find("Voice").gameObject.GetComponent<AudioManager>();
        moveAudioManager = transform.Find("Sound").Find("Move").gameObject.GetComponent<AudioManager>();

        CapsuleHeight = controller.height;
        CapsuleCenter = controller.center;

        waitingTimeForWaitingMotionTimer = 0;

        AttackArea[] area = gameObject.GetComponentsInChildren<AttackArea>();

        for (int i = 0; i < area.Length; i++)
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
        DistanceFromGround = CalcHeight();

        if (DistanceFromGround <= GroundCheckDistance && rigidbody.velocity.y <= 0.001f)
        {
            // 점프 중엔 DistanceFromGround <= GroundCheckDistance 라도 IsOnGrounded를 false로 체크해줘야 한다.
            // 따라서, currentVelocity.y가 음의 방향일 때만 IsOnGrounded를 true로 체크함
            IsOnGrounded = true;
            IsFalling = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = false;
            rigidbody.Sleep();
            controller.enabled = true;
        }
        else
        {
            IsOnGrounded = false;
            rigidbody.useGravity = true;
            controller.enabled = false;
        }

        waitingTimeForWaitingMotionTimer += Time.deltaTime;

        if (IsOnGrounded == false)
        {
            return;
        }

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
        // 스태미나 부족 상태에서 달리려하면 속도가 느려지고, 계속되면 Relax 애니메이션에 들어가, 스태미너를 회복할 때 까지 움직일 수 없게 됨
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
        if (Animator.GetInteger("AttackState") == 0 && IsOnGrounded == true &&
           (Animator.GetCurrentAnimatorStateInfo(0).IsTag("Ground") |
            Animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack")))
        {
            if (IsOnGrounded == true && player.playerStatus.stamina > 15)
            {
                HandleGroundedMovement(MoveVector, IsJump);
            }
            else if (IsOnGrounded == true && player.playerStatus.stamina <= 15)
            {
                Animator.Play("Refresh");
                BreakRestTime();
            }
        }

        if (waitingTimeForWaitingMotionTimer > waitingTimeForWaitingMotion)
        {
            RandomDecideRestType();
            waitingTimeForWaitingMotionTimer = 0;
        }

        UpdateAnimator();

        IsKickAttacking = false;
        IsPunchAttacking = false;
        IsAirAttacking = false;
        IsDashAttack = false;
        IsJump = false;
    }

    // 공중에서의 움직임 처리는 FixedUpdate로, 지상에서의 움직임 처리는 Update로 해야 끊기지 않는다.
    private void FixedUpdate()
    {
        if (IsOnGrounded == true)
        {
            return;
        }

        if (rigidbody.velocity.y < -1)
        {
            IsFalling = true;
        }

        BreakRestTime();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (status.currentHP <= 0)
        {
            // 플레이어 사망에 관한 이벤트 처리는 여기서.
            // 지금은 원활한 디버깅을 위해 주석 처리

            // Animator.Play("Death");
        }

        HandleAttackEvent();

        // 기본동작은 걷기
        CamForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        MoveVector = (v * CamForward + h * cam.right) / 2;

        if (status.stamina + staminaRecoverMultiplier * Time.deltaTime < player.StaminaMax)
        {
            status.stamina += staminaRecoverMultiplier * Time.deltaTime;
        }
        else if (status.stamina < player.StaminaMax)
        {
            status.stamina = player.StaminaMax;
        }

        // 공중에서도 왼쪽 쉬프트 버튼을 누르면 살짝 빨라지게 처리함.
        // 스태미너는 소모 되지 않음 (FixedUpdate에서 Time.deltaTime으로 처리해도 정상적으로 감소하지 않음)
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveVector *= 1.25f;
        }

        // Attack 중이라면 움직일 수 없음
        if (Animator.GetInteger("AttackState") == 0 &&
           (Animator.GetCurrentAnimatorStateInfo(0).IsTag("Airborne Movable")))
        {
            HandleAirborneMovement(MoveVector);
        }

        UpdateAnimator();

        IsKickAttacking = false;
        IsPunchAttacking = false;
        IsAirAttacking = false;
        IsDashAttack = false;
        IsJump = false;

    }


    private void UpdateAnimator()
    {
        Animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetFloat("Turn", TurnAmount, 0.1f, Time.smoothDeltaTime);
        Animator.SetFloat("Height", DistanceFromGround);
        Animator.SetBool("OnGround", IsOnGrounded);
        Animator.SetBool("IsAirAttack", IsAirAttacking);
        Animator.SetBool("IsJump", IsJump);
        Animator.SetBool("IsFalling", IsFalling);

        // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
        if (IsKickAttacking == true &&
            IsOnGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            IsDashAttack = true;
        }

        // 지상에서 움직이지 않는 상태에서 공격버튼이 눌러지면 AttackState를 1, 4로 활성화.
        if (IsOnGrounded &&
            IsDashAttack == false)
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

    private void HandleGroundedMovement(Vector3 moveVector, bool IsJump)
    {
        if (moveVector.magnitude > 1f) moveVector.Normalize();
        moveVector = transform.InverseTransformDirection(moveVector);
        moveVector = Vector3.ProjectOnPlane(moveVector, GroundNormal);
        TurnAmount = Mathf.Atan2(moveVector.x, moveVector.z);
        ForwardAmount = moveVector.z;
        ApplyExtraTurnRotation();

        Vector3 snapGround = Vector3.down;

        // 입력 값에 따라 캐릭터를 조정
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Landing") == false)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, new Vector3(MoveSpeed * MoveVector.x, 0, MoveSpeed * MoveVector.z), Time.deltaTime * 5f);

            // 지상에서의 점프 처리 
            if (IsJump == true &&
                Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                currentVelocity = Vector3.zero;
                controller.enabled = false;
                rigidbody.useGravity = true;
                rigidbody.velocity = new Vector3(currentVelocity.x, JumpPower, currentVelocity.z);
                return;
            }
            controller.Move(currentVelocity * Time.deltaTime + snapGround);
        }
    }


    private void HandleAirborneMovement(Vector3 moveVector)
    {
        if (moveVector.magnitude > 1f) moveVector.Normalize();
        moveVector = transform.InverseTransformDirection(moveVector);
        moveVector = Vector3.ProjectOnPlane(moveVector, GroundNormal);
        TurnAmount = Mathf.Atan2(moveVector.x, moveVector.z);
        ForwardAmount = moveVector.z;

        ApplyExtraTurnRotation();

        if (Input.GetButtonDown("KickAttack"))
        {
            IsAirAttacking = true;
        }

        // 추가적인 중력 부여 
        // (높은 곳에서 낙하해도 발이 Terrain에 묻히지 않게 하기 위해, Mass를 일정 값 이상으로 잡아야 하는데 이 때 움직임이 느려지는 것을 방지)
        rigidbody.AddForce(rigidbody.mass * Physics.gravity);

        // 공중에서의 키 입력에 따른 캐릭터 조정
        rigidbody.velocity =
           new Vector3(0.65f * MoveSpeed * MoveVector.x,
           rigidbody.velocity.y,
           0.65f * MoveSpeed * MoveVector.z);
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, ForwardAmount);
        transform.Rotate(0, TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
    }

    private float CalcHeight()
    {
        // 레이 캐스팅 방식을 이용해 지면까지 남은 거리를 계산
        RaycastHit hitInfo;

        // 아래의 마지막 인자에서 13은 Terrain Layer이다. 따라서, Ray는 Terrain Layer만을 감지한다.
        // https://docs.unity3d.com/kr/530/Manual/Layers.html 참고

        Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 1 << 13);

        // Debug.Log(hitInfo.distance);

        return hitInfo.distance;
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

    public void Damaged(Damage damage)
    {
        BreakRestTime();

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
        Animator.SetInteger("RestType", 0);
        waitingTimeForWaitingMotionTimer = 0;
    }

    #endregion

}


