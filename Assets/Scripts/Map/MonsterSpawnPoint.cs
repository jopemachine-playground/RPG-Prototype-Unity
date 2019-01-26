using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MonsterSpawnPoint : MonoBehaviour
{
    private Transform point;

    public List<MonsterSpawnPoint> SpawnItemList = new List<MonsterSpawnPoint>();

    private void Awake()
    {
        point = this.GetComponent<Transform>();
    }

    [System.Serializable]
    public class SpawnMonster
    {
        public int ID;
        public float SpawnProbablity;
    }

}

