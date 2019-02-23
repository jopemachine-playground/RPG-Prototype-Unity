using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace UnityChanRPG
{
    abstract public class ICheckPanel : MonoBehaviour
    {
        public Button yesBut;
        public Button noBut;

        public Text Desc;
        public Text Title;

        public Image Icon;

        // 아래처럼 UniRx를 사용해 작성하려 했으나, 클릭 자체를 이벤트로 받아서 
        // 버튼 의외의 곳을 클릭해도 이벤트가 발생하길래, 유니티에서 드래그로 끌어와 썼다.

        //public virtual void Start()
        //{
        //    var LeftClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));

        //    yesBut.onClick
        //        .AsObservable()
        //        .Buffer(LeftClickStream)
        //        .Subscribe(_ =>
        //        {
        //            Debug.Log("확인");
        //        });

        //    noBut.onClick
        //        .AsObservable()
        //        .Buffer(LeftClickStream)
        //        .Subscribe(_ =>
        //        {
        //            this.enabled = false;
        //        });
        //}

        public abstract void yesButtonClicked();

        public void noButtonClicked()
        {
            gameObject.SetActive(false);
        }

    }

}