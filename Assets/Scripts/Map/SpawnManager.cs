using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 랜덤으로 스폰 위치와 아이템이나 몬스터의 종류를 결정해 스폰함. (또는 일정 확률로 스폰하지 않음.)
/// 스폰 위치와, 스폰되는 아이템(이나 몬스터) 들은 유니티 에디터에서 넣을 것
/// SpawnManager에서 스폰하는 오브젝트의 갯수는 maxSpawnNumber를 넘을 수 없기 때문에 처음 정해진 크기에서 더 늘어나지 않지만, 아이템의 경우
/// 몬스터 사냥으로 드랍되어 일정 상황에서 maxSpawnNumber보다 높은 숫자를 가질 수 있고, 이 경우 두 배만큼 아이템을 미리 생성해 놓는다
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

        private void Awake()
        {
            waitingTime = new WaitForSeconds(WaitingTime);
            SpawnPoint = GetComponentsInChildren<SpawnPoint>();
        }

        private void Start()
        {
            if (SpawnPoint.Length > 0)
            {
                StartCoroutine(this.SpawnOnField(type));

                switch (type)
                {
                    case spawnerType.FieldSpawnItem:
                        GenerateObject += ItemParser.mInstance.GenerateItemPool;
                        Pool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("PickUp Item Pool").gameObject;
                        break;

                    case spawnerType.FieldSpawnMonster:
                        GenerateObject += MonsterParser.mInstance.GenerateMonsterPool;
                        Pool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("Monster Pool").gameObject;
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

            while (true)
            {
                FieldObjectsCount = GameObject.FindGameObjectsWithTag(type_string).Length;

                if (FieldObjectsCount < maxSpawnNumber)
                {
                    yield return waitingTime;

                    int index = UnityEngine.Random.Range(0, SpawnPoint.Length);

                    float probability = ((float)UnityEngine.Random.Range(0, 10000)) / 10000f;

                    int ID = ReturnID(probability, index, 0.0001f);

                    GameObject spawnObj = SearchObject(ID);

                    if (spawnObj != null)
                    {
                        spawnObj.transform.position = SpawnPoint[index].transform.position;
                        spawnObj.SetActive(true);
                    }

                }
                else
                {
                    yield return waitingTime;
                }
            }
        }

        private int ReturnID(float _prob, int _placeIndex, float _minProbUnit)
        {

            Debug.Assert(_prob <= 1, "Error Occur - ReturnID in SpawnManager.cs");

            float[] probAccum = new float[SpawnPoint[_placeIndex].SpawnObjects.Count + 1];

            float[] minProb = new float[SpawnPoint[_placeIndex].SpawnObjects.Count];

            for (int i = 1; i < SpawnPoint[_placeIndex].SpawnObjects.Count + 1; i++)
            {
                probAccum[i] = (SpawnPoint[_placeIndex].SpawnObjects[i - 1].SpawnProbablity);

                probAccum[i] += probAccum[i - 1];

                minProb[i - 1] =

                    Math.Abs(probAccum[i] - _prob - _minProbUnit) < Math.Abs(probAccum[i - 1] - _prob) ?

                    Math.Abs(probAccum[i] - _prob - _minProbUnit) : Math.Abs(probAccum[i - 1] - _prob);

                // 확률의 합은 1보다 작아야 함
                Debug.Assert(probAccum[i] <= 1, "Error Occur - ReturnID in SpawnManager.cs");
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
            return SpawnPoint[_placeIndex].SpawnObjects[Index].ID;
        }
    }
}