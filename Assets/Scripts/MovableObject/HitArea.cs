using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 아래 스크립트의 작성은 http://www.yes24.com/24/goods/27894042 도서를 참고함

public class HitArea : MonoBehaviour
{ 
    public void Damaged(Damage damage)
    {
        // attacker와 attackee가 같은 태그라면, 예를 들어 몬스터가 같은 몬스터를 공격하는 경우
        // 데미지 처리를 하지 않는다.
        
        // Attacker의 Collider와 Attack Area와의 충돌 이벤트는 Layer에서 따로 셋팅해, 충돌이 아예 감지되지 않게 한다.
        // (즉, 여기서 처리하지 않음)
        if (damage.attacker.gameObject.tag == damage.attackee.gameObject.tag) return;

        Status st = gameObject.GetComponent<Status>();
        st.SendMessage("CalculateDamage", damage);
    }


}

