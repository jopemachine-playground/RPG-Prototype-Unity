using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// 아래 스크립트의 작성은 http://www.yes24.com/24/goods/27894042 도서를 참고함

public class AttackArea : MonoBehaviour
{
    private Status thisStatus;
    private new Collider collider;
    private Animator attacker;

    private void Start()
    {
        thisStatus = gameObject.GetComponentInParent<Status>();
        gameObject.GetComponentInParent<Animator>();
        collider = GetComponent<Collider>();
        attacker = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    { 
        HitArea hit = other.gameObject.GetComponent<HitArea>();

        if (hit == null) return;

        Damage damage = thisStatus.Attack();

        damage.attackee = other.gameObject.GetComponent<Animator>();

        damage.attacker = attacker;

        // PlayerControl의 Damaged, HitArea의 Damaged를 호출한다
        other.SendMessage("Damaged", damage);
    }

    // 각 MoveableObject들은 공격 애니메이션의 진행도에 따라 OnAttack, OffAttack을 호출해 처리하도록 한다
    public void OnAttack()
    {
        collider.enabled = true;
    }

    public void OffAttack()
    {
        collider.enabled = false;
    }
}

