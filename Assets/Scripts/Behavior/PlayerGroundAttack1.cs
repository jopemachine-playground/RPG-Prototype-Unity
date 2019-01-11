using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttack1 : StateMachineBehaviour
{

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 다음 콤보 공격을 이을 경우
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetInteger("AttackState", 2);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetInteger("AttackState") == 2 && stateInfo.IsName("GroundAttack2"))
        {
            return;
        }

        animator.SetInteger("AttackState", 0);
    }


}
