using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterControl : MonoBehaviour
{
    private Monster monster;

    public Transform monsterTr;
    public Transform playerTr;
    public NavMeshAgent nvAgent;
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

    // 바닥 감지 거리
    private float GroundCheckDistance;
    // 공격 거리
    public float attackDistance;
    // 탐지 거리
    public float detectionDistance;
    // 대쉬 어택 거리
    public float dashAttackMinDistance;
    public float dashAttackMaxDistance;

    // Roaming , Idle을 반복하는 시간을 재기 위한 타이머
    private const float RoamingTime = 6.0f;
    private const float Idletime = 3.0f;
    private float RoamingTimer;

    private Animator animator;

    private void Start()
    {
        monster = GetComponent<MonsterAdapter>().monster;
        monsterTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = GetComponent<NavMeshAgent>();
        AIState = MonsterState.Idle;
        CheckingTime = new WaitForSeconds(0.2f);

        attackDistance = 2.0f;
        dashAttackMinDistance = 10.0f;
        dashAttackMaxDistance = 12.0f;
        detectionDistance = 14.5f;

        RoamingTimer = 0;

        StartCoroutine(this.CheckMonsterAI());
        StartCoroutine(this.MonsterAction());
    }

    IEnumerator CheckMonsterAI()
    {
        while (IsDied == false)
        {
            yield return CheckingTime;

            if (monster.currentHP < 0)
            {
                AIState = MonsterState.Death;
                IsDied = true;
            }

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
        }
    }

    IEnumerator MonsterAction()
    {
        while (IsDied == false)
        {
            CheckGroundStatus();

            animator.SetBool("OnGround", IsGrounded);

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
                        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") != false && 
                            animator.GetCurrentAnimatorStateInfo(0).IsName("Down") != true)
                        {
                            monsterTr.LookAt(playerTr);
                            nvAgent.SetDestination(playerTr.position);
                        }
                        nvAgent.ResetPath();
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

                        animator.SetBool("IsDashAttacking", true);

                        break;
                    }
                case MonsterState.Death:
                    {
                        animator.SetBool("IsDied", true);
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

            #region Action Change by Animation Play

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                nvAgent.velocity = Vector3.zero;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Emerge"))
            {
                nvAgent.velocity = Vector3.zero;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down") || animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
            {
                nvAgent.velocity = Vector3.zero;
            }

            // 대쉬 어택의 velocity를 높이려면, 애니메이션 시간을 줄여야 한다.
            // 공격 속도를 조절하려면 Idle의 시간을 조절해야 할 듯?
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack"))
            {
                // nvAgent.velocity *= 1.01f;
            }

            #endregion
            yield return null;

        }
    }

    // 지상의 몬스터가 Roaming할 방향을 난수 생성으로 결정
    private Vector3 RandomDecideRoamingDirection()
    {
        float x = Random.Range(patrolArea.minX, patrolArea.maxX);
        float z = Random.Range(patrolArea.minZ, patrolArea.maxZ);

        return new Vector3(x, 0, z);

    }


    private void CheckGroundStatus()
    {
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

    public void OnTriggerEnter(Collider coll)
    {
        if (Player.mInstance.state == PlayerState.GroundAttack1 && coll.gameObject.tag == "PlayerLeftLeg")
        {
            animator.SetTrigger("Damaged");
            DamageIndicator.mInstance.CallFloatingText(monsterTr, monster.Damaged(Player.mInstance.DecideAttackValue()));
        }

        if (Player.mInstance.state == PlayerState.GroundAttack2 && coll.gameObject.tag == "PlayerRightLeg")
        {
            animator.SetTrigger("Damaged");
            DamageIndicator.mInstance.CallFloatingText(monsterTr, monster.Damaged(Player.mInstance.DecideAttackValue()));
        }

        if (Player.mInstance.state == PlayerState.GroundAttack3 && coll.gameObject.tag == "PlayerLeftLeg")
        {
            animator.SetBool("IsDown", true);
            DamageIndicator.mInstance.CallFloatingText(monsterTr, monster.Damaged(Player.mInstance.DecideAttackValue()));
        }

    }

}