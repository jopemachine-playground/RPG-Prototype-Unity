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

    public const float DisableTime = 0.5f;
    public const float FadeoutTime = .2f;
    private WaitForSeconds FadeoutTimeW;
    private float DisableTimer;

    public bool isActived;

    public Transform targetTr;
    public int damagedValue;
    public Camera cam;

    private void Awake()
    {
        isActived = false;
        DisableTimer = 0;
        text = GetComponent<Text>();
        FadeoutTimeW = new WaitForSeconds(FadeoutTime);
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        text.font = gameObject.GetComponentInParent<DamageIndicator>().font;
        text.fontSize = 42;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
    }

    private void OnEnable()
    {
        isActived = true;
        text.text = damagedValue + "";
        text.transform.position = cam.WorldToScreenPoint(targetTr.position);
        StartCoroutine("textUpdate");
    }

    private IEnumerator textUpdate()
    {
        while (true)
        {
            yield return FadeoutTimeW;

            DisableTimer += Time.deltaTime;

            if (DisableTimer > DisableTime)
            {
                DisableTimer = 0;
                isActived = false;
                gameObject.active = false;
            }

            else
            {
                Vector3 swap = text.transform.position;
                swap.y += 4.0f;
                text.transform.position = swap;
            }
        }

    }

}

