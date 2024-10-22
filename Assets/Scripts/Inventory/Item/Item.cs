// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     Item 클래스는 Item에 대한 정보를 모두 담당. 실제로 필드 위에 표시되는 아이템은 더 많은 정보를 포함하는 PickUpItem 스크립트를
// @     사용하고, Item 클래스엔 json 파일에서 값을 파싱해 담아야 하기 때문에, MonoBehavior를 상속하지 않음. (monster 클래스도 마찬가지)
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    [Serializable]
    public class Item
    {
        #region Item Property

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

        // 아이템 갯수
        public int ItemValue;

        // 아이템의 인벤토리 창에서 표시될 Sprite
        public Sprite ItemIcon;

        // 아이템 타입. 장비 아이템, 소비용 아이템, 퀘스트용 아이템 등. ItemType 참조.
        public ItemType ItemType;

        // 아이템이 갖고 있을 성질들
        [SerializeField]
        public List<ItemAttribute> ItemAttributes = new List<ItemAttribute>();

        #endregion

        #region Item Method
        public Item getCopy()
        {
            Item ret = (Item)this.MemberwiseClone();
            //ret.ItemConsume = this.ItemConsume;
            //ret.ItemEquip = this.ItemEquip;
            return ret;
        }

        public void clean()
        {
            ID = 0;
            Name = "";
            Description = "";
            Rarity = 0;
            ShopPrice = 0;
            ItemValue = 0;
            ItemType = 0;
        }

        #endregion

        #region Item Use

        // 아이템 사용을 위한 Delegate와 이벤트. 사용 가능하다면 true를 리턴

        public delegate bool ItemUsing();
        public ItemUsing ItemConsume;
        public ItemUsing ItemEquip;

        public void Consume()
        {
            if (ItemConsume != null)
            {
                bool used = ItemConsume();

                if (used == true)
                {
                    if (ItemValue == 1)
                    {
                        clean();
                    }
                    else
                    {
                        ItemValue--;
                    }
                }
            }
        }

        public void Equip()
        {
            if (ItemEquip != null)
            {
                bool equiped = ItemEquip();

                if (equiped == true)
                {
                    // 아이템 착 탈의에 대한 코드 처리
                }

            }
        }
        #endregion
    }
}