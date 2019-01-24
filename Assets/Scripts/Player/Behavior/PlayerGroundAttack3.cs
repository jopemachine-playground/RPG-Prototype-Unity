using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundAttack3 : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("AttackState", 0);
    }
}
