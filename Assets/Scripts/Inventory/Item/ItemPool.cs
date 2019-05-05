using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 몬스터 Pool을 뒤져, 몬스터들이 드롭할 수 있는 아이템들을 미리 생성해놓는다.
/// 필드 위에 자동으로 스폰되는 아이템들은 ItemPool에서 풀링하지 않음 (SpawnManager에서 한다)
/// </summary>

namespace UnityChanRPG
{
    public class ItemPool : MonoBehaviour
    {
        public static ItemPool Instance;

        public const int defaultPoolingNumber = 2;

        // wholePoolingItems는 중복을 허용하지 않는 string들로, 아이템들의 ID를 담음. 처음 풀링할 때 태깅에만 사용하고, 확장엔 사용하지 않음
        private HashSet<string> wholePoolingItems = new HashSet<string>();

        private void ExtendPool(int ID, Transform tag, int extendSize)
        {
            int index = tag.childCount;

            for (int i = index; i < index + extendSize; i++)
            {
                GenerateMonsterSpawnItemPool(ID).parent = tag;
            }
        }

        // 아이템 풀에서 비활성화된 오브젝트를 찾아 활성화함. 없는 경우 Pool을 확장하고 활성화함.
        public void DropItem(int ID, Vector3 spawnPosition, int ItemNumber = 1)
        {
            Transform tag = transform.Find(ID + " (Tag)");

            for (int i = 0; i < tag.childCount; i++)
            {
                GameObject item = tag.GetChild(i).gameObject;

                if (item.activeSelf == false)
                {
                    item.transform.position = spawnPosition;
                    item.GetComponent<PickUpItem>().item.ItemValue = ItemNumber;
                    item.SetActive(true);
                }
                return;
            }

            ExtendPool(ID, tag, defaultPoolingNumber);
            DropItem(ID, spawnPosition, ItemNumber);
        }

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                GameObject monsterPool = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("FieldSpawnMonster Pool").gameObject;

                for (int i = 0; i < monsterPool.transform.childCount; i++)
                {
                    GameObject point = monsterPool.transform.GetChild(i).gameObject;

                    for (int j = 0; j < point.transform.childCount; j++)
                    {
                        MonsterAdapter monsterApt = point.transform.GetChild(j).gameObject.GetComponent<MonsterAdapter>();

                        for (int k = 0; k < monsterApt.monster.monsterDropItems.Count; k++)
                        {
                            for (int l = 0; l < defaultPoolingNumber; l++)
                            {
                                GenerateMonsterSpawnItemPool(monsterApt.monster.monsterDropItems[k].ItemID);

                                wholePoolingItems.Add(monsterApt.monster.monsterDropItems[k].ItemID + "");
                            }
                        }
                    }
                }
                Tagging();
            }
        }

        // 풀링한 아이템들을 ID로 묶어놓는다. (On, Off할 때 더 빠르게 접근하기 위하여)
        private void Tagging()
        {
            foreach (string ID_tag in wholePoolingItems)
            {
                GameObject ID_Tag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Destroy(ID_Tag.gameObject.GetComponent<Collider>());
                Destroy(ID_Tag.gameObject.GetComponent<MeshRenderer>());
                ID_Tag.name = ID_tag + " (Tag)";
                ID_Tag.transform.parent = gameObject.transform;

                while (transform.Find(ID_tag) != null)
                {
                    transform.Find(ID_tag).transform.parent = ID_Tag.transform;
                }
            }
        }

        // 몬스터를 쓰러뜨렸을 때 스폰되는 아이템의 경우, ItemPool을 만들 때 사용하는 함수
        private Transform GenerateMonsterSpawnItemPool(int _ID)
        {
            Transform obj = GameObject.FindGameObjectWithTag("Parser").transform.Find("Item Parser").Find("" + _ID);
            Transform tr = Instantiate(obj, Vector3.zero, Quaternion.identity);

            tr.gameObject.GetComponent<PickUpItem>().item.ItemConsume += obj.gameObject.GetComponent<PickUpItem>().item.ItemConsume;
            tr.gameObject.SetActive(false);
            tr.gameObject.name = _ID + "";
            tr.gameObject.tag = "FieldSpawnItem";
            tr.parent = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("Monster Drop Item Pool");

            return tr;
        }

    }
}
