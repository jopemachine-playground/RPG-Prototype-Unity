using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using UnityChanRPG;

namespace UnityRPG_UnitTest
{
    public class AttackArea
    {
        private int HP = 100;

        public delegate void HandleAttackEvent(int damage);
        public HandleAttackEvent handleAttackEvent;

        public void OnTriggerEnter(HitArea other)
        {
            other.handleAttackedEvent(5);
        }
    }
}