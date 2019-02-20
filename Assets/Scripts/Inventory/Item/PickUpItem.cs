using System;
using UnityEngine;
using UnityEngine.UI;

// 인벤토리 관련 클래스들은 https://assetstore.unity.com/packages/tools/gui/inventory-master-ugui-26310 를 많이 참고해 작성함

/// <summary>
/// 필드 위에 표시되는 아이템들을 다루는 클래스. 아이템 말고도, 필드 위에 표시되는 일시적인 회복 아이템이나, 
/// 돈 (코인) 역시 PickUpItem에서 처리한다
/// </summary>

namespace UnityChanRPG
{
    public class PickUpItem : MonoBehaviour
    {
        public const float elapsedTime = 15.0f;

        public Item item;
        private Inventory inv;
        private GameObject player;

        // 아이템의 종류에 따라 전혀 다른 메서드가 실행되어야 하므로 delegate를 이용해 구현. 기본값은 Item.
        // PickUpType이 Item이 아닐 땐, item엔 Null이 있으니 접근해선 안 된다.
        [Serializable]
        public enum PickUpType
        {
            Item = 0,
            Money,
            Recover
        }

        public PickUpType type;

        private delegate void GetItem();
        private GetItem getItem;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            inv = player.GetComponent<Inventory>();

            switch (type)
            {
                case PickUpType.Item:
                    getItem += GetPickUpItem;
                    break;
                case PickUpType.Money:
                    break;
                case PickUpType.Recover:
                    break;
            }
        }

        private void OnEnable()
        {
            // 일정 시간이 지난 아이템은 삭제
            Invoke("DestroyByTimeElapse", elapsedTime);
        }

        // 땅에 떨어진 Pickupitem 객체와 플레이어가 충돌하면 플레이어의 아이템이 되고
        // Pickupitem 객체 비활성화 한다.
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.tag == "Player")
            {
                // ItemIndexInList는 InventorySystem에서 아이템 순서를 드래깅으로 변경할 때,
                // PickUpItem과 충돌했을 때 변경, 초기화 된다.
                getItem();
                ParticlePool.getItemPool.CallParticle(GetItemParticle.ID, transform.position);
                gameObject.SetActive(false);

            }
        }

        private void GetPickUpItem()
        {
            inv.ItemPickup(item);
        }

        // 아이템을 움직이게 해, 애니메이션 처럼 보이게 하려고 했음.
        private void Update()
        {
            transform.Rotate(45 * Time.deltaTime, 90 * Time.deltaTime, 0);
        }

        public void DestroyByTimeElapse()
        {
            gameObject.SetActive(false);
        }
    }

}