// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 랜덤으로 스폰 위치와 아이템이나 몬스터의 종류를 결정해 스폰함. (또는 일정 확률로 스폰하지 않음.)
/// 스폰 위치와, 스폰되는 아이템(이나 몬스터) 들은 유니티 에디터에서 넣을 것
/// SpawnManager에서 스폰하는 오브젝트의 갯수는 maxSpawnNumber를 넘을 수 없기 때문에 처음 정해진 크기에서 더 늘어나지 않지만, 아이템의 경우
/// 몬스터 사냥으로 드랍되는 아이템은 생성 한계가 정해져 있지 않으므로, ItemPool에서 따로 풀링해 관리함
/// </summary>

namespace UnityChanRPG
{
    public class SpawnManager : MonoBehaviour
    {
        // 유니티 에디터에서 지정. spawnArea가 다른 SpawnManager를 여러 개 지정함으로써 
        // 여러 장소에서 동시에 오브젝트가 생성되도록 할 수 있음. 하지만, Area가 다르다면 spawn Point를 공유하지 않도록 해야,
        // 같은 장소에서 동시에 여러 몬스터가 생성되는 현상을 방지할 수 있을 것임에 주의
        public GameObject spawnArea;

        // 해당 위치에 스폰할 대상의 종류 (현재로선, 아이템과 몬스터)
        [Serializable]
        public enum spawnerType
        {
            FieldSpawnMonster,
            FieldSpawnItem
        }

        public spawnerType type;

        private delegate void GenerateThing(int _ID, SpawnPoint _SpawnPoint);
        private event GenerateThing GenerateObject;

        private GameObject Pool;

        // 최대한 스폰될 수 있는 갯수와 현재 오브젝트의 갯수 (현재 오브젝트의 ID를 가리지 않고 갯수를 셈에 주의)
        public int maxSpawnNumber;
        public int FieldObjectsCount;

        // 스폰위치는 유니티 에디터에서 정해 넣을 것. WaitingTime은 maxSpawnNumber보다 FieldObjectsCount가 작을 때 스폰할 때 걸리는 시간
        private WaitForSeconds waitingTime;
        public SpawnPoint[] SpawnPoint;
        public float WaitingTime;

        private void Start()
        {
            waitingTime = new WaitForSeconds(WaitingTime);
            SpawnPoint = GetComponentsInChildren<SpawnPoint>();

            if (SpawnPoint.Length > 0)
            {
                switch (type)
                {
                    case spawnerType.FieldSpawnItem:
                        GenerateObject += ItemParser.mInstance.GenerateFieldSpawnItemPool;
                        Pool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("FieldSpawnItem Pool").gameObject;
                        break;

                    case spawnerType.FieldSpawnMonster:
                        GenerateObject += MonsterParser.mInstance.GenerateMonsterPool;
                        Pool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("FieldSpawnMonster Pool").gameObject;
                        break;
                }
            }

            // 스폰 포인트들의 스폰 오브젝트들의 ID를 확인해 오브젝트 풀링으로 생성
            // 스폰 포인트끼리 같은 ID의 오브젝트를 갖고 있다면 그 수 만큼 중복으로 생성한다.
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                for (int j = 0; j < SpawnPoint[i].SpawnObjects.Count; j++)
                {
                    for (int k = 0; k < SpawnPoint[i].SpawnObjects[j].PoolingValue; k++)
                    {
                        GenerateObject(SpawnPoint[i].SpawnObjects[j].ID, SpawnPoint[i]);
                    }
                }
            }

            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                for (int j = 1; j < SpawnPoint[i].SpawnObjects.Count + 1; j++)
                {
                    SpawnPoint[i].SpawnProbAccum = new float[SpawnPoint[i].SpawnObjects.Count + 1];

                    SpawnPoint[i].SpawnProbAccum[j] = (SpawnPoint[i].SpawnObjects[j - 1].SpawnProbablity);

                    SpawnPoint[i].SpawnProbAccum[j] += SpawnPoint[i].SpawnProbAccum[j - 1];
                }
            }

