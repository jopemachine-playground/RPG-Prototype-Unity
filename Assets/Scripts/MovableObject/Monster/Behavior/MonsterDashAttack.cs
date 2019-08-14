// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    public class MonsterDashAttack : StateMachineBehaviour
    {

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsDashAttacking", true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("DamagedProcessed", false);
            animator.SetBool("IsDashAttacking", false);
        }
    }

}