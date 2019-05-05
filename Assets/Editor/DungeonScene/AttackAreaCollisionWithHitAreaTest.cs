using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;


/// <summary>
/// 플레이어 객체와 몬스터와의 충돌을 가상으로 만들어 테스트하는 테스트 코드. 아래와 같은 메서드들을 테스트한다.
/// 1 - 플레이어 AttackArea와 몬스터 HitArea 충돌에 대한 이벤트 테스트 (OnTriggerEvent)
/// 2 - 몬스터 AttackArea와 플레이어 HitArea 충돌에 대한 이벤트 테스트 (OnTriggerEvent)
/// 
/// 아래의 환경에서 테스트할 수 있다.
/// 1 - Dungeon Scene : (FieldOnMonster 타입의 SpawnManager 컴포넌트가 존재하는 Scene)
/// </summary>

namespace UnityRPG_UnitTest
{
    [TestFixture]
    public class AttackAreaCollisionWithHitAreaTest : MonoBehaviour
    {
        GameObject player;
        GameObject monster;
        HitArea playerHitArea;
        HitArea monsterHitArea;

        AttackArea playerAtkArea;
        AttackArea monsterAtkArea;


        [SetUp]
        public void test_Env_SetUp()
        {
            playerAtkArea = new AttackArea();
            monsterHitArea = new HitArea();
        }

        [Test]
        public void test_PlayerAttackArea_CollisionWith_MonsterHitArea()
        {
            playerAtkArea.OnTriggerEnter(monsterHitArea);

            Assert.That(monsterHitArea.GetHP() == 95);

        }

        [Test]
        public void test_MonsterAttackArea_CollisionWith_PlayerHitArea()
        {

 
        }

        [TearDown]
        public void afterTest()
        {
            Debug.Log("after");
        }
    }
}