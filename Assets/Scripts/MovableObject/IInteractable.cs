using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IInteractAble
{
    // update에서 돌아가며, 공격 애니메이션이 재생될 때 필요한 컬라이더를 활성화 시킨다.
    // 활성화된 컬라이더는 다른 애니메이션이 재생될 때 AttackAnimationQuit의 호출로 비활성화 됨
    bool HandleAttackEvent();

    // damage를 받아와 조건에 맞게 파티클을 넣어놓음 (스킬 ID에 맞는 파티클 ID를 찾아 damage에 값을 넣어놓는다)
    void HandleAttackParticle(ref Damage damage);

    // 공격 당했을 때의 애니메이션을 처리하는 함수.
    void Damaged(Damage damage);

}

