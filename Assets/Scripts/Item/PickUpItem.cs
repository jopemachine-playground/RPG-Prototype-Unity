using System;
using UnityEngine;
using UnityEngine.UI;

class PickUpItem : MonoBehaviour
{
    // 일정 시간이 지난 아이템은 삭제해 메모리 버그를 방지
    public const float elapsedTime = 15.0f;

    public Item item;
    private Inventory inv;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player, "player not existed");
        inv = player.GetComponent<Inventory>();
        Invoke("ItemDestroy", elapsedTime);
    }

    // 땅에 떨어진 Pickupitem 객체와 플레이어가 충돌하면 플레이어의 아이템이 되고
    // Pickupitem 객체 비활성화 한다.
    void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag == "Player")
        {
            Destroy(this.gameObject);
            inv.ItemPickup(item);
        }
    }

    private void ItemDestroy()
    {
        Destroy(this.gameObject);
    }


}

