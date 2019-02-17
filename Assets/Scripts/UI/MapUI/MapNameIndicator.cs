using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityChanRPG
{
    public class MapNameIndicator : MonoBehaviour
    {
        public static MapNameIndicator Instance;

        public Text mapName
        {
            get;
            private set;
        }

        private Image mapNamePanel;

        private const float DisableTime = 3.0f;
        private float DisableTimer;

        private Color InitialColor;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                InitialColor = new Color(1f, 1f, 1f, 1f);
                mapName = transform.Find("Text").gameObject.GetComponent<Text>();
                mapNamePanel = gameObject.GetComponent<Image>();
            }
        }
        
        public void IndicateMapName(string _mapName)
        {
            DisableTimer = 0;
            mapName.color = InitialColor;
            mapNamePanel.color = InitialColor;
            mapName.text = _mapName;
            MiniMapText.Instance.NameUpdate();
            gameObject.SetActive(true);
            StartCoroutine("FadeOut");
        }

        private IEnumerator FadeOut()
        {
            while (true)
            {
                DisableTimer += Time.deltaTime;

                if (DisableTimer > DisableTime)
                {
                    DisableTimer = 0;
                    gameObject.SetActive(false);
                }

                else
                {
                    mapName.color = new Color(mapName.color.r, mapName.color.g, mapName.color.b, mapName.color.a - 0.25f * Time.deltaTime);
                    mapNamePanel.color = new Color(mapName.color.r, mapName.color.g, mapName.color.b, mapName.color.a - 0.25f * Time.deltaTime);
                }
                yield return null;
            }
        }


    }

}