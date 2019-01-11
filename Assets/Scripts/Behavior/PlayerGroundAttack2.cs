using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttack2 : StateMachineBehaviour
{
   

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetInteger("AttackState", 3);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetInteger("AttackState") == 3 && stateInfo.IsName("GroundAttack3"))
        {
            return;
        }

        animator.SetInteger("AttackState", 0);
    }



}