// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:36:52
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    [Serializable]
    public class Particle
    {
        // AttackParticles 내 AttackParticle의 ID들은 모두 다르다고 가정한다
        // Particle ID가 0 이면 파티클이 없는 공격이라 가정하고 처리.
        [SerializeField]
        public int ID;
        [SerializeField]
        public ParticleSystem particle;

        // 기본적으로 생성해놓을 파티클의 갯수
        [NonSerialized]
        public int defaultParticlesNumber = 5;
        [NonSerialized]
        public int currentParticlesNumber = 0;
    }
}