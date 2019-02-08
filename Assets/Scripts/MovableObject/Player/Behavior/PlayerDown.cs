using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDown : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("RestType", 0);
    }

}
