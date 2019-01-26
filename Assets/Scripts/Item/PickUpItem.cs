using System;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    // 일정 시간이 지난 아이템은 삭제
    public const float elapsedTime = 15.0f;

    public Item item;
    private Inventory inv;
    private GameObject player;
    private Light light;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player, "player not existed");
        inv = player.GetComponent<Inventory>();
    }

    private void Start()
    {
        light = GetComponent<Light>();
        light.type = LightType.Point;
        light.range = 4;

        Invoke("DestroyByTimeElapse", elapsedTime);
    }

    // 땅에 떨어진 Pickupitem 객체와 플레이어가 충돌하면 플레이어의 아이템이 되고
    // Pickupitem 객체 비활성화 한다.
    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            // ItemIndexInList는 InventorySystem에서 아이템 순서를 드래깅으로 변경할 때,
            // PickUpItem과 충돌했을 때 변경, 초기화 된다.
            inv.ItemPickup(item);
            Destroy(this.gameObject);

        }
    }

    // 아이템을 움직이게 해, 애니메이션 처럼 보이게 함.
    private void Update()
    {
        transform.Rotate(45 * Time.deltaTime, 90 * Time.deltaTime, 0);
    }

    public void DestroyByTimeElapse()
    {
        Destroy(this.gameObject);
    }



}