            StartCoroutine(this.SpawnOnField(type));
        }

        // ID가 같고 비활성화된 게임 오브젝트를 찾는다. 없다면 null 반환
        private GameObject SearchObject(int ID)
        {
            for (int i = 0; i < Pool.transform.childCount; i++)
            {
                Transform point = Pool.transform.GetChild(i);

                for (int j = 0; j < point.transform.childCount; j++)
                {
                    Transform child = point.transform.GetChild(j);

                    if (child.name == ID + "")
                    {
                        if (child.gameObject.activeSelf == false)
                        {
                            return child.gameObject;
                        }
                    }
                }
            }
            return null;
        }

        private IEnumerator SpawnOnField(spawnerType type)
        {
            string type_string = Enum.GetName(type.GetType(), type);

            GameObject fieldSpawnPool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find(type_string + " Pool").gameObject;

            while (true)
            {
                // 아래 같은 코드가 렉의 원인이 됨. 실수로 FindGameObjectsWithTag를 무한루프를 도는 코루틴, Update 안에 넣지 않게 조심하자 
                // 코드가 많기 때문에 한 번 넣어놓으면 찾기가 힘들어질 수 있다..
                // FieldObjectsCount = GameObject.FindGameObjectsWithTag(type_string).Length;

                FieldObjectsCount = 0;

                for (int i = 0; i < fieldSpawnPool.transform.childCount; i++)
                {
                    Transform point = fieldSpawnPool.transform.GetChild(i);

                    for (int j = 0; j < point.childCount; j++)
                    {
                        if (point.GetChild(j).gameObject.activeSelf == true)
                        {
                            FieldObjectsCount++;
                        }
                    }
                }

                if (FieldObjectsCount < maxSpawnNumber)
                {
                    yield return waitingTime;

                    int placeIndex = UnityEngine.Random.Range(0, SpawnPoint.Length);

                    float probability = (UnityEngine.Random.Range(0f, 10000f)) / 10000f;

                    int ID = ReturnID(probability, placeIndex, 0.0001f);

                    if (ID == -1)
                    {
                        yield return null;
                    }
                    else {
                        Spawn(placeIndex, ID);
                    }

                }
                else
                {
                    yield return waitingTime;
                }
            }
        }

        public GameObject Spawn(int placeIndex, int ID)
        {
            GameObject spawnObj = SearchObject(ID);

            if (spawnObj != null)
            {
                spawnObj.transform.position = SpawnPoint[placeIndex].transform.position;
                spawnObj.SetActive(true);
                return spawnObj;
            }

            Debug.Assert(false, "Wrong Spawner ID, this ID: " + ID);
            return null;

        }

        // 이 ReturnID와 MonsterControl.cs의 DropItem.cs는 다음에라도 반드시 리팩토링 하자
        private int ReturnID(float _prob, int _placeIndex, float _minProbUnit)
        {

            Debug.Assert(_prob <= 1, "Error Occur - ReturnID in SpawnManager.cs");

            float[] minProb = new float[SpawnPoint[_placeIndex].SpawnObjects.Count];

            for (int i = 1; i < SpawnPoint[_placeIndex].SpawnObjects.Count + 1; i++)
            {
                minProb[i - 1] =

                    Math.Abs(SpawnPoint[_placeIndex].SpawnProbAccum[i] - _prob - _minProbUnit) < Math.Abs(SpawnPoint[_placeIndex].SpawnProbAccum[i - 1] - _prob) ?

                    Math.Abs(SpawnPoint[_placeIndex].SpawnProbAccum[i] - _prob - _minProbUnit) : Math.Abs(SpawnPoint[_placeIndex].SpawnProbAccum[i - 1] - _prob);

                // 확률의 합은 1보다 작아야 함
                Debug.Assert(SpawnPoint[_placeIndex].SpawnProbAccum[i] <= 1, "Error Occur - ReturnID in SpawnManager.cs");
            }

            int Index = 0;
            float result = 1.0f;

            for (int i = 0; i < minProb.Length; i++)
            {
                if (result > minProb[i])
                {
                    result = minProb[i];
                    Index = i;
                }
            }

            //Debug.Log("Index: " + Index);
            //Debug.Log("Result: " + result);
            //Debug.Log("_prob: " + _prob);
            //Debug.Log("_placeIndex: " + _placeIndex);
            //Debug.Log("SpawnPoint[_placeIndex].SpawnProbAccum.Length - 1: " + (SpawnPoint[_placeIndex].SpawnProbAccum.Length - 1));


            if (_prob > SpawnPoint[_placeIndex].SpawnProbAccum[SpawnPoint[_placeIndex].SpawnProbAccum.Length - 1])
            {
                return -1;
            }

            return SpawnPoint[_placeIndex].SpawnObjects[Index].ID;
        }
    }
}