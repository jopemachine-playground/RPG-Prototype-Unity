// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    [Serializable]
    public class Player : MonoBehaviour
    {
        static public Player mInstance;

        public PlayerSkillState state;

        #region Variables irrelevant level (including level)

        [SerializeField]
        public string Name;
        [SerializeField]
        public int Money;

        [SerializeField]
        public int Level;

        [SerializeField]
        private int experienceValue;
        public int ExperienceValue
        {
            get
            {
                return experienceValue;
            }
            set
            {
                experienceValue = value;
                if (PlayerInfoSystem.Instance != null)
                {
                    PlayerInfoSystem.Instance.LevelUP();
                }
            }
        }

        [SerializeField]
        public Status playerStatus;

        #endregion

        #region Variables except level

        public int MaxHPIncrement;
        public int MaxMPIncrement;
        public int FatalBlowValueIncrement;
        public int FatalBlowProbIncrement;
        public int AttackValueIncrement;
        public int DefenceValueIncrement;

        #endregion

        public int MaxHP;
        public int MaxMP;
        public float StaminaMax;

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

            StaminaMax = 100;
            playerStatus = GetComponent<Status>();
            playerStatus.CurrentHP = PlayerInfo.mInstance.currentHPTemp;
            playerStatus.CurrentMP = PlayerInfo.mInstance.currentMPTemp;
            playerStatus.Stamina = StaminaMax;
            playerInfoUpdate();

        }

        private void Start()
        {
            PlayerInfoSystem.Instance.PlayerInfoWindowUpdate();
        }

        public void Experience(int TakeExperienceValue)
        {
            ExperienceValue += TakeExperienceValue;
        }

        // 항상 변하는 변수가 아닌, 아이템 착탈의, 레벨 업시에만 변하는 변수들이므로 성능을 위해 update에 넣지 않았다
        // 후에 update에 넣어야 한다면 아래 함수를 그대로 Update로 바꿀 것
        public void playerInfoUpdate()
        {
            MaxHP = LevelInfo.getMaxHP(Level) + MaxHPIncrement;
            MaxMP = LevelInfo.getMaxMP(Level) + MaxMPIncrement;
            playerStatus.FatalBlowValue = LevelInfo.getDefaultFatalBlowValue(Level) + FatalBlowValueIncrement;
            playerStatus.FatalBlowProb = LevelInfo.getDefalutFatalBlowProb(Level) + FatalBlowProbIncrement;
            playerStatus.AttackValue = LevelInfo.getDefaultAttackValue(Level) + AttackValueIncrement;
            playerStatus.DefenceValue = LevelInfo.getDefaultDefenceValue(Level) + DefenceValueIncrement;
        }


    }

}