using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

// 게임을 시작하면, 화면에 로딩 씬을 띄워놓고 아이템과 몬스터 정보들을 로드.
// 각 오브젝트 풀에서 로드가 끝나면 끝났다는 메서지를 전송하고, 끝나는대로 Main 씬에 돌입.

namespace UnityChanRPG
{
    public class ItemPool : MonoBehaviour
    {
        public static ItemPool mInstance;

        public List<Item> entireItemList = new List<Item>();

        public List<GameObject> objModel;
        public List<Sprite> objIcon;

        private void Awake()
        {
            // 싱글톤
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

        #region Data Parsing And Load
        IEnumerator LoadCoroutine()
        {
            string JsonString_item = File.ReadAllText(Application.dataPath + "/Custom/Resources/ItemData.json");
            string JsonString_attribute = File.ReadAllText(Application.dataPath + "/Custom/Resources/ItemAttribute.json");

            JsonData itemData = JsonMapper.ToObject(JsonString_item);
            JsonData itemAttributeData = JsonMapper.ToObject(JsonString_attribute);

            Debug.Assert(itemData != null, "item Data == null");
            Debug.Assert(itemAttributeData != null, "itemAttribute Data == null");

            ParsingJsonItem(itemData);
            ParsingJsonItemAttribute(itemAttributeData);

            yield return null;
        }


        private void ParsingJsonItemAttribute(JsonData name)
        {
            for (int i = 0; i < name.Count; i++)
            {
                getItemByID((int)(name[i]["ID"])).
                    ItemAttributes.Add
                    (new ItemAttribute((name[i]["AttributeName"]).ToString(), (int)(name[i]["AttributeValue"])));
            }
        }


        private void ParsingJsonItem(JsonData name)
        {
            for (int i = 0; i < name.Count; i++)
            {
                entireItemList[i].ID = (int)(name[i]["ID"]);
                entireItemList[i].Name = (name[i]["Name"]).ToString();
                entireItemList[i].Description = (name[i]["Description"]).ToString();
                entireItemList[i].Rarity = (int)(name[i]["Rarity"]);
                entireItemList[i].ShopPrice = (int)(name[i]["ShopPrice"]);
                entireItemList[i].ItemType = (ItemType)((int)(name[i]["ItemType"]));
                entireItemList[i].ItemModel = objModel[i];
                entireItemList[i].ItemIcon = objIcon[i];
                entireItemList[i].ItemValue = 1;

                MakePickUpItemPool(i, entireItemList[i].ID);
            }

        }

        #endregion

        #region Make Item Pool
        private void MakePickUpItemPool(int _index, int _ID)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "" + _ID;
            // 12는 픽업 아이템의 레이어
            sphere.layer = 12;
            sphere.SetActive(false);
            sphere.AddComponent<PickUpItem>();
            sphere.AddComponent<Rigidbody>();
            sphere.GetComponent<PickUpItem>().item = entireItemList[_index];

            // Item Pool 아래에 PickUpItem 들을 미리 생성
            sphere.transform.parent = GameObject.FindGameObjectWithTag("ItemPool").transform;

            // Renderer 내 Material 배열 교체.
            sphere.GetComponent<Renderer>().materials = entireItemList[_index].ItemModel.GetComponent<Renderer>().sharedMaterials;
            sphere.GetComponent<MeshFilter>().mesh = entireItemList[_index].ItemModel.GetComponent<MeshFilter>().sharedMesh;
        }

        public void GeneratePickUpItem(Vector3 _genPoint, Quaternion _genRotate, int _ID)
        {
            Transform obj = GameObject.FindGameObjectWithTag("ItemPool").transform.Find("" + _ID);
            Transform tr = Instantiate(obj, _genPoint, _genRotate);
            tr.gameObject.SetActive(true);
            tr.parent = GameObject.FindGameObjectWithTag("Pickup Items").transform;
        }

        #endregion

        #region Get Item Info

        public Item getItemByID(int id)
        {
            for (int i = 0; i < entireItemList.Count; i++)
            {
                if (entireItemList[i].ID == id)
                    return entireItemList[i].getCopy();
            }
            return null;
        }

        public Sprite getItemIcon(int _ID)
        {
            for (int i = 0; i < entireItemList.Count; i++)
            {
                if (entireItemList[i].ID == _ID)
                    return entireItemList[i].ItemIcon;
            }
            return null;
        }

        #endregion

    }

}