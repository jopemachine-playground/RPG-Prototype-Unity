// ==============================+===============================================================
// @ Author : jopemachine
// @ Issue : 
// @     현재 플레이어, 몬스터에 붙여 사용하고 있는, Status 스크립트. 같은 스크립트를 공유하므로, 몬스터의 currentHP가 변해도
// @     플레이어의 정보를 업데이트 하는 함수를 (필요 없는 경우에도) 호출한다는 단점이 있다.
// @     Status는 스스로를 초기화하지 않으므로, 다른 스크립트에서 초기화 해 사용해야 함
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace UnityChanRPG
{
    public class Status : MonoBehaviour
    {
        [SerializeField]
        private int currentHP;
        public int CurrentHP
        {
            get
            {
                return currentHP;
            }
            set
            {
                currentHP = value;

                if (PlayerInfoSystem.Instance != null)
                {
                    PlayerInfoSystem.Instance.PlayerInfoWindowUpdate();
                }
            }
        }

        [SerializeField]
        private int currentMP;
        public int CurrentMP
        {
            get
            {
                return currentMP;
            }
            set
            {
                currentMP = value;

                if (PlayerInfoSystem.Instance != null)
                {
                    PlayerInfoSystem.Instance.PlayerInfoWindowUpdate();
                }
            }
        }

        [SerializeField]
        private float stamina;
        public float Stamina
        {
            get
            {
                return stamina;
            }

            set
            {
                stamina = value;
                if (PlayerInfoSystem.Instance != null)
                {
                    PlayerInfoSystem.Instance.PlayerInfoWindowUpdate();
                }
            }
        }

        public int FatalBlowValue;
        public int FatalBlowProb;
        public int AttackValue;
        public int DefenceValue;

        // 몬스터가 활성화될 때 호출해, status를 초기화.
        public void StatusInit(int MaxHP, int MaxMP)
        {
            CurrentHP = MaxHP;
            CurrentMP = MaxMP;
        }

        // 방어력 속성들을 이용해 최종적인 데미지를 계산하고, 
        // UI에 입힌 데미지를 표시하며, 공격 이펙트를 불러와 재생한다 
        public void CalculateDamage(Damage damage)
        {
            float resultDamageFloat = damage.value * (damage.skillCoefficient / 100.0f);

            if (DefenceValue >= resultDamageFloat)
            {
                resultDamageFloat = 1;
            }
            else
            {
                resultDamageFloat -= DefenceValue;
            }

            int resultDamage = (int)(Mathf.Floor(resultDamageFloat));

            if (CurrentHP - resultDamage >= 0)
            {
                CurrentHP -= (int)(Mathf.Floor(resultDamage));
            }
            else
            {
                CurrentHP = 0;
            }

            DamageIndicator.mInstance.CallFloatingText(damage.SetDamageValue(resultDamage));

            damage.attacker.SetBool("DamagedProcessed", true);

        }

        // 공격 데미지 공식
        public Damage DecideDamageValue(Animator attacker, Animator attackee)
        {
            bool isFatalBlow;

            float minDamageValue = AttackValue;
            float maxDamageValue = 1.25f * AttackValue;

            float damageValue = UnityEngine.Random.Range(minDamageValue, maxDamageValue);

            if (isFatalBlow = DecideFatalBlow())
            {
                damageValue *= (FatalBlowValue / 100.0f);
            }

            return new Damage((int)(Mathf.Floor(damageValue)), isFatalBlow, attacker, attackee);
        }

        // 이번 공격이 치명타인지 결정
        public bool DecideFatalBlow()
        {
            int prob = UnityEngine.Random.Range(0, 100);

            if (prob > FatalBlowProb)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

    }

}