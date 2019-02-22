using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    public class Status : MonoBehaviour
    {
        public int currentHP;
        public int currentMP;
        public float stamina;

        public int FatalBlowValue;
        public int FatalBlowProb;
        public int AttackValue;
        public int DefenceValue;

        // 몬스터가 활성화될 때 호출해, status를 초기화.
        public void StatusInit(int MaxHP, int MaxMP)
        {
            currentHP = MaxHP;
            currentMP = MaxMP;
        }

        // 방어력 속성들을 이용해 최종적인 데미지를 계산하고, 
        // UI에 입힌 데미지를 표시하며, 공격 이펙트를 불러와 재생한다 
        public void CalculateDamage(Damage damage)
        {
            float resultDamageFloat = damage.value * (damage.skillCoefficient / 100);

            if (DefenceValue >= resultDamageFloat)
            {
                resultDamageFloat = 1;
            }
            else
            {
                resultDamageFloat -= DefenceValue;
            }

            int resultDamage = (int)(Mathf.Floor(resultDamageFloat));

            if (currentHP - resultDamage >= 0)
            {
                currentHP -= (int)(Mathf.Floor(resultDamage));
            }
            else
            {
                currentHP = 0;
            }

            DamageIndicator.mInstance.CallFloatingText(damage.SetDamageValue(resultDamage));

            damage.attacker.SetBool("DamagedProcessed", true);

        }

        // 공격 데미지 공식
        public Damage DecideDamageValue(Animator attacker, Animator attackee)
        {
            bool isFatalBlow;

            float minDamageValue = AttackValue;
            float maxDamageValue = 1.5f * AttackValue;

            float damageValue = UnityEngine.Random.Range(minDamageValue, maxDamageValue);

            if (isFatalBlow = DecideFatalBlow())
            {
                damageValue *= (FatalBlowValue / 100);
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