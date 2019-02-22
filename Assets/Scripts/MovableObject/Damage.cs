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