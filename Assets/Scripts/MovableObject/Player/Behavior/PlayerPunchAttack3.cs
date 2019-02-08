using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunchAttack3 : StateMachineBehaviour
{

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.mInstance.state = PlayerState.PunchAttack3;

        // 다음 콤보 공격을 이을 경우
        if (Input.GetButtonDown("PunchAttack"))
        {
            animator.SetInteger("AttackState", 7);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AttackState", 0);
        Player.mInstance.state = PlayerState.Grounded;
    }


}
