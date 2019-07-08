using UnityEngine;

namespace UnityChanRPG
{
    class BombPool: MonoBehaviour
    {
        // 기본적으로 생성해놓을 폭탄 갯수

        public int defaultBombValue;

        public int currentBombValue;

        private void Start() {
            InitBombPool();
        }
        private void InitBombPool()
        {
            for (int i = 0; i < defaultBombValue; i++)
            {
                Transform obj = transform.Find("Bombball");
                Transform tr = Instantiate(obj, Vector3.zero, Quaternion.identity);

                tr.name = "bomb" + currentBombValue++;
                tr.parent = gameObject.transform;
                tr.gameObject.SetActive(false);
            }
        }

        private void ExtendPool(int extendSize) {

            for (int i = currentBombValue; i < currentBombValue + extendSize; i++)
            {
                Transform obj = transform.Find("Bombball");
                Transform tr = Instantiate(obj, Vector3.zero, Quaternion.identity);

                tr.name = "bomb" + currentBombValue++;
                tr.parent = gameObject.transform;
                tr.gameObject.SetActive(false);
            }
        }

    }
}
