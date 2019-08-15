// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     아이템이 갖고 있는 hp, mp 회복량, 스탯 변동, 특수 효과등을 attribute로 관리함
// @     ItemParser가 게임 시작시에 Handle Item Effect의 각 함수들을 Item 클래스의 delegate에 등록해 ItemSlot이나 Hotbar 클래스에서
// @     사용 요청이 들어오면 해당 아이템이 갖고 있는 Attribute에 맞게 사용한다. 
// @ Ref URLs : 
// @     1. https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310
// ==============================+===============================================================

using System;
using UnityEngine;
using System.Collections;

namespace UnityChanRPG
{
    [Serializable]
    public class ItemAttribute
    {
        // heal_hp, heal_mp 등의 아이템 효과의 종류
        [SerializeField]
        public string AttributeName;

        // 아이템 효과의 정도
        [SerializeField]
        public int AttributeValue;

        public ItemAttribute(string _AttributeName, int _AttributeValue)
        {
            AttributeName = _AttributeName;
            AttributeValue = _AttributeValue;
        }

        #region Handle Item Effect (Attribute)

        public bool HealHP()
        {
            if (PlayerInfo.mInstance.player.playerStatus.CurrentHP == LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
            {
                return false;
            }
            else if (PlayerInfo.mInstance.player.playerStatus.CurrentHP + AttributeValue >= LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
            {
                PlayerInfo.mInstance.player.playerStatus.CurrentHP = LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level);
                return true;
            }
            else if (PlayerInfo.mInstance.player.playerStatus.CurrentHP + AttributeValue < LevelInfo.getMaxHP(PlayerInfo.mInstance.player.Level))
            {
                PlayerInfo.mInstance.player.playerStatus.CurrentHP += AttributeValue;
                return true;
            }

            return false;
        }

        public bool HealMP()
        {
            if (PlayerInfo.mInstance.player.playerStatus.CurrentMP == LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
            {
                return false;
            }
            else if (PlayerInfo.mInstance.player.playerStatus.CurrentMP + AttributeValue >= LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
            {
                PlayerInfo.mInstance.player.playerStatus.CurrentMP = LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level);
                return true;
            }
            else if (PlayerInfo.mInstance.player.playerStatus.CurrentMP + AttributeValue < LevelInfo.getMaxMP(PlayerInfo.mInstance.player.Level))
            {
                PlayerInfo.mInstance.player.playerStatus.CurrentMP += AttributeValue;
                return true;
            }

            return false;
        }

        public bool ItemBoxOpen()
        {
            return false;
        }

        #endregion
    }
}