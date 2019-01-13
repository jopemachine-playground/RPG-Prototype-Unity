using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int currentHP;

    public int ID;
    public string Name;
    public string Description;
    public int MaxHP;
    public int AttackValue;
    public int DefenceValue;
    public int ExpeienceValue;
    public int Speed;
    public MonsterType Type;
    public GameObject MonsterModel;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = MaxHP;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Damaged(int playerAtk)
    {
        int resultDamage;

        if (DefenceValue >= playerAtk)
        {
            resultDamage = 1;
        }
        else
        {
            resultDamage = playerAtk - DefenceValue;
        }

        currentHP -= resultDamage;

        if (currentHP <= 0)
        {
            this.gameObject.SetActive(false);
        }

    }


}
