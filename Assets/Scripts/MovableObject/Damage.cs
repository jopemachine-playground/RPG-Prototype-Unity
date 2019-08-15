// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     데미지를 담는 자료구조. 공격이 들어갔는지의 판단을 Animator의 상태로 판단하기 때문에,
// @     attacker와 attackee의 Animator가 있어야 생성 가능.
// ==============================+===============================================================

using UnityEngine;

namespace UnityChanRPG
{
    public class Damage
    {
        public int value;
        public bool IsFatalBlow;

        public Animator attacker;
        public Animator attackee;

        public int EmittingParticleID = 0;
        public float skillCoefficient = 100f;

        public Damage(int _value, bool _IsFatalBlow, Animator _attacker, Animator _attackee)
        {
            value = _value;
            IsFatalBlow = _IsFatalBlow;
            attacker = _attacker;
            attackee = _attackee;
        }

        public Damage SetDamageValue(int _value)
        {
            value = _value;
            return this;
        }
    }

}