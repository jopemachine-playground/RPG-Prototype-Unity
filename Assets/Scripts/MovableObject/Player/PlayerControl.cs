using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아래 스크립트의 작성은 Stardard Asset의 ThirdPersonControl와
// http://www.yes24.com/24/goods/27894042 도서를 참고함

namespace UnityChanRPG
{
    public class PlayerControl : MonoBehaviour, IInteractAble
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

        // 스태미나 소모, 회복량의 Default 값
        private float staminaRecoverMultiplier = 15f;
        private float staminaUseMultiplier = 10f;

        // 일정 시간 이상 키보드 Input이 들어오지 않으면 랜덤으로 3개의 Waiting motion 중 하나를 재생
        private const float waitingTimeForWaitingMotion = 15.0f;
        private float waitingTimeForWaitingMotionTimer;

        private AttackArea LeftHand;
        private AttackArea RightHand;
        private AttackArea LeftFoot;
        private AttackArea RightFoot;

        public CharacterController controller;
        private const float gravityValue = 15f;
        public Vector3 currentVelocity;

        // 공격 애니메이션 String을 담는 변수
        public string AnimationNameString;

        // true인 경우 캐릭터를 조작할 수 없음
        public bool NoInputMode;

        #endregion

        private void Start()
        {
            NoInputMode = false;
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
                }
            }
        }

        private void Update()
        {
            if (NoInputMode == true)
            {
                return;
            }

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
                player.playerStatus.stamina -= staminaUseMultiplier * Time.deltaTime;
                MoveVector *= 2;
            }
            else if (Input.GetKey(KeyCode.LeftShift) &&
                Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
                status.stamina < 30 &&
                (h != 0 | v != 0))
            {
                player.playerStatus.stamina -= staminaUseMultiplier * Time.deltaTime;
                MoveVector *= 0.5f;
            }
            else
            {
                if (status.stamina + staminaRecoverMultiplier * Time.deltaTime < Player.mInstance.StaminaMax)
                {
                    player.playerStatus.stamina += staminaRecoverMultiplier * Time.deltaTime;
                }
                else if (status.stamina < Player.mInstance.StaminaMax)
                {
                    player.playerStatus.stamina = Player.mInstance.StaminaMax;
                }
            }

            // Attack 중이라면 움직일 수 없음
            if (Animator.GetInteger("AttackState") == 0)
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

        void UpdateAnimator(Vector3 move)
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


        void HandleAirborneMovement()
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


        void HandleGroundedMovement(bool IsJump)
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

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(STATIONARY_TURN_SPEED, MOVING_TURN_SPEED, ForwardAmount);
            transform.Rotate(0, TurnAmount * turnSpeed * Time.smoothDeltaTime, 0);
        }

        #region Handle Attack Event

        public void HandleAttackParticle(ref Damage damage)
        {
            // Animator의 실행 중인 애니메이션 이름을 구하는 함수가 없어, 직접 AnimationNameString을 선언해 사용했음
            PlayerSkill skill = PlayerSkillManager.GetSkill(AnimationNameString);

            damage.skillCoefficient = skill.AttackValueCoefficient;
            damage.EmittingParticleID = skill.EmittingParticleID;
        }

        public bool HandleAttackEvent()
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

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack1"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OnAttack();
                    RightFoot.OffAttack();
                    RightHand.OffAttack();

                    AnimationNameString = "Punch Attack1";
                    return true;
                }

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack2"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OffAttack();
                    RightFoot.OffAttack();
                    RightHand.OnAttack();

                    AnimationNameString = "Punch Attack2";
                    return true;
                }

                if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Punch Attack3"))
                {
                    LeftFoot.OffAttack();
                    LeftHand.OffAttack();
                    RightFoot.OffAttack();
                    RightHand.OnAttack();

                    AnimationNameString = "Punch Attack3";
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

                AnimationNameString = "";
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
            if (status.currentHP <= 0)
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