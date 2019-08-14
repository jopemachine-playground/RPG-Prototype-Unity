// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    /* 
     * 특정 컬라이더 영역 내에 플레이어가 있을 경우, 플레이어의 CharacterControl 컴포넌트를
     * 일시적으로 비활성화하고 Rigidbody로 움직이게 한다.
     * 
     * 특정 이유로 Character Control이 아닌 Rigidbody를 통한 물리 효과로 캐릭터를 움직여야 할 때 사용
     */

    #region UNUSED CODE
    //public class ApplyPhysics: MonoBehaviour
    //{
    //    private Player player;
    //    private Collider ApplyPhysicsArea;
    //    private Rigidbody playerRigidbody;

    //    private void Start()
    //    {
    //        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>();
    //        playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
    //        ApplyPhysicsArea = GetComponent<Collider>();
    //    }

    //}
    #endregion

}