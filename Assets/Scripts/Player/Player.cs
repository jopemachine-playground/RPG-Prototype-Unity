using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : MonoBehaviour
{
    // PlayerInfo에 값을 저장하고, 읽어옴.
    [SerializeField]
    public string Name;
    [SerializeField]
    public int Money;
    [SerializeField]
    public int currentHP;
    [SerializeField]
    public int currentMP;
    [SerializeField]
    public int Level;
    [SerializeField]
    public int ExperienceValue;
    [SerializeField]
    public int AttackValueMultiplier;
    [SerializeField]
    public int DefenceValue;

    public int Damaged(int monsterAtk)
    {
        int resultDamage;

        if (DefenceValue >= monsterAtk)
        {
            resultDamage = 1;
        }
        else
        {
            resultDamage = monsterAtk - DefenceValue;
        }

        if (currentHP <= 0)
        {
            Destroy(this.gameObject);
        }

        return 0;
    }


}
