using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityChanRPG
{
    [Serializable]
    public class EntryPoint : MonoBehaviour
    {
        // 유니티 에디터에서 전환할 씬의 이름 (장소의 이름) 과 동일하게 맞춰줄 것. 
        // 장소 이름 (placeName) 과는 다름에 주의
        public string nodeName;

        // 엔트리 포인트와는 별개로, 엔트리 포인트를 통해 들어올 때 맞춰줄 위치
        public GameObject entrance;

        // 엔트리 포인트에 들어오면 씬을 전환함. 캐릭터의 이동은 MoveCharacter에서.
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Dungeon.thisScene.MoveScene(nodeName);
            }
        }
    }
}