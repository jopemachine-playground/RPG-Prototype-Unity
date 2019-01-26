using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monster가 Monobehavior를 상속할 수 없으므로 Adapter를 만듬
public class MonsterAdapter: MonoBehaviour
{
    public Monster monster;

    public int ID;

    public void Start()
    {
        monster = MonsterPool.mInstance.getMonsterByID(ID);
    }

}

