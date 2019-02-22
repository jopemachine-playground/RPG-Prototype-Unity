using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 플레이어의 현재 HP, 경험치 등의 정보를 실제로 UI에 표시하고 관리하는 싱글톤 클래스.
/// Update에서 계속해서, 창을 관리하는 것은 낭비가 커보여, Status 프로퍼티의 값이 변경될 때 마다, PlayerInfoSystem 클래스의
/// 업데이트 함수를 호출하게 했다.
/// </summary>

namespace UnityChanRPG
{
    public class PlayerInfoSystem : MonoBehaviour
    {
        public static PlayerInfoSystem Instance;

        public Text HPText;
        public Text MPText;
        public Text PlayerName;
        public Text LevelText;

        public Slider HPSlider;
        public Slider MPSlider;
        public Slider EXPSlider;
        public Slider StaminaSlider;

        private Image StaminaSliderFillArea;

        private Player player;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this;
            }
        }

        private void Start()
        {
            player = PlayerInfo.mInstance.player;
            HPText = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("HP Bar").Find("Text").gameObject.GetComponent<Text>();
            MPText = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("MP Bar").Find("Text").gameObject.GetComponent<Text>();
            PlayerName = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("Player NickName Panel").Find("Player NickName Text").gameObject.GetComponent<Text>();
            PlayerName.text = (player.Name);
            LevelText = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("Player NickName Panel").Find("Player LevelText").gameObject.GetComponent<Text>();
            LevelText.text = "Lv." + (player.Level).ToString();
            HPSlider = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("HP Bar").gameObject.GetComponent<Slider>();
            MPSlider = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("MP Bar").gameObject.GetComponent<Slider>();
            EXPSlider = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("Experience Bar").gameObject.GetComponent<Slider>();
            StaminaSlider = GameObject.FindGameObjectWithTag("PlayerInfoSystem").transform.Find("PlayerInfo").Find("Stamina Bar").gameObject.GetComponent<Slider>();
            StaminaSliderFillArea = StaminaSlider.transform.Find("Fill Area").GetComponentInChildren<Image>();

            SliderMaxValueChange();
        }

        // HP, MP, Stamina에 변화가 있을 때 프로퍼티에서 호출되어 값의 변화를 창에 업데이트함
        public void PlayerInfoWindowUpdate()
        {
            if (player == null)
            {
                return;
            }

            HPText.text = (player.playerStatus.CurrentHP).ToString() + " / " + player.MaxHP;
            HPSlider.value = player.playerStatus.CurrentHP;
            MPText.text = (player.playerStatus.CurrentMP).ToString() + " / " + player.MaxMP;
            MPSlider.value = player.playerStatus.CurrentMP;
            EXPSlider.value = player.ExperienceValue;
            StaminaSlider.value = player.playerStatus.Stamina;

            StaminaSliderFillArea.color = Color.yellow;

            if (StaminaSlider.value < 30)
            {
                StaminaSliderFillArea.color = Color.red;
            }
        }

        // 경험치에 변화가 있을 때 프로퍼티에서 호출되어, 레벨업이 필요한 경우, 레벨업 시키고, 창을 업데이트
        public void LevelUP()
        {
            if (player == null)
            {
                return;
            }

            if (player.ExperienceValue > LevelInfo.getMaxExp(player.Level))
            {
                player.ExperienceValue -= LevelInfo.getMaxExp(player.Level);
                player.Level++;
                LevelText.text = "Lv." + (player.Level).ToString();
                player.playerInfoUpdate();
                SliderMaxValueChange();
                player.playerStatus.CurrentHP = player.MaxHP;
                player.playerStatus.CurrentMP = player.MaxMP;
            }

            PlayerInfoWindowUpdate();
        }

        private void SliderMaxValueChange()
        {
            HPSlider.maxValue = player.MaxHP;
            MPSlider.maxValue = player.MaxMP;
            EXPSlider.maxValue = LevelInfo.getMaxExp(player.Level);
            StaminaSlider.maxValue = player.StaminaMax;
        }
    }

}