// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// @ Desc : 
// @     성능을 위해 데미지 플로팅 텍스트들을 오브젝트 풀링으로 관리
// @     디폴트 갯수로 만들어 놓았던 플로팅 텍스트 갯수를 넘으면 두 배의 갯수로 만들어 놓는다.
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanRPG
{
    public class DamageIndicator : MonoBehaviour
    {
        // 기본적으로 생성해놓을 플로팅 텍스트의 갯수

        public int defaultTextValue;

        public int currentTextValue;

        public Font font;

        static public DamageIndicator mInstance;

        public List<GameObject> floatingTextsObj;

        private void Start()
        {
            defaultTextValue = 10;

            currentTextValue = 0;

            if (mInstance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                mInstance = this;
            }

            floatingTextsObj = new List<GameObject>(defaultTextValue);
            floatingTextUpdate();
        }

        private void floatingTextUpdate()
        {
            for (int i = currentTextValue; i < floatingTextsObj.Capacity; i++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.SetActive(false);
                quad.name = "Damage Floating Text " + i;
                quad.AddComponent<Text>();
                quad.AddComponent<FloatingTextTweener>();
                floatingTextsObj.Add(quad);
                quad.transform.parent = gameObject.transform;
            }

            currentTextValue = floatingTextsObj.Count;
        }

        public void CallFloatingText(Damage damageResult)
        {

            for (int i = 0; i < currentTextValue; i++)
            {
                if (floatingTextsObj[i].gameObject.GetComponent<FloatingTextTweener>().isActived == false)
                {
                    FloatingTextTweener floatingText = floatingTextsObj[i].gameObject.GetComponent<FloatingTextTweener>();
                    floatingText.damage = damageResult;
                    floatingTextsObj[i].SetActive(true);
                    return;
                }
            }

            floatingTextsObj.Capacity = currentTextValue * 2;
            floatingTextUpdate();
            CallFloatingText(damageResult);
        }

    }
}