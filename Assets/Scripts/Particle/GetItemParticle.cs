using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 아이템을 먹었을 때 나오는 이펙트가 아이템에 상관없이 같다고 가정했기 때문에 GetItemParticleList를 만들지 않았다.
/// 필요하다면, 만들어 사용할 것
/// </summary>

namespace UnityChanRPG
{
    public class GetItemParticle: MonoBehaviour
    {
        [NonSerialized]
        public const int ID = 0;
        [SerializeField]
        public ParticleSystem particle;

        // 기본적으로 생성해놓을 파티클의 갯수
        [NonSerialized]
        public int defaultParticlesNumber = 5;
        [NonSerialized]
        public int currentParticlesNumber = 0;
    }
}
