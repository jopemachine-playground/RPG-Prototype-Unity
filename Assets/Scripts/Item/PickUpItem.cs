using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class PickUpItem : MonoBehaviour
{
    public Item item;
    private Inventory inv;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player, "player not existed");
        inv = player.GetComponent<Inventory>();

    }

    // 땅에 떨어진 Pickupitem 객체와 플레이어가 충돌하면 플레이어의 아이템이 되고
    // Pickupitem 객체는 파괴한다.
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Player")
        {
            Destroy(this.gameObject);
            inv.ItemPickup(item);
            
        }

    }
}

