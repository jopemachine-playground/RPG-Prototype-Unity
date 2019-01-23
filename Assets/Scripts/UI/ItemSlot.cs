using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using UniRx;

// 페이지 방식으로 구현해 아이템을 슬롯 갯수 (25개) 초과해
// 획득한 경우 슬롯 내용을 바꾸는 형식으로 구현할 것.

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item Item;

    public bool ItemExist;

    [SerializeField]
    public Button button;

    public static ItemDeletePanel panel;

    // ToolTip을 활성화
    public void OnPointerEnter(PointerEventData data)
    {

    }

    // ToolTip을 비활성화
    public void OnPointerExit(PointerEventData data)
    {

    }

    // 오른쪽 클릭만을 처리. 오른쪽 클릭 시 아이템을 삭제할 것이냐고 묻는 창을 띄움.
    public void OnPointerClick(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Right && panel.gameObject.active == false)
        {
            panel.DeleteSlot = this;
            panel.gameObject.SetActive(true);
        }
    }

    public void Delete()
    {
        this.ItemExist = false;
    }

    // Update를 돌며 클릭과 더블클릭을 나눠 입력 받으려 했지만, UniRx란 걸 알게 되어 이걸로 구현 (모든 이벤트 처리를 UniRx로 구현하진 않고 더블클릭만 이걸로 구현했음)
    void Start()
    {
        var doubleLeftClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        var rightClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));

        if (panel == null)
        {
            panel = GameObject.FindGameObjectWithTag("InventorySystem").transform.Find("Check Panel").gameObject.GetComponent<ItemDeletePanel>();
        }

        button.onClick
            .AsObservable()
            .Buffer(doubleLeftClickStream.Throttle(TimeSpan.FromMilliseconds(100)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(_ =>
            {
                Debug.Log("두 번 클릭");
            });

    }

}
