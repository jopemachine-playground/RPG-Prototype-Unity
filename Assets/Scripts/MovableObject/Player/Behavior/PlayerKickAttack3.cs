// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    public class PlayerKickAttack3 : StateMachineBehaviour
    {
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Player.mInstance.state = PlayerSkillState.KickAttack3;

            if (Input.GetButtonDown("KickAttack"))
            {
                animator.SetInteger("AttackState", 4);
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("DamagedProcessed", false);
            animator.SetInteger("AttackState", 0);
            Player.mInstance.state = PlayerSkillState.None;
        }
    }
}