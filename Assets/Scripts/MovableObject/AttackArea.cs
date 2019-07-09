using UnityEngine;

// 아래 스크립트의 작성은 http://www.yes24.com/24/goods/27894042 도서를 참고함

/// <summary>
/// 공격이 상대에게 닿는 영역에 스크립트를 붙여 사용.
/// </summary>

namespace UnityChanRPG
{
    public class AttackArea : MonoBehaviour
    {
        private Status attackerStatus;
        private new Collider collider;
        private Animator attacker;

        public delegate void HandleAttackEvent(ref Damage damage);
        public HandleAttackEvent handleAttackEvent;

        private void Awake()
        {
            attackerStatus = GetComponentInParent<Status>();
            gameObject.GetComponentInParent<Animator>();
            collider = GetComponent<Collider>();
            attacker = GetComponentInParent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            // 어떤 경우에도, 한 공격 모션에 데미지가 한 번만 들어가게 한다.
            // 그렇게 하기 위해 Animator 파라미터로 'DamagedProcessed' 를 만들어 사용함
            // 하지만, 이렇게 하면 플레이어가 다수의 몬스터를 한 번에 공격할 수 없으므로 수정이 필요함
            if (attacker.GetBool("DamagedProcessed") == true) return;

            HitArea hitArea = other.gameObject.GetComponent<HitArea>();

            if (hitArea == null) return;

            Damage damage = attackerStatus.DecideDamageValue(attacker, other.gameObject.GetComponent<Animator>());

            this.handleAttackEvent(ref damage);

            hitArea.Damaged(damage);
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