using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// 아래 스크립트의 작성 중 Character Controller와 NavMeshAgent를 함께 사용하는 법에 대해
// https://forum.unity.com/threads/using-a-navmeshagent-with-a-charactercontroller.466902/ 를 참고함

public class MonsterControl : MonoBehaviour
{
    public MonsterAdapter monsterAdpt;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private CharacterController controller;
    public MonsterState AIState;

    public MonsterPatrolArea patrolArea;

    private static WaitForSeconds CheckingTime;

    private bool IsGrounded;
    private bool IsAttacking;
    private bool IsDashAttack;
    private bool IsDied;
    private bool IsStuned;

    private Vector3 movingDirection;
    private Vector3 nextMovingDirection;
    public Vector3 desireVelocity;

    private const float GroundCheckDistance = 0.1f;

    // 공격 거리
    public const float attackDistance = 2.0f;
    // 플레이어 탐지 거리
    public const float detectionDistance = 10.0f;
    // 대쉬 어택 판정 거리
    public const float dashAttackMinDistance = 12.0f;
    public const float dashAttackMaxDistance = 14.5f;

    // Roaming , Idle을 반복하는 시간을 재기 위한 타이머
    private const float RoamingTime = 6.0f;
    private const float Idletime = 3.0f;
    private float RoamingTimer;

    private Animator animator;

    // 죽은 몬스터가 사라지는데 걸리는 시간
    private float monsterDisappearingTime = 3f;
    private const float gravityValue = 15f;

    // 애니메이션에 따른, 이동 속도변화에 필요한 상수들
    private const float RoamingSpeedMultiplier = 2.0f;
    private const float DashAttackSpeedMultiplier = 4.5f;
    private const float ChasingSpeedMultiplier = 2.5f;

    private AttackArea OrcWeapon;

    private void Start()
    {
        monsterAdpt = GetComponent<MonsterAdapter>();
        monsterTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        nvAgent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();

        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        AIState = MonsterState.Idle;
        CheckingTime = new WaitForSeconds(0.2f);

        RoamingTimer = 0;
        OrcWeapon = GetComponentInChildren<AttackArea>();

        StartCoroutine(this.CheckMonsterAI());
    }

    IEnumerator CheckMonsterAI()
    {
        while (IsDied == false)
        {
            // 몬스터의 AI 상태는 일정 시간 (CheckingTime) 을 두고 변화함.
            yield return CheckingTime;

            // 데미지를 받는 State는 OnTriggerEvent로 따로 처리한다
            if (AIState == MonsterState.Damaged || AIState == MonsterState.Airbone)
            {
                yield return null;
            }

            // 데미지를 받고 있는 상태가 아닐 때
            else
            {
                float DistanceFromPlayer = Vector3.Distance(playerTr.position, monsterTr.position);

                // 플레이어가 탐지 거리 내로 들어오면 추적 시작
                if ((DistanceFromPlayer < detectionDistance && DistanceFromPlayer > dashAttackMaxDistance) ||
                    (DistanceFromPlayer > attackDistance && DistanceFromPlayer < dashAttackMinDistance))
                {
                    AIState = MonsterState.Chasing;
                }

                // 플레이어가 애매한 거리에 있으면 대쉬 공격으로 거리를 좁히며 공격
                else if (DistanceFromPlayer < dashAttackMaxDistance && DistanceFromPlayer > dashAttackMinDistance)
                {
                    AIState = MonsterState.DashAttacking;
                }

                // 플레이어가 공격 거리 내로 들어오면 공격 시작
                else if (DistanceFromPlayer < attackDistance)
                {
                    AIState = MonsterState.Attacking;
                }

                // 플레이어와 먼 거리에 있다면, Idle 상태로 (3초) 대기하다, RoamingTime 만큼 (6초) Patrol Area를 랜덤한 방향으로 순찰하는 것을 반복
                else
                {
                    if (AIState != MonsterState.Idle)
                    {
                        AIState = MonsterState.Roaming;
                    }
                }
            }

            // 스턴 상태는 별개로 처리
            if (IsStuned == true && IsGrounded == true)
            {
                AIState = MonsterState.Stun;
            }

            if (monsterAdpt.monster.monsterStatus.currentHP < 0)
            {
                AIState = MonsterState.Death;
            }
        }
    }

    private void Update()
    {
        CheckGroundStatus();

        HandleAttackEvent();

        animator.SetBool("OnGround", IsGrounded);

        desireVelocity.x = nvAgent.desiredVelocity.x;
        desireVelocity.z = nvAgent.desiredVelocity.z;

        if (IsGrounded == false)
        {
            // 중력 처리 (불완전할 수 있음에 주의)
            desireVelocity.y += Vector3.down.y * gravityValue * Time.smoothDeltaTime;
        }

        else
        {
            desireVelocity.y = Vector3.down.y;
        }

        #region Action Change by Animation Play

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") |
            animator.GetCurrentAnimatorStateInfo(0).IsName("Emerge") |
            animator.GetCurrentAnimatorStateInfo(0).IsName("Down") |
            animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp") |
            animator.GetCurrentAnimatorStateInfo(0).IsName("Wait") |
            animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            desireVelocity.x = 0;
            desireVelocity.z = 0;
        }

        #endregion

