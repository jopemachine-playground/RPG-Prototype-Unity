using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

// 게임을 시작하면, 화면에 로딩 씬을 띄워놓고 아이템과 몬스터 정보들을 로드.
// 각 오브젝트 풀에서 로드가 끝나면 끝났다는 메서지를 전송하고, 끝나는대로 Main 씬에 돌입.


public class ItemPool : MonoBehaviour
{
    public static ItemPool mInstance;

    public List<Item> entireItemList = new List<Item>();

    public List<GameObject> objModel;
    public List<Sprite> objIcon;

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

        Load();
    }

    public void Load()
    {
        StartCoroutine("LoadCoroutine");
    }

    IEnumerator LoadCoroutine()
    {
        string JsonString = File.ReadAllText(Application.dataPath + "/Custom/Resources/ItemData.json");

        JsonData itemData = JsonMapper.ToObject(JsonString);

        ParsingJsonItem(itemData);

        yield return null;
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
        }

    }

}

