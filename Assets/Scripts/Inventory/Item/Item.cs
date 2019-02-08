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

    // 아이템이 갖고 있을 성질들
    [SerializeField]
    public List<ItemAttribute> ItemAttributes = new List<ItemAttribute>();

    public Item getCopy()
    {
        return (Item)this.MemberwiseClone();
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

    #region Item Use
    public void use()
    {
        if (ItemType == ItemType.UseAble)
        {
            bool used = false;

            for (int i = 0; i < ItemAttributes.Count; i++)
            {
                switch (ItemAttributes[i].AttributeName)
                {
                    #region heal_hp
                    case "heal_hp":
                        {
                            if (PlayerInfo.mInstance.player.playerStatus.currentHP == LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
                            {
                                continue;
                            }
                            else if (PlayerInfo.mInstance.player.playerStatus.currentHP + ItemAttributes[i].AttributeValue >= LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
                            {
                                Debug.Log(ItemAttributes[i].AttributeValue);
                                PlayerInfo.mInstance.player.playerStatus.currentHP = LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level);
                                used = true;
                            }
                            else if (PlayerInfo.mInstance.player.playerStatus.currentHP + ItemAttributes[i].AttributeValue < LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
                            {
                                PlayerInfo.mInstance.player.playerStatus.currentHP += ItemAttributes[i].AttributeValue;
                                used = true;
                            }
                            break;
                        }
                    #endregion

                    #region heal_mp
                    case "heal_mp":
                        {
                            if (PlayerInfo.mInstance.player.playerStatus.currentMP == LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
                            {
                                continue;
                            }
                            else if (PlayerInfo.mInstance.player.playerStatus.currentMP + ItemAttributes[i].AttributeValue >= LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
                            {
                                PlayerInfo.mInstance.player.playerStatus.currentMP = LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level);
                                used = true;
                            }
                            else if (PlayerInfo.mInstance.player.playerStatus.currentMP + ItemAttributes[i].AttributeValue < LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
                            {
                                PlayerInfo.mInstance.player.playerStatus.currentMP += ItemAttributes[i].AttributeValue;
                                used = true;
                            }
                            
                            break;
                        }
                    #endregion

                    #region get_item
                    case "get_item":
                        {
                            break;
                        }
                    #endregion

                    default: Debug.Assert(false, "Attribute does not exist!"); break;

                }
            }

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

        if (ItemType == ItemType.EquipAble)
        {
            for (int i = 0; i < ItemAttributes.Count; i++)
            {
                switch (ItemAttributes[i].AttributeName)
                {

                    case "adj_atk": PlayerInfo.mInstance.player.playerStatus.currentHP += ItemAttributes[i].AttributeValue; break;

                    case "adj_def": PlayerInfo.mInstance.player.playerStatus.currentMP += ItemAttributes[i].AttributeValue; break;

                    default: Debug.Assert(false, "Attribute does not exist!"); break;

                }
            }
        }

    }
    #endregion 
}
