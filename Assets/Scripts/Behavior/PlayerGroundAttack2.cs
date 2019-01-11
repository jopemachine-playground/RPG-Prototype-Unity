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

        animator.SetInteger("AttackState", 0);
    }



}