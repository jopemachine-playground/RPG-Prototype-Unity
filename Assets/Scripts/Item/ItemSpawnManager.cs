using UnityEngine;
using System;
using System.Collections;


// 랜덤으로 스폰 위치와 아이템 종류를 결정해 스폰함.
// 그리고 일정 확률로 스폰하지 않음.

public class ItemSpawnManager : MonoBehaviour
{
    public GameObject SpawnArea;
  
    public int maxItem;

    private WaitForSeconds waitingTime;
    public ItemSpawnPoint[] SpawnPoint;
    public float WaitingTime;

    private void Awake()
    {
        waitingTime = new WaitForSeconds(WaitingTime);
        SpawnPoint = GetComponentsInChildren<ItemSpawnPoint>();
    }

    private void Start()
    {
        if (SpawnPoint.Length > 0)
        {
            StartCoroutine("SpawnRandomItemOnField");
        }
    }

    IEnumerator SpawnRandomItemOnField()
    {
        while (true)
        {
            int FieldSpawnItemCount = GameObject.FindGameObjectsWithTag("FieldSpawnItem").Length;

            if (FieldSpawnItemCount < maxItem)
            {
                yield return waitingTime;

                int index = UnityEngine.Random.Range(0, SpawnPoint.Length);

                float probability = ((float)UnityEngine.Random.Range(0, 100)) / 100f;

                int ID = ReturnID(probability, index, 0.01f);

                ItemPool.mInstance.GeneratePickUpItem(SpawnPoint[index].transform.position, Quaternion.identity, ID);
            }
            else
            {
                yield return null;
            }
        }
    }

    private int ReturnID(float _prob, int _placeIndex, float _minProbUnit)
    {
        // Debug.Log("_prob: " + _prob);

        // Debug.Log("SpawnPoint[index].SpawnItemList.Count: " + SpawnPoint[_placeIndex].SpawnItemList.Count);

        Debug.Assert(_prob <= 1, "Error Occur - ReturnID in ItemSpawnManager.cs");

        float[] itemProbAccum = new float[SpawnPoint[_placeIndex].SpawnItemList.Count + 1];

        float[] minProb = new float[SpawnPoint[_placeIndex].SpawnItemList.Count];

        for (int i = 1; i < SpawnPoint[_placeIndex].SpawnItemList.Count + 1; i++)
        {
            itemProbAccum[i] = (SpawnPoint[_placeIndex].SpawnItemList[i - 1].SpawnProbablity);

            itemProbAccum[i] += itemProbAccum[i - 1];

            // Debug.Log("itemProbAccum[ " + i + "] : " + itemProbAccum[i]);

            minProb[i - 1] = 

                Math.Abs(itemProbAccum[i] - _prob - _minProbUnit) < Math.Abs(itemProbAccum[i - 1] - _prob) ?

                Math.Abs(itemProbAccum[i] - _prob - _minProbUnit) : Math.Abs(itemProbAccum[i - 1] - _prob);

            // 확률의 합은 1보다 작아야 함
            Debug.Assert(itemProbAccum[i] <= 1, "Error Occur - ReturnID in ItemSpawnManager.cs");
        }

        int ItemIndex = 0;
        float result = 1.0f;

        for (int i = 0; i < minProb.Length; i++)
        {
            // Debug.Log("minProb[ " + i + "] : " + minProb[i]);
            if (result > minProb[i])
            {
                result = minProb[i];
                ItemIndex = i;
            }
        }

        // Debug.Log("ItemIndex: " + ItemIndex);
        // Debug.Log("result: " + result);

        return SpawnPoint[_placeIndex].SpawnItemList[ItemIndex].ID;
    }

}

