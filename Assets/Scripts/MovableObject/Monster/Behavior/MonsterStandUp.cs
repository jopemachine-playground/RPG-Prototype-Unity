using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityChanRPG
{

    public class MonsterStandUp : StateMachineBehaviour
    {
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //animator.SetBool("IsChasing", true);
        }
    }

}