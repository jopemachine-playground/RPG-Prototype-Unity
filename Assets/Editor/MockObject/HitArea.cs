using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using UnityChanRPG;

namespace UnityRPG_UnitTest
{
    public class HitArea 
    {
        private int HP = 100;

        public delegate void HandleAttackedEvent(int damage);
        public HandleAttackedEvent handleAttackedEvent;

        public HitArea()
        {
            handleAttackedEvent += Damaged;
        }

        public int GetHP() {
            return HP;
        }

        private void Damaged(int damage)
        {
            HP -= damage;
        }
    }
}