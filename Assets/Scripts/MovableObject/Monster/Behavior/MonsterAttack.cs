// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    public class MonsterAttack : StateMachineBehaviour
    {

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsAttacking", true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("DamagedProcessed", false);
            animator.SetInteger("AttackType", 0);
            animator.SetBool("IsAttacking", false);
        }
    }

}