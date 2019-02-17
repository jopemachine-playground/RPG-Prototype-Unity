using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

/// <summary>
/// 아이템과 그 속성들을 파싱해 갖고 있음. Json 파싱엔 LitJson을 이용
/// </summary>

namespace UnityChanRPG
{
    public class ItemParser : MonoBehaviour
    {
        public static ItemParser mInstance;

        public List<Item> entireItemList = new List<Item>();

        public List<GameObject> objModel;
        public List<Sprite> objIcon;

        private LayerMask pickUpItemLayer;

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

                pickUpItemLayer = LayerMask.NameToLayer("PickUpItem");
                StartCoroutine("LoadCoroutine");
            }
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
                int ItemIndex = getItemIndexByID((int)(name[i]["ID"]));

                entireItemList[ItemIndex].ItemAttributes.Add
                (
                    new ItemAttribute((name[i]["AttributeName"]).ToString(), (int)(name[i]["AttributeValue"]))
                );

                // 추가한 속성을 이벤트에 등록
                int AttIndex = entireItemList[ItemIndex].ItemAttributes.Count - 1;

                switch (entireItemList[ItemIndex].ItemAttributes[AttIndex].AttributeName)
                {
                    case "heal_hp":
                        entireItemList[ItemIndex].test(entireItemList[ItemIndex].ItemAttributes[AttIndex].HealHP);
                        Debug.Log(entireItemList[ItemIndex].ItemConsume == null);
                        break;
                    case "heal_mp":
                        entireItemList[ItemIndex].test(entireItemList[ItemIndex].ItemAttributes[AttIndex].HealMP);
                        Debug.Log(entireItemList[ItemIndex].ItemConsume == null);
                        break;
                    case "get_item":
                        entireItemList[ItemIndex].test(entireItemList[ItemIndex].ItemAttributes[AttIndex].ItemBoxOpen);
                        Debug.Log(entireItemList[ItemIndex].ItemConsume == null);
                        break;
                    case "adj_atk":
                        break;
                    case "adj_def":
                        break;

                    default: Debug.Assert(false, "Attribute does not exist!"); break;
                }
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
            sphere.layer = pickUpItemLayer;
            sphere.SetActive(false);
            sphere.AddComponent<PickUpItem>();
            sphere.AddComponent<Rigidbody>();
            sphere.GetComponent<PickUpItem>().item = entireItemList[_index];

            Debug.Log(entireItemList[_index].ItemConsume == null);
            Debug.Log(sphere.GetComponent<PickUpItem>().item.ItemConsume == null);

            // Item Pool 아래에 PickUpItem 들을 미리 생성
            sphere.transform.parent = GameObject.FindGameObjectWithTag("ItemPool").transform;

            // Renderer 내 Material 배열 교체.
            sphere.GetComponent<Renderer>().materials = entireItemList[_index].ItemModel.GetComponent<Renderer>().sharedMaterials;
            sphere.GetComponent<MeshFilter>().mesh = entireItemList[_index].ItemModel.GetComponent<MeshFilter>().sharedMesh;
        }

        public void GeneratePickUpItem(Vector3 _genPoint, Quaternion _genRotate, int _ID)
        {
            // false
            Debug.Log(entireItemList[0].ItemConsume == null);

            Transform obj = GameObject.FindGameObjectWithTag("ItemPool").transform.Find("" + _ID);
            Transform tr = Instantiate(obj, _genPoint, _genRotate);
            tr.gameObject.SetActive(true);
            tr.parent = GameObject.FindGameObjectWithTag("Pickup Items").transform;
        }

        #endregion

        #region Get Item Info

        public int getItemIndexByID(int _ID)
        {
            for (int i = 0; i < entireItemList.Count; i++)
            {
                if (entireItemList[i].ID == _ID)
                    return i;
            }
            return -1;
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