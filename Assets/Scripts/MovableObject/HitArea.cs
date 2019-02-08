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
        if (damage.attacker.gameObject.tag == damage.attackee.gameObject.tag) return;

        Status st = gameObject.GetComponent<Status>();
        st.SendMessage("CalculateDamage", damage);
    }


}

