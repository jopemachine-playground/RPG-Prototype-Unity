using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityChanRPG
{
    [System.Serializable]
    public class PlayerSkill
    {
        public int ID;
        public string Name;
        public string Description;
        public int AttackValueCoefficient;
        public string AnimationClipName;
        public int EmittingParticleID;
    }

}