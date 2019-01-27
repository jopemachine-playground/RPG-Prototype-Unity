using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamaged : StateMachineBehaviour
{

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsDamaged", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsDamaged", false);
    }
}

