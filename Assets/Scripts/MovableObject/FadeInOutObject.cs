using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mesh Renderer에서 Material들을 모두 가져와 투명하게 만들어 페이드 아웃 시킨다.
/// 플레이어 캐릭터, 몬스터, 아이템을 페이드 인, 아웃 시킬 때 이용. Screen Cover의 페이드 인, 아웃은 FadeInOutObject가 아닌
/// Scene에서 구현되어 있음에 주의 (작동 방식이 다름)
/// </summary>

namespace UnityChanRPG
{
    [RequireComponent(typeof(MeshRenderer))]
    public class FadeInOutObject : MonoBehaviour
    {
        // 페이드 아웃 시키기 위한, 3D 모델의 material 배열
        private Material[] materials;

        // 페이드 인, 아웃되고 있는 상태에선 비활성화 된 것으로 간주할 것.
        public bool IsFadingOut;
        public bool IsFadingIn;
        private WaitForSeconds FADEINOUT_WAITTIME = new WaitForSeconds(0.01f);
        private const float FADEINOUT_SPEED_MULTIPLIER = 1.5f;

        private void Awake()
        {
            materials = GetComponent<MeshRenderer>().materials;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            Debug.Log("FadeIn 시작! ");

            for (int i = 0; i< materials.Length; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, 1f);
            }

            StartCoroutine("FadeIn");

        }


        public IEnumerator FadeOut()
        {
            IsFadingOut = true;

            while (materials[materials.Length - 1].color.a <= 1)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    //Debug.Log("FadeOut 실행 ");

                    materials[i].color =
                        new Color(
                            materials[i].color.r,
                            materials[i].color.g,
                            materials[i].color.b,
                            materials[i].color.a + FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
                        );
                    //Debug.Log(materials[i].color);
                    yield return FADEINOUT_WAITTIME;
                }
            }

            IsFadingOut = false;
            gameObject.SetActive(false);
        }

        public IEnumerator FadeIn()
        {
            IsFadingIn = true;

            while (materials[materials.Length - 1].color.a >= 0)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    // Debug.Log("FadeIn 실행 ");

                    materials[i].color =
                        new Color(
                            materials[i].color.r,
                            materials[i].color.g,
                            materials[i].color.b,
                            materials[i].color.a - FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
                        );

                    // Debug.Log(materials[i].color);
                    // 이유는 아직 모르겠지만 작동 X
                    yield return FADEINOUT_WAITTIME;
                }
            }

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, 0f);
            }

            IsFadingIn = false;

        }
    }
}
