using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
데미지 값을 나타내는 플로팅 텍스트 컴포넌트를 컨트롤. 
*/

public class FloatingTextTweener: MonoBehaviour
{
    public Text text;

    public const float DisableTime = 2.0f;
    private float DisableTimer;

    public bool isActived;

    public Transform targetTr;
    public Damage damage;
    // 플로팅 텍스트를 world 좌표계에서 UI (2D)로 가져오려면 플레이어의 Camera가 필요하다.
    public Camera cam;

    private int updateCounter; 

    private void Awake()
    {
        isActived = false;
        DisableTimer = 0;
        updateCounter = 0;
        text = GetComponent<Text>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        text.font = gameObject.GetComponentInParent<DamageIndicator>().font;
        text.fontSize = 42;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        if (damage.IsFatalBlow)
        {
            text.color = Color.red;
            text.fontSize = 44;
        }
    }

    private void OnEnable()
    {
        isActived = true;
        text.text = damage.value + "";
        text.transform.position = cam.WorldToScreenPoint(targetTr.position);
        StartCoroutine("textUpdate");
    }

    private IEnumerator textUpdate()
    {
        while (true)
        {
            DisableTimer += Time.deltaTime;

            if (DisableTimer > DisableTime)
            {
                DisableTimer = 0;
                updateCounter = 0;
                isActived = false;
                gameObject.active = false;
            }

            else
            {
                updateCounter++;
                Vector3 swap = cam.WorldToScreenPoint(targetTr.position);
                swap.y += 2f * updateCounter;
                text.transform.position = swap;
            }

            yield return null;
        }

    }

}

