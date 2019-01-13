using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

/*
  
  게임에 필요한 정보들을 json으로 직접 저장
  세이브 로드 기능은 GameManager에서 별도로 제공.

*/

public class JsonManager : MonoBehaviour
{

    // 게임 내 모든 아이템들의 리스트
    public List<Item> itemList = new List<Item>();


    private void ItemMake()
    {
        itemList.Add(new Item(10001, 500, "초급자의 빨간 물약", 1, "체력을 50 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(10002, 750, "초급자의 파란 물약", 1, "마력을 50 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(11001, 750, "초급 아이템 상자", 1, "초급 아이템들을 얻을 수 있는 아이템 상자", ItemType.UseAble));
        itemList.Add(new Item(10003, 1200, "중급자의 빨간 물약", 1, "체력을 100 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(10004, 1600, "중급자의 파란 물약", 1, "마력을 100 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(11002, 1500, "중급 아이템 상자", 1, "중급 아이템들을 얻을 수 있는 아이템 상자", ItemType.UseAble));
        itemList.Add(new Item(10005, 3000, "상급자의 빨간 물약", 1, "체력을 200 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(10006, 3500, "상급자의 파란 물약", 1, "마력을 200 회복시키는 마법의 물약", ItemType.UseAble));
        itemList.Add(new Item(20001, 10000, "초심자의 칼", 1, "이제 막 모험을 시작한 초심자의 칼", ItemType.EquipAble));

        JsonData ItemJson = JsonMapper.ToJson(itemList);

        File.WriteAllText(Application.dataPath + "/Custom/Resources/ItemData.json", ItemJson.ToString());
    }

}
