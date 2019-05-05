using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아래 스크립트는 Terrain에서 이동하다보면 버그가 보이기 쉬우니 주의할 것.
/// </summary>

// 아래 스크립트의 작성은 Stardard Asset의 ThirdPersonControl를 수정해 만들었다

namespace UnityChanRPG
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Player))]
    public class PlayerControl : MonoBehaviour
    {
        #region Variables
        [NonSerialized]
        public Player player;
        [NonSerialized]
        public Status status;

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
        private bool IsAirAttacking;
        private bool IsDashAttack;
        private bool IsFalling;

        private Animator Animator;
        private AudioManager voiceAudioManager;
        private AudioManager moveAudioManager;
        private Transform playerTr;
        private Transform cam;
        private new Rigidbody rigidbody;

        // 피격 당한 경우 잠시의 무적 시간
        public const float gracePeriod = 0.5f;
        public bool IsGracePeriod;

        // 스태미나 소모, 회복량의 Default 값 (아이템 사용등으로 변경 가능하므로 const가 아님)
        private float staminaRecoverMultiplier = 15f;
        private float staminaUseMultiplier = 10f;

        // 레이 캐스팅을 통해 측정된 거리가 GroundCheckDistance 보다 작다면 지면에 있는 것으로 처리
        public const float GroundCheckDistance = 1.0f;
        private float DistanceFromGround;

        // 일정 시간 이상 키보드 Input이 들어오지 않으면 랜덤으로 3개의 Waiting motion 중 하나를 재생
        private const float waitingTimeForWaitingMotion = 15.0f;
        private float waitingTimeForWaitingMotionTimer;

        private AttackArea LeftHand;
        private AttackArea RightHand;
        private AttackArea LeftFoot;
        private AttackArea RightFoot;

        private HitArea playerHitArea;

        public CharacterController controller;
        private const float gravityValue = 15f;
        public Vector3 currentVelocity;

        // TerrainSlope가 CharacterController의 Slope Limit보다 높을 때, 즉 허가되지 않은 높이의 지형에 점프했을 땐,
        // Rigidbody로 움직이게 하고, 미끄러져 Slope가 Limit보다 작아질 때 까지 입력을 받지 않는다.
        public float TerrainSlope;
        public bool IsSliding;
        private const float SlideTime = 1.0f;

        // 공격 애니메이션 String을 담는 변수
        public string AnimationNameString;

        // true인 경우 캐릭터를 조작할 수 없음
        public bool NoInputMode;

        private LayerMask TerrainLayer;

        #endregion

        private void Awake()
        {
            NoInputMode = false;
            player = GetComponent<Player>();
            status = GetComponent<Status>();
            Animator = GetComponent<Animator>();
            playerTr = GetComponent<Transform>();
            controller = GetComponent<CharacterController>();
            rigidbody = GetComponent<Rigidbody>();
            playerHitArea = GetComponent<HitArea>();

            cam = GameObject.FindGameObjectWithTag("MainCamera").gameObject.GetComponent<Transform>();

            voiceAudioManager = transform.Find("Sound").Find("Voice").gameObject.GetComponent<AudioManager>();
            moveAudioManager = transform.Find("Sound").Find("Move").gameObject.GetComponent<AudioManager>();

            TerrainLayer = LayerMask.NameToLayer("Terrain");

            CapsuleHeight = controller.height;
            CapsuleCenter = controller.center;

            waitingTimeForWaitingMotionTimer = 0;

            AttackArea[] area = gameObject.GetComponentsInChildren<AttackArea>();
            playerHitArea.handleAttackedEvent += HandleAttackedEvent;

            for (int i = 0; i < area.Length; i++)
            {
                switch (area[i].name)
                {
                    case "Character1_RightFoot":
                        RightFoot = area[i];
                        RightFoot.handleAttackEvent += HandleAttackParticle;
                        break;
                    case "Character1_LeftFoot":
                        LeftFoot = area[i];
                        LeftFoot.handleAttackEvent += HandleAttackParticle;
                        break;
                    case "Character1_LeftHand":
                        LeftHand = area[i];
                        LeftHand.handleAttackEvent += HandleAttackParticle;
                        break;
                    case "Character1_RightHand":
                        RightHand = area[i];
                        RightHand.handleAttackEvent += HandleAttackParticle;
                        break;
                }
            }
        }

        private void Update()
        {

            CheckTerrainStatus();

            if (NoInputMode == true || IsSliding == true)
            {
                return;
            }

            // 랜딩, 땅에서 걸어 다닐 때
            if (ControlFlags.IsOnGroundable(DistanceFromGround, rigidbody, TerrainSlope, controller.slopeLimit)) {
                IsOnGrounded = true;
                IsFalling = false;
                rigidbody.velocity = Vector3.zero;
                rigidbody.useGravity = false;
                controller.enabled = true;
                IsSliding = false;
            }
   
            // 미끄러짐 처리. 
            else if (ControlFlags.IsSlideable(DistanceFromGround, TerrainSlope, controller.slopeLimit))
            {
                IsOnGrounded = false;
                rigidbody.useGravity = true;
                controller.enabled = false;
                IsSliding = true;
                // 미끄러진 다음 일정 시간 동안 update, fixedupdate에서 입력을 받지 않고, TerrainSlope가 controller.slopeLimit와 충분한 간격을 두도록 강제함
                Invoke("SlideOff", SlideTime);
            }

            // 공중에 있을 때
            else if(ControlFlags.IsOnAirable(DistanceFromGround, rigidbody))
            {
                IsOnGrounded = false;
                rigidbody.useGravity = true;
                controller.enabled = false;
                IsSliding = false;
                // 공중에 떠 있다면 Update는 그대로 return
                return;
            }

            waitingTimeForWaitingMotionTimer += Time.deltaTime;

            ToggleAttackArea();

            if (ControlFlags.IsOnGround(Animator))
            {
                if (ControlKeyStates.KickAttackButtonClicked())
                {
                    IsKickAttacking = true;
                    BreakRestTime();
                }
            }

            HandleDeathEvent();

            if (!ControlKeyStates.ArrowButtonClicked())
            {
                BreakRestTime();
            }

            // 기본동작은 걷기
            CamForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            MoveVector = (Input.GetAxis("Vertical") * CamForward + Input.GetAxis("Horizontal") * cam.right) / 2;

            // 지상에서 왼쪽 쉬프트 버튼을 누르면 달리기를 하며 속도가 두 배가 되지만, 스태미나를 소모한다.
            // 스태미나 부족 상태에서 달리려하면 속도가 느려지고, 계속되면 Relax 애니메이션에 들어가, 스태미너를 회복할 때 까지 움직일 수 없게 됨
            if (ControlFlags.IsRunnable(Animator, status))
            {
                status.Stamina -= staminaUseMultiplier * Time.deltaTime;
                MoveVector *= 2;
            }
            else if (ControlFlags.IsGaspable(Animator, status))
            {
                status.Stamina -= staminaUseMultiplier * Time.deltaTime;
                MoveVector *= 0.5f;
            }
            else
            {
                if (ControlFlags.IsStaminaRecoverable(status, staminaRecoverMultiplier))
                {
                    status.Stamina += staminaRecoverMultiplier * Time.deltaTime;
                }
                else if (ControlFlags.IsStaminaFull(status))
                {
                    status.Stamina = player.StaminaMax;
                }
            }

            if (ControlFlags.IsMoveable(Animator, IsOnGrounded))
            {
                if (ControlFlags.IsRefreshable())
                {
                    Animator.Play("Refresh");
                    BreakRestTime();
                }
                else 
                {
                    HandleGroundedMovement(MoveVector, IsJump);
                }
            }

            if (waitingTimeForWaitingMotionTimer > waitingTimeForWaitingMotion)
            {
                RandomDecideRestType();
                waitingTimeForWaitingMotionTimer = 0;
            }

            UpdateAnimator();

            IsKickAttacking = false;
            IsAirAttacking = false;
            IsDashAttack = false;
            IsJump = false;
        }

        void UpdateAnimator()
        {
            Animator.SetFloat("Forward", ForwardAmount, 0.1f, Time.smoothDeltaTime);
            Animator.SetFloat("Turn", TurnAmount, 0.1f, Time.smoothDeltaTime);
            Animator.SetFloat("Height", DistanceFromGround);
            Animator.SetBool("OnGround", IsOnGrounded);
            Animator.SetBool("IsAirAttack", IsAirAttacking);
            Animator.SetBool("IsJump", IsJump);
            Animator.SetBool("IsFalling", IsFalling);

            // 지상에서 대쉬상태에서 공격버튼이 눌러지면 대쉬어택
            if (IsOnGrounded && ControlKeyStates.DashAttackButtonClicked())
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
            }

            Animator.SetBool("IsDashAttack", IsDashAttack);
        }


        private void SlideOff()
        {
            // Character Controller가 활성화 될 때 이전의 ForwardAmount, TurnAmount 값이 들어가 있으면 그 쪽 방향으로 위치가 갱신되어 순간이동하는 것 처럼 보이게 된다.
            // 이걸 방지 하기 위해 0을 대입해줘야 함.
            ForwardAmount = 0;
            TurnAmount = 0;
            currentVelocity = Vector3.zero;
            IsSliding = false;
        }


        void HandleGroundedMovement(Vector3 moveVector, bool IsJump)
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

                controller.Move(currentVelocity * Time.deltaTime + snapGround);
            }

        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, ForwardAmount);
            transform.Rotate(0, TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
        }

        #region Handle Attack Event

        public void HandleAttackParticle(ref Damage damage)
        {
            // Animator의 실행 중인 애니메이션 이름을 구하는 함수가 없어, 직접 AnimationNameString을 선언해 사용했음
            Skill skill = SkillManager.GetSkill(0, AnimationNameString);

            damage.skillCoefficient = skill.AttackValueCoefficient;
            damage.EmittingParticleID = skill.EmittingParticleID;
        }

        public bool ToggleAttackArea()
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

                    AnimationNameString = "Kick Attack1";
                    return true;
                }

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Kick Attack2"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OffAttack();
                    RightFoot.OnAttack();
                    RightHand.OffAttack();

                    AnimationNameString = "Kick Attack2";
                    return true;
                }

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Kick Attack3"))
                {
                    LeftFoot.OnAttack();
                    LeftHand.OffAttack();
                    RightFoot.OffAttack();
                    RightHand.OffAttack();

                    AnimationNameString = "Kick Attack3";
                    return true;
                }


                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack4"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OffAttack();
                    RightFoot.OffAttack();
                    RightHand.OnAttack();

                    AnimationNameString = "Punch Attack4";
                    return true;
                }

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OffAttack();
                    RightFoot.OnAttack();
                    RightHand.OffAttack();

                    AnimationNameString = "Dash Attack";
                    return true;
                }

            }

            else
            {
                LeftFoot.OffAttack();
                LeftHand.OffAttack();
                RightFoot.OffAttack();
                RightHand.OffAttack();

                AnimationNameString = "";
            }

            return false;
        }
        #endregion

        #region Handle Attacked Event
        public void HandleAttackedEvent(Damage damage)
        {
            BreakRestTime();

            if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                Animator.SetBool("IsDamaged", true);
                LookAtAndInitAngle(damage.attacker.transform);
                currentVelocity = Vector3.zero;
            }

            else if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
            {
                Animator.Play("Down");
                LookAtAndInitAngle(damage.attacker.transform);
                currentVelocity = Vector3.zero;
            }
        }
        #endregion

        #region Handle Other Event

        private void HandleDeathEvent()
        {
            if (status.CurrentHP <= 0)
            {
                // 플레이어 사망에 관한 이벤트 처리는 여기서.
                // 지금은 원활한 디버깅을 위해 주석 처리

                // Animator.Play("Death");
            }
        }

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

        private void CheckTerrainStatus()
        {
            // 레이 캐스팅 방식을 이용해 지면까지 남은 거리를 계산
            RaycastHit hitInfo;

            // 아래의 Ray는 Terrain Layer만을 감지한다.
            // https://docs.unity3d.com/kr/530/Manual/Layers.html 참고

            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, 1 << TerrainLayer))
            {
                GroundNormal = hitInfo.normal;

                // Terrain 버텍스의 GroundNormal과 Vector3.up (y축)이 이루는 각이 구하는 'Terrain의 경사도' 이다.
                TerrainSlope = Vector3.Angle(GroundNormal, Vector3.up);
            }
            else
            {
                GroundNormal = Vector3.up;
            }

            DistanceFromGround = hitInfo.distance;

            // 지상에서 GroundCheckDistance 가 DistanceFromGround보다 작으면 버그가 생김.
            // GroundCheckDistance를 0.5 정도로 맞추면 버그가 없어지지만, 점프할 때 공중에 착지하는 버그가 있어 아래처럼 씀 

            // GroundCheckDistance = (TerrainSlope / 100f) + 0.15f;

        }

        public void LookAtAndInitAngle(Transform target)
        {
            transform.LookAt(target);
            Vector3 swap = new Vector3(0, transform.localEulerAngles.y, 0);
            transform.localEulerAngles = swap;
        }
        public void LookAtAndInitAngle(Vector3 target)
        {
            transform.LookAt(target);
            Vector3 swap = new Vector3(0, transform.localEulerAngles.y, 0);
            transform.localEulerAngles = swap;
        }

        public void CameraChange(Transform _camTr)
        {
            cam = _camTr;
        }


    }
}