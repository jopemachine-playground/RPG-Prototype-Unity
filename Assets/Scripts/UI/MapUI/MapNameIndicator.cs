using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNameIndicator : MonoBehaviour
{
    private Text mapName;
    private Image mapNamePanel;

    public const float DisableTime = 3.0f;
    private float DisableTimer;

    private void Awake()
    {
        mapName = transform.Find("Text").gameObject.GetComponent<Text>();
        mapNamePanel = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        DisableTimer = 0;
        StartCoroutine("MapNameIndicate");
    }

    private IEnumerator MapNameIndicate()
    {
        while (true)
        {
            DisableTimer += Time.deltaTime;

            if (DisableTimer > DisableTime)
            {
                DisableTimer = 0;
                gameObject.active = false;
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

