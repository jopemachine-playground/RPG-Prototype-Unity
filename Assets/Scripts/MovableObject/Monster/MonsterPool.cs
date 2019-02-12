using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

class MonsterPool : MonoBehaviour
{
    public static MonsterPool mInstance;

    public List<Monster> entireMonsterList = new List<Monster>();

    public List<GameObject> monsterModel;

    private void Awake()
    {
        if (mInstance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            mInstance = this;
        }

        // 파싱되어 있는 데이터 로드
        StartCoroutine("LoadCoroutine");
    }

    #region Data Parsing and Load
    IEnumerator LoadCoroutine()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/MonsterData.json");

        JsonData monsterData = JsonMapper.ToObject(JsonString);

        Debug.Assert(monsterData != null, "monster Data == null");

        ParsingJsonMonster(monsterData);

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

            // MakeMonsterPool(i, entireMonsterList[i].ID);
        }

    }
    #endregion

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
    // * 아래 메서드는 몬스터 풀을 런타임에 만들어주지만, 안의 모든 컴포넌트를 모두 코드로 만들기엔 너무 많은 것 같아 포기함.

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

    public void GenerateMonster()
    {

    }

}

