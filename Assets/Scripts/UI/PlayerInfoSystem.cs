using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerInfoSystem: MonoBehaviour
{
    //public Text Floating_Damage_text;
    //public Text Floating_Damage_Canvas;

    public Text HPText;
    public Text MPText;
    public Text PlayerName;
    public Text LevelText;

    public Slider HPSlider;
    public Slider MPSlider;
    public Slider EXPSlider;

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

        SliderMaxValueChange();
    }

    private void Update()
    {
        HPText.text = (player.currentHP).ToString() + " / " + LevelInfo.getMAXHP(player.Level);
        HPSlider.value = player.currentHP;
        MPText.text = (player.currentMP).ToString() + " / " + LevelInfo.getMAXMP(player.Level);
        MPSlider.value = player.currentMP;
        EXPSlider.value = player.ExperienceValue;
        LevelUP();
    }

    private void LevelUP()
    {
        if (player.ExperienceValue > LevelInfo.getMaxExp(player.Level))
        {
            player.ExperienceValue -= LevelInfo.getMaxExp(player.Level);
            player.Level++;
            player.currentHP = LevelInfo.getMAXHP(player.Level);
            player.currentMP = LevelInfo.getMAXMP(player.Level);
            SliderMaxValueChange();
            LevelText.text = "Lv." + (player.Level).ToString();
        }

    }

    private void SliderMaxValueChange()
    {
        HPSlider.maxValue = LevelInfo.getMAXHP(player.Level);
        MPSlider.maxValue = LevelInfo.getMAXMP(player.Level);
        EXPSlider.maxValue = LevelInfo.getMaxExp(player.Level);
    }


}

