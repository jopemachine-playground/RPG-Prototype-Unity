using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// 플레이어의 현재 HP, 경험치 등의 정보를 실제로 UI에 표시하는 클래스

public class PlayerInfoSystem: MonoBehaviour
{
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

    private void Update()
    {
        HPText.text = (player.playerStatus.currentHP).ToString() + " / " + player.MaxHP;
        HPSlider.value = player.playerStatus.currentHP;
        MPText.text = (player.playerStatus.currentMP).ToString() + " / " + player.MaxMP;
        MPSlider.value = player.playerStatus.currentMP;
        EXPSlider.value = player.ExperienceValue;
        StaminaSlider.value = player.playerStatus.stamina;

        StaminaSliderFillArea.color = Color.yellow;

        if (StaminaSlider.value < 30)
        {
            StaminaSliderFillArea.color = Color.red;
        }

        LevelUP();
    }

    private void LevelUP()
    {
        if (player.ExperienceValue > LevelInfo.getMaxExp(player.Level))
        {
            player.ExperienceValue -= LevelInfo.getMaxExp(player.Level);
            player.Level++;
            player.playerStatus.currentHP = player.MaxHP;
            player.playerStatus.currentMP = player.MaxMP;
            SliderMaxValueChange();
            LevelText.text = "Lv." + (player.Level).ToString();
            player.playerInfoUpdate();
        }

    }

    private void SliderMaxValueChange()
    {
        HPSlider.maxValue = player.MaxHP;
        MPSlider.maxValue = player.MaxMP;
        EXPSlider.maxValue = LevelInfo.getMaxExp(player.Level);
        StaminaSlider.maxValue = player.StaminaMax;
    }


}

