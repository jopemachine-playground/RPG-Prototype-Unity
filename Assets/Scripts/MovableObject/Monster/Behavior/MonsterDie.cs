// ==============================+===============================================================
// @ Author : jopemachine
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityChanRPG
{
    public class MonsterDie : StateMachineBehaviour
    {

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("IsDied", true);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 몬스터 die에 대한 이벤트 처리 (비활성화 등)
            animator.SetBool("IsDied", false);
        }
    }

}