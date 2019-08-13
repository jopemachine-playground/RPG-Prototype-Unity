// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityChanRPG
{
    static class ControlFlags
    {
        // 스테미너를 회복할 수 있는 상태 (최대치보다 적은 스태미너 보유)
        static public bool IsStaminaRecoverable(Status status, float staminaRecoverMultiplier)
        {
            return status.Stamina + staminaRecoverMultiplier * Time.deltaTime < Player.mInstance.StaminaMax;
        }

        // 스테미너를 소모해 달릴 수 있는 상태
        static public bool IsRunnable(Animator animator, Status status)
        {
            return 
                ControlKeyStates.RunningButtonClicked() &&
                ControlKeyStates.ArrowButtonClicked() &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
                status.Stamina > 30;
        }

        // 스태미너가 꽉 찬 상태
        static public bool IsStaminaFull(Status status)
        {
            return status.Stamina < Player.mInstance.StaminaMax;
        }

        // 헐떡이는 상태
        static public bool IsGaspable(Animator animator, Status status)
        {
            return 
                ControlKeyStates.RunningButtonClicked() &&
                ControlKeyStates.ArrowButtonClicked() &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded") &&
                status.Stamina < 30;
        }

        // 움직일 수 있는 상태
        static public bool IsMoveable(Animator animator, bool IsOnGrounded)
        {            
            // Attack 중이라면 움직일 수 없음
            return
                animator.GetInteger("AttackState") == 0 && IsOnGrounded == true &&
                animator.GetCurrentAnimatorStateInfo(0).IsTag("Ground") |
                animator.GetCurrentAnimatorStateInfo(0).IsName("Dash Attack");
        }

        static public bool IsOnGroundable(float DistanceFromGround, Rigidbody rigidbody, float terrainSlope, float slopeLimit)
        {
            return 
                DistanceFromGround <= PlayerControl.GroundCheckDistance && 
                rigidbody.velocity.y <= 0.001f &&
                terrainSlope < slopeLimit;
        }

        static public bool IsOnGround(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
        }

        static public bool IsOnAirable(float DistanceFromGround, Rigidbody rigidbody) {
            return 
                DistanceFromGround > PlayerControl.GroundCheckDistance &&
                rigidbody.velocity.y > 0.001f;
        }

        static public bool IsSlideable(float DistanceFromGround, float terrainSlope, float slopeLimit) {
            return
                DistanceFromGround <= PlayerControl.GroundCheckDistance && 
                terrainSlope > slopeLimit;
        }

        static public bool IsRefreshable() {
            return Player.mInstance.playerStatus.Stamina <= 15;
        }


    }
}
