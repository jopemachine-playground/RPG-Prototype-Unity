// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using UnityEngine;

namespace UnityChanRPG
{
    // 원거리 공격 투사체. 마력 소모
    public class Bomb : MonoBehaviour
    {

        public Animator attacker;

        public Status playerStatus;

        public LayerMask terrainLayer;

        public new Rigidbody rigidbody;

        // 폭발 파티클의 ID
        public int bombExplodeParticleID = 10006;

        private void Awake() {
            var player = GameObject.FindGameObjectWithTag("Player");

            playerStatus = player.GetComponent<Status>();
            attacker = player.GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody>();
            terrainLayer = LayerMask.NameToLayer("Terrain");

            Invoke("destroyByTime" , 6);
        }

        private void destroyByTime() {
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision other)
        {
            // terrainLayer는 구하지 않음
            Collider[] targets = Physics.OverlapSphere(transform.position, 5f, terrainLayer);

            // 지형, 몬스터와 충돌한 경우 그 자리에서 폭발함
            if (other.gameObject.layer == 13 || other.gameObject.layer == 9) {

                for (int i = 0; i < targets.Length; i++)
                {
                    var hitArea = targets[i].gameObject.GetComponent<HitArea>();

                    if (hitArea == null) continue;

                    Damage damage = playerStatus.DecideDamageValue(attacker, targets[i].gameObject.GetComponent<Animator>());

                    damage.EmittingParticleID = bombExplodeParticleID;

                    hitArea.Damaged(damage);

                }

                // 폭발한 자리에 폭발 파티클 생성한 후 폭탄 비활성화
                ParticlePool.attackParticle.CallParticle(bombExplodeParticleID, transform.position);
                gameObject.SetActive(false);
            }
        }
    }

}