        #region Action Change by AI State
        switch (AIState)
        {
            case MonsterState.Airbone:
                {
                    animator.SetBool("IsAirDamaged", true);
                    break;
                }
            case MonsterState.Attacking:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == true &&
                        animator.GetCurrentAnimatorStateInfo(0).IsName("Down") != true)
                    {
                        monsterTr.LookAt(playerTr);
                        nvAgent.SetDestination(playerTr.position);
                    }
                    nvAgent.ResetPath();

                    if (animator.GetInteger("AttackType") == 0)
                    {
                        RandomDecideAttackType();
                    }

                    animator.SetBool("IsAttacking", true);
                    break;
                }
            case MonsterState.Chasing:
                {
                    nvAgent.ResetPath();
                    nvAgent.SetDestination(playerTr.position);

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down") != true)
                    {
                        monsterTr.LookAt(playerTr);
                        controller.Move(desireVelocity.normalized * Time.deltaTime * monsterAdpt.monster.Speed * ChasingSpeedMultiplier);
                        nvAgent.velocity = controller.velocity;
                    }
                    animator.SetBool("IsChasing", true);

                    break;
                }
            case MonsterState.Damaged:
                {
                    nvAgent.ResetPath();
                    monsterTr.LookAt(playerTr);

                    animator.SetBool("IsDamaged", true);

                    break;
                }

            case MonsterState.DashAttacking:
                {
                    monsterTr.LookAt(playerTr);
                    nvAgent.SetDestination(playerTr.position);

                    controller.Move(desireVelocity.normalized * Time.deltaTime * monsterAdpt.monster.Speed * DashAttackSpeedMultiplier);
                    nvAgent.velocity = controller.velocity;

                    animator.SetBool("IsDashAttacking", true);

                    break;
                }
            case MonsterState.Death:
                {
                    IsDied = true;
                    animator.SetBool("IsDied", true);
                    Invoke("DeactivateMonster", monsterDisappearingTime);
                    break;
                }

            case MonsterState.Idle:
                {
                    RoamingTimer += Time.deltaTime;

                    animator.SetBool("IsChasing", false);
                    animator.SetBool("IsIdle", true);

                    if (RoamingTimer > Idletime)
                    {
                        movingDirection = RandomDecideRoamingDirection();
                        nextMovingDirection = RandomDecideRoamingDirection();
                        RoamingTimer = 0;
                        AIState = MonsterState.Roaming;
                    }

                    nvAgent.ResetPath();

                    break;
                }

            case MonsterState.Roaming:
                {
                    RoamingTimer += Time.deltaTime;

                    animator.SetBool("IsChasing", false);
                    animator.SetBool("IsIdle", false);

                    if (RoamingTimer > RoamingTime)
                    {
                        RoamingTimer = 0;
                        AIState = MonsterState.Idle;
                    }

                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Roaming"))
                    {
                        monsterTr.LookAt(movingDirection);
                        nvAgent.ResetPath();
                        nvAgent.SetDestination(movingDirection);

                        controller.Move(desireVelocity.normalized * Time.deltaTime * monsterAdpt.monster.Speed * ChasingSpeedMultiplier);
                        nvAgent.velocity = controller.velocity;

                        if (isAtTargetLocation(nvAgent, movingDirection, nvAgent.stoppingDistance))
                        {
                            movingDirection = nextMovingDirection;
                        }
                    }

                    animator.SetBool("IsRoaming", true);

                    break;
                }

            case MonsterState.Stun:
                {
                    nvAgent.ResetPath();

                    animator.SetBool("IsStunned", true);
                    break;
                }
        }

        #endregion

    }

    // 지상의 몬스터가 Roaming할 방향을 난수 생성으로 결정
    private Vector3 RandomDecideRoamingDirection()
    {
        float x = Random.Range(patrolArea.minX, patrolArea.maxX);
        float z = Random.Range(patrolArea.minZ, patrolArea.maxZ);

        return new Vector3(x, 0, z);

    }

    // 공격할 스킬을 결정
    private void RandomDecideAttackType()
    {
        animator.SetInteger("AttackType", UnityEngine.Random.Range(1, 5));
    }

    private void DeactivateMonster()
    {
        gameObject.active = false;
    }

    // remaining Distance가 0이 아닐 상황에서도, 왜 계속 0인지 이해가 안 되서, 구글에 검색하다 찾음
    private bool isAtTargetLocation(NavMeshAgent navMeshAgent, Vector3 moveTarget, float minDistance)
    {
        float dist;

        //-- If navMeshAgent is still looking for a path then use line test
        if (navMeshAgent.pathPending)
        {
            dist = Vector3.Distance(transform.position, moveTarget);
        }
        else
        {
            dist = navMeshAgent.remainingDistance;
        }

        return dist <= minDistance;
    }

    private void HandleAttackEvent()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            OrcWeapon.OnAttack();
        }

        else
        {
            OrcWeapon.OffAttack();
        }

    }

    private void Damaged(Damage damage)
    { 
        if (IsDied == true)
        {
            return;
        }

        if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsTag("DamageAttack"))
        {
            animator.SetTrigger("Damaged");
        }

        else if (damage.attacker.GetCurrentAnimatorStateInfo(0).IsTag("DownAttack"))
        {
            animator.Play("Down");
        }

    }

    private void CheckGroundStatus()
    {
        // 왜인지 모르겠지만, MonsterControl 클래스의 CharacterController.isGrounded 가 제대로 작동하지 않아 
        // 새 함수를 사용함
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))
        {
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }
    }
}