using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

/// <summary>
/// 아이템과 그 속성들을 파싱해 갖고 있음. Json 파싱엔 LitJson을 이용
/// 아이템을 파싱하는 것만 하는 건 아니고, 자식오브젝트로 갖고 있다가 필요할 때 오브젝트 풀링으로 생성할 때 거쳐가는 역할도 함 (GeneratePickUpItem)
/// </summary>

namespace UnityChanRPG
{
    public class ItemParser : MonoBehaviour
    {
        public static ItemParser mInstance;

        public List<Item> entireItemList = new List<Item>();

        public List<Sprite> objIcon;

        private LayerMask pickUpItemLayer;

        private void Awake()
        {
            if (mInstance != null)
            {
                Debug.Assert(false, "ItemParser Class is Singleton.");
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

                // 추가한 속성을 등록
                int AttIndex = entireItemList[ItemIndex].ItemAttributes.Count - 1;

                switch (entireItemList[ItemIndex].ItemAttributes[AttIndex].AttributeName)
                {
                    case "heal_hp":
                        entireItemList[ItemIndex].ItemConsume += entireItemList[ItemIndex].ItemAttributes[AttIndex].HealHP;
                        break;
                    case "heal_mp":
                        entireItemList[ItemIndex].ItemConsume += entireItemList[ItemIndex].ItemAttributes[AttIndex].HealMP;
                        break;
                    case "get_item":
                        entireItemList[ItemIndex].ItemConsume += entireItemList[ItemIndex].ItemAttributes[AttIndex].ItemBoxOpen;
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
                entireItemList[i].ItemIcon = objIcon[i];
                entireItemList[i].ItemValue = 1;
                transform.Find(entireItemList[i].ID + "").GetComponent<PickUpItem>().item = entireItemList[i];
            }

        }

        #endregion

        #region Make Item Pool

        // 일정한 장소에 스폰되는 아이템의 경우, ItemPool을 만들 때 사용하는 함수
        public void GenerateFieldSpawnItemPool(int _ID, SpawnPoint _SpawnPoint)
        {
            Transform obj = GameObject.FindGameObjectWithTag("Parser").transform.Find("Item Parser").Find("" + _ID);
            Transform tr = Instantiate(obj, Vector3.zero, Quaternion.identity);
            // Instantiate 를 통해 생성한 프리팹의 delegate엔 이 아이템의 속성 delegate (ItemConsume)가 등록되어 있지 않으므로, 직접 붙여줘야함에 주의
            tr.gameObject.GetComponent<PickUpItem>().item.ItemConsume += obj.gameObject.GetComponent<PickUpItem>().item.ItemConsume;
            tr.gameObject.SetActive(false);
            tr.gameObject.name = _ID + "";
            tr.gameObject.tag = "FieldSpawnItem";
            tr.parent = GameObject.FindGameObjectWithTag("Object Pool").transform.Find("FieldSpawnItem Pool").Find(_SpawnPoint.gameObject.name);
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