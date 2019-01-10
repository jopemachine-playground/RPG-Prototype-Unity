using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
//    public static Player mInstance;
//    // 임시 이름
//    private string mPlayerName = "Unity Chan";

//    public int mMoney;
//    public int mHP;
//    public int mLevel;
//    public int mExperience;
//    public int mDefence;

//    public GameObject Floating_Damage_text;
//    public GameObject Floating_Damage_Canvas;

//    //public static Player GetInstance()
//    //{
//    //    if(mInstance == null)
//    //    {
//    //        mInstance = FindObjectOfType<Player>();
//    //        if(mInstance == null)
//    //        {
//    //            GameObject container = new GameObject("Player");
//    //            container.AddComponent<Player>();
//    //        }
//    //    }
//    //    return mInstance;
//    //}

//    private void Awake()
//    {
//        if(mInstance != null)
//        {
//            Destroy(this.gameObject);
//        }
//        else
//        {
//            DontDestroyOnLoad(this.gameObject);
//            mInstance = this;
//        }
//    }

//    private Player()
//    {
//        // mLevel = 
//    }

//    public int Damaged(int monsterAtk)
//    {
//        int resultDamage;

//        if (mDefence >= monsterAtk)
//        {
//            resultDamage = 1;
//        }
//        else
//        {
//            resultDamage = monsterAtk - mDefence;
//        }

//        if (mHP <= 0)
//        {
//            Destroy(this.gameObject);
//        }

//        return 0;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        mInstance = this;
//        //mHP = 
//    }

//    //struct Level
//    //{
//    //    int MAX_HP_VALUE;
//    //    int MAX_EXPERIENCE_VALUE;
//    //}

//    //static Level[] level = new Level{ { }, { }, { }, { }, { }, { }, { }, { }, { }, { } };

}
