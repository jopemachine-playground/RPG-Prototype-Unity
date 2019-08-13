// ==============================+===============================================================
// @ Author : jopemachine
// @ Ref URLs : 
// @     1. http://www.yes24.com/24/goods/27894042
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    [RequireComponent(typeof(Status))]
    [RequireComponent(typeof(CharacterController))]
    public class HitArea : MonoBehaviour
    {
        private Status status;

        private CharacterController hitPoint;

        public delegate void HandleAttackedEvent(Damage damage);
        public HandleAttackedEvent handleAttackedEvent;

        private void OnEnable()
        {
            status = GetComponent<Status>();
            hitPoint = GetComponent<CharacterController>();
        }

        public void Damaged(Damage damage)
        {
            // attacker와 attackee가 같은 태그라면, 예를 들어 몬스터가 같은 몬스터를 공격하는 경우
            // 데미지 처리를 하지 않는다.

            // Attacker의 Collider와 Attack Area와의 충돌 이벤트는 Layer에서의 셋팅으로, 충돌이 감지되지 않게 한다.
            // (여기서 따로 처리할 필요 없음)

            if (damage.attacker.gameObject.tag == damage.attackee.gameObject.tag) return;

            status.CalculateDamage(damage);

            handleAttackedEvent(damage);

            if (damage.EmittingParticleID != 0)
            {
                ParticlePool.attackParticle.CallParticle(damage.EmittingParticleID, transform.position + hitPoint.center);
            }
        }


    }

}