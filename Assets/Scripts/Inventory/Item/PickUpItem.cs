using System;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 관련 클래스들은 https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310 를 많이 참고해 작성함

public class PickUpItem : MonoBehaviour
{
    public const float elapsedTime = 15.0f;

    public Item item;
    private Inventory inv;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inv = player.GetComponent<Inventory>();
    }

    private void Start()
    {
        // 일정 시간이 지난 아이템은 삭제
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

    // 아이템을 움직이게 해, 애니메이션 처럼 보이게 하려고 했음.
    private void Update()
    {
        transform.Rotate(45 * Time.deltaTime, 90 * Time.deltaTime, 0);
    }

    public void DestroyByTimeElapse()
    {
        Destroy(this.gameObject);
    }



}

