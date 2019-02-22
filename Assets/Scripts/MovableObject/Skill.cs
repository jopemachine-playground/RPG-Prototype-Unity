using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 플레이어와 몬스터의 스킬을 담는 자료구조.
/// </summary>

namespace UnityChanRPG
{
    [Serializable]
    public class Skill
    {
        // 스킬 별 고유한 ID
        public int Skill_ID;
        // 스킬 사용자의 ID. 몬스터의 경우 ID를 쓰고, 플레이어는 0
        public int USER_ID;
        public string Name;
        public string Description;
        public int AttackValueCoefficient;
        public string AnimationClipName;
        public int EmittingParticleID;

        public Skill(int _Skill_ID, int _USER_ID, string _Name, string _Desc, int _AttackValueCoefficient, string _AnimationClipName, int _EmittingParticleID)
        {
            Skill_ID = _Skill_ID;
            USER_ID = _USER_ID;
            Name = _Name;
            Description = _Desc;
            AttackValueCoefficient = _AttackValueCoefficient;
            AnimationClipName = _AnimationClipName;
            EmittingParticleID = _EmittingParticleID;
        }

    }

}