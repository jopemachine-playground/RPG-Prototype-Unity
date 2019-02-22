using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 아래 스크립트의 작성은 http://www.yes24.com/24/goods/27894042 도서를 참고함

namespace UnityChanRPG
{
    public class AttackArea : MonoBehaviour
    {
        private Status attackerStatus;
        private new Collider collider;
        private Animator attacker;
        private IInteractAble attackerObj;

        private void Awake()
        {
            attackerStatus = GetComponentInParent<Status>();
            gameObject.GetComponentInParent<Animator>();
            collider = GetComponent<Collider>();
            attacker = GetComponentInParent<Animator>();

            attackerObj = GetComponentInParent<MonsterControl>();

            if (attackerObj == null)
            {
                attackerObj = GetComponentInParent<PlayerControl>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // 어떤 경우에도, 한 공격 모션에 데미지가 한 번만 들어가게 한다.
            // 그렇게 하기 위해 Animator 파라미터로 'DamagedProcessed' 를 만들어 사용함
            if (attacker.GetBool("DamagedProcessed") == true) return;

            HitArea hit = other.gameObject.GetComponent<HitArea>();

            if (hit == null) return;

            Damage damage = attackerStatus.DecideDamageValue(attacker, other.gameObject.GetComponent<Animator>());

            attackerObj.HandleAttackParticle(ref damage);

            // PlayerControl이나 MonsterControl 둘 중 하나의 Damaged와, HitArea의 Damaged를 호출한다
            other.SendMessage("Damaged", damage);
        }

        // 각 MoveableObject들은 공격 애니메이션의 진행도에 따라 OnAttack, OffAttack을 호출해 처리하도록 한다
        public void OnAttack()
        {
            collider.enabled = true;
        }

        public void OffAttack()
        {
            collider.enabled = false;
        }
    }

}