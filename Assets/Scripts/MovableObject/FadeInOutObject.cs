// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mesh Renderer에서 Material들을 모두 가져와 투명하게 만들어 페이드 아웃 시킨다.
/// 플레이어 캐릭터, 몬스터, 아이템을 페이드 인, 아웃 시킬 때 이용. Screen Cover의 페이드 인, 아웃은 FadeInOutObject가 아닌
/// Scene에서 구현되어 있음에 주의 (작동 방식이 다름)

/// 코드는 작성했는데, Stardard Shader의 Transparent 렌더링 모드에서, 에셋이 깨진다는 걸 확인.
/// 이것도 Shader에서 하는거라, 아마도 Material을 투명하게 만드는 Shader 코드를 작성해야 되는 것 같다..?
/// 셰이더 쪽은 아직 배우지 못해 일단 미뤄두기로 했다.
/// </summary>

namespace UnityChanRPG
{
    [RequireComponent(typeof(MeshRenderer))]
    public class FadeInOutObject : MonoBehaviour
    {
        // 페이드 아웃 시키기 위한, 3D 모델의 material 배열. 
        // 자식 컴포넌트의 material 배열을 전부 가져와 List로 만들고, 투명도를 줘 페이드 인, 아웃 시킴
        private MeshRenderer[] meshRenderer;
        private List<Material> materials = new List<Material>();

        // 페이드 인, 아웃되고 있는 상태에선 비활성화 된 것으로 간주할 것.
        public bool IsFadingOut;
        public bool IsFadingIn;
        private WaitForSeconds FADEINOUT_WAITTIME = new WaitForSeconds(0.01f);
        private const float FADEINOUT_SPEED_MULTIPLIER = 1.5f;

        private void Awake()
        {
            meshRenderer = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderer.Length; i++)
            {
                for (int j = 0; j < meshRenderer[i].materials.Length; j++)
                {
                    materials.Add(meshRenderer[i].materials[j]);
                }
            }
        }

        // FadeInOutObject를 구현한 오브젝트 (몬스터, 아이템)는 인에이블 될 때 FadeIn이 자동 적용
        private void OnEnable()
        {
            StopAllCoroutines();

            for (int i = 0; i< materials.Count; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, 1f);
            }

            StartCoroutine("FadeIn");

        }

        // FadeInOutObject를 구현한 오브젝트에서 호출해 사용할 것
        public IEnumerator FadeOut()
        {
            IsFadingOut = true;

            while (materials[materials.Count - 1].color.a >= 0)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    //Debug.Log("FadeOut 실행 ");

                    materials[i].color =
                        new Color(
                            materials[i].color.r,
                            materials[i].color.g,
                            materials[i].color.b,
                            materials[i].color.a - FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
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

            while (materials[materials.Count - 1].color.a <= 100f)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    // Debug.Log("FadeIn 실행 ");

                    materials[i].color =
                        new Color(
                            materials[i].color.r,
                            materials[i].color.g,
                            materials[i].color.b,
                            materials[i].color.a + FADEINOUT_SPEED_MULTIPLIER * Time.deltaTime
                        );

                    // Debug.Log(materials[i].color);
                    yield return FADEINOUT_WAITTIME;
                }
            }

            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].color = new Color(materials[i].color.r, materials[i].color.g, materials[i].color.b, 1f);
            }

            IsFadingIn = false;

        }
    }
}
