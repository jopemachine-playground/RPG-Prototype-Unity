using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

/// <summary>
/// 몬스터에 대한 정보를 json 파일에서 가져옴. 
/// ItemParser와 마찬가지로, 필드 위 몬스터들을 오브젝트 풀링으로 생성하기 위해, 이 클래스를 거침
/// </summary>

namespace UnityChanRPG
{
    class MonsterParser : MonoBehaviour
    {
        public static MonsterParser mInstance;

        public List<Monster> entireMonsterList = new List<Monster>();

        public List<GameObject> monsterModel;

        private void Awake()
        {
            if (mInstance != null)
            {
                Debug.Assert(false, "MonsterParser Class is Singleton.");
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                mInstance = this;
                StartCoroutine("LoadCoroutine");
            }
        }

        #region Data Parsing and Load
        IEnumerator LoadCoroutine()
        {
            string monsterJsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/MonsterData.json");

            string monsterDropItemJsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/MonsterDropItem.json");

            JsonData monsterData = JsonMapper.ToObject(monsterJsonString);

            JsonData monsterDropItemData = JsonMapper.ToObject(monsterDropItemJsonString);

            Debug.Assert(monsterData != null, "monster Data == null");

            Debug.Assert(monsterDropItemData != null, "monsterDropItemData == null");

            ParsingJsonMonster(monsterData);

            ParsingJsonMonsterDropItem(monsterDropItemData);

            for (int i = 0; i < entireMonsterList.Count; i++)
            {
                transform.Find(entireMonsterList[i].ID + "").gameObject.AddComponent<MonsterAdapter>();
                transform.Find(entireMonsterList[i].ID + "").gameObject.GetComponent<MonsterAdapter>().monster = entireMonsterList[i];
            }

            Calc_ItemDropProbAccum();

            yield return null;
        }

        private void ParsingJsonMonster(JsonData monsterData)
        {
            for (int i = 0; i < monsterData.Count; i++)
            {
                entireMonsterList[i].ID = (int)(monsterData[i]["ID"]);
                entireMonsterList[i].Name = (monsterData[i]["Name"]).ToString();
                entireMonsterList[i].Description = (monsterData[i]["Description"]).ToString();
                entireMonsterList[i].Type = (MonsterType)((int)(monsterData[i]["MonsterType"]));
                entireMonsterList[i].MaxHP = (int)(monsterData[i]["MaxHP"]);
                entireMonsterList[i].ExperienceValue = (int)(monsterData[i]["ExperienceValue"]);
                entireMonsterList[i].Speed = (int)(monsterData[i]["Speed"]);
                entireMonsterList[i].MonsterModel = monsterModel[i];

                entireMonsterList[i].monsterStatus = transform.Find(entireMonsterList[i].ID + "").gameObject.GetComponent<Status>();
                entireMonsterList[i].monsterStatus.currentHP = entireMonsterList[i].MaxHP;
                entireMonsterList[i].monsterStatus.AttackValue = (int)(monsterData[i]["AttackValue"]);
                entireMonsterList[i].monsterStatus.DefenceValue = (int)(monsterData[i]["DefenceValue"]);

            }

        }

        private void ParsingJsonMonsterDropItem(JsonData monsterDropItemData)
        {
            for (int j = 0; j < monsterDropItemData.Count; j++)
            {
                int index = getMonsterIndex((int)(monsterDropItemData[j]["MonsterID"]));

                entireMonsterList[index].monsterDropItems.Add(new MonsterDropItem
                (
                    (int)(monsterDropItemData[j]["ItemID"]),
                    ((int)(monsterDropItemData[j]["DropProb"]) / 100f),
                    (int)(monsterDropItemData[j]["DropMinNum"]),
                    (int)(monsterDropItemData[j]["DropMaxNum"]))
                );
            }
        }

        private void Calc_ItemDropProbAccum()
        {
            for (int i = 0; i < entireMonsterList.Count; i++)
            {
                entireMonsterList[i].dropItemProbAccum = new float[entireMonsterList[i].monsterDropItems.Count + 1];

                for (int j = 1; j < entireMonsterList[i].dropItemProbAccum.Length; j++)
                {
                    entireMonsterList[i].dropItemProbAccum[j] = entireMonsterList[i].monsterDropItems[j - 1].DropProb;
                    entireMonsterList[i].dropItemProbAccum[j] += entireMonsterList[i].dropItemProbAccum[j - 1];
                }
            }
        }

        #endregion

        private int getMonsterIndex(int id)
        {
            for (int i = 0; i < entireMonsterList.Count; i++)
            {
                if (entireMonsterList[i].ID == id)
                    return i;
            }
            return -1;
        }

        public Monster getMonsterByID(int id)
        {
            for (int i = 0; i < entireMonsterList.Count; i++)
            {
                if (entireMonsterList[i].ID == id)
                    return entireMonsterList[i].getCopy();
            }
            return null;
        }

        #region UNUSED CODE
        // * 몬스터는 아이템과 달리 내부에 만들어줄 객체가 너무 많기 때문에, 유니티에서 컴파일 시점에 만들어 둠. 
        // * 아래 메서드는 몬스터 풀을 런타임에 만들어주지만, 안의 모든 컴포넌트를 모두 코드로 만들기엔 너무 많고, 상세히 설정할 수 없을 것 같아 포기함

        //private void MakeMonsterPool(int _index, int _ID)
        //{
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.name = "" + _ID;
        //    cube.SetActive(false);
        //    Destroy(cube.GetComponent<BoxCollider>());
        //    cube.AddComponent<Rigidbody>();
        //    cube.AddComponent<CapsuleCollider>();
        //    cube.GetComponent<CapsuleCollider>().height = monsterModel[_index].GetComponent<CapsuleCollider>().height;
        //    cube.GetComponent<CapsuleCollider>().radius = monsterModel[_index].GetComponent<CapsuleCollider>().radius;
        //    cube.GetComponent<CapsuleCollider>().center = monsterModel[_index].GetComponent<CapsuleCollider>().center;

        //    // Monster Pool 아래에 Monster 들을 미리 생성
        //    cube.transform.parent = GameObject.FindGameObjectWithTag("MonsterPool").transform;

        //    // Renderer 내 Material 배열 교체.
        //    cube.GetComponent<MeshRenderer>().materials = monsterModel[_index].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;
        //    cube.GetComponent<MeshFilter>().mesh = monsterModel[_index].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
        //}
        #endregion

        #region Make Monster Pool

        public void GenerateMonsterPool(int _ID, SpawnPoint _SpawnPoint)
        {
            Transform obj = GameObject.FindGameObjectWithTag("Parser").transform.Find("Monster Parser").Find("" + _ID);
            Transform tr = Instantiate(obj, Vector3.zero, Quaternion.identity);
            tr.gameObject.SetActive(false);
            tr.gameObject.name = _ID + "";
            tr.gameObject.GetComponent<MonsterControl>().spawnPoint = _SpawnPoint;
            tr.parent = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("FieldSpawnMonster Pool").Find(_SpawnPoint.gameObject.name);
        }

        #endregion

    }
}
