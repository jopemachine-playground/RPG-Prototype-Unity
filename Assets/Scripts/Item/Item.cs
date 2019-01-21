using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    // 아이템 고유번호. (중복되지 않음)
    public int ID;

    // 아이템 이름
    public string Name;  
    
    // 툴팁 내 아이템 설명
    public string Description;

    // 아이템 레어도
    public int Rarity;

    // 상점 가격
    public int ShopPrice;

    // 아이템의 fbx 모델 (3D 맵 상에서 표현될 모델)
    public GameObject ItemModel;

    // 아이템 갯수
    public int ItemValue;

    // 아이템의 인벤토리 창에서 표시될 Sprite
    public Sprite ItemIcon;                 
    
    // 아이템 타입. 장비 아이템, 소비용 아이템, 퀘스트용 아이템 등. ItemType 참조.
    public ItemType ItemType;

    // 인벤토리 내 인덱스 번호
    public int IndexItemInList;

    // 아이템이 갖고 있을 성질들
    [SerializeField]
    public List<ItemAttribute> ItemAttributes = new List<ItemAttribute>();

    public Item getCopy()
    {
        return (Item)this.MemberwiseClone();
    }

}
