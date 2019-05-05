using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

using UnityChanRPG;

[TestFixture]
public class UnitTestTest : MonoBehaviour
{
    GameObject player;
    HitArea playerHitArea;
    AttackArea[] playerAtkArea;

    Collider colliderMock;

    [SetUp]
    public void prev()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerHitArea = player.GetComponent<HitArea>();
        playerAtkArea = player.GetComponentsInChildren<AttackArea>();

    }

    [Test]
    public void TestMethod() {

        // playerAtkArea[0].SendMessage("OnTriggerEnter", );
    }

    [TearDown]
    public void after()
    {
        Debug.Log("after");
    }
}
