using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityChanRPG
{
    [System.Serializable]
    public class ItemSpawnPoint : MonoBehaviour
    {
        private Transform point;

        public List<SpawnItem> SpawnItemList = new List<SpawnItem>();

        private void Awake()
        {
            point = this.GetComponent<Transform>();
        }

        [System.Serializable]
        public class SpawnItem
        {
            public int ID;
            public float SpawnProbablity;

        }
    }
}