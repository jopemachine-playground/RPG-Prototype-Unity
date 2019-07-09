using UnityEngine;

namespace UnityChanRPG
{
    class BombPool : MonoBehaviour
    {
        // 기본적으로 생성해놓을 폭탄 갯수

        public int defaultBombValue;

        public int currentBombValue;

        public static BombPool mInstance;

        private void Awake()
        {
            if (mInstance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                mInstance = this;
            }
        }

        public void CastBomb(Vector3 emitPosition, Vector3 castingForce)
        {

            for (int i = 1; i < transform.childCount; i++)
            {
                var childObj = transform.GetChild(i).GetComponent<Bomb>();

                if (childObj.gameObject.activeSelf == false)
                {
                    childObj.gameObject.SetActive(true);
                    childObj.transform.position = emitPosition;
                    childObj.rigidbody.AddForce(castingForce);
                    return;
                }
            }

            ExtendPool(currentBombValue);
            CastBomb(emitPosition, castingForce);
        }

        private void Start()
        {
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

        private void ExtendPool(int extendSize)
        {

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
