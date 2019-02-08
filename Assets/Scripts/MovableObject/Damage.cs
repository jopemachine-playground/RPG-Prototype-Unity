using UnityEngine;

public class Damage
{
    public int value;
    public bool IsFatalBlow;

    public Animator attacker;
    public Animator attackee;

    public Damage(int _value, bool _IsFatalBlow)
    {
        value = _value;
        IsFatalBlow = _IsFatalBlow;
    }
}

