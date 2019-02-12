using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 파티클을 미리 생성하고 활성화 해 놓기 때문에, 모든 공격 파티클은 PlayOnAwake가 false여야 한다.

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool mInstance;

    private const int PARTICLE_LAYER = 14;

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

        AttackParticleList[] particleLists = FindObjectsOfType<AttackParticleList>();

        for (int i = 0; i < particleLists.Length; i++)
        {
            for (int j = 0; j < particleLists[i].attackParticleList.Count; j++)
            {
                GameObject ID_Tag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Destroy(ID_Tag.gameObject.GetComponent<Collider>());
                Destroy(ID_Tag.gameObject.GetComponent<MeshRenderer>());
                ID_Tag.name = particleLists[i].attackParticleList[j].ID + "";
                ID_Tag.transform.parent = gameObject.transform;
                ID_Tag.layer = PARTICLE_LAYER;

                for (int k = 0; k < particleLists[i].attackParticleList[j].defaultParticlesNumber; k++)
                {
                    GameObject particleObj = Instantiate(particleLists[i].attackParticleList[j].particle.gameObject, Vector3.zero, Quaternion.identity);
                    particleObj.name = ID_Tag.name + " (" + k + ")";
                    particleObj.transform.parent = ID_Tag.transform;
                    particleObj.layer = PARTICLE_LAYER;
                }

                particleLists[i].attackParticleList[j].currentParticlesNumber = particleLists[i].attackParticleList[j].defaultParticlesNumber;
            }
        }
    }

    public void CallAttackParticle(int particleID, Vector3 emitPosition)
    {
        ParticleSystem particle = getParticleObject(getParticlePoolByID(particleID));
        particle.transform.position = emitPosition;
        particle.gameObject.SetActive(true);
        particle.Play();
    }
    

    private GameObject getParticlePoolByID(int particleID)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == particleID + "")
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        Debug.Assert(false, "Wrong Particle ID");
        return null;
    }


    private ParticleSystem getParticleObject(GameObject ID_Tag)
    {
        for (int i = 0; i < ID_Tag.transform.childCount; i++)
        {
            GameObject childObj = ID_Tag.transform.GetChild(i).gameObject;
            ParticleSystem particle = childObj.GetComponent<ParticleSystem>();

            if (particle.IsAlive(true) == false)
            {
                return particle;
            }
        }
        
        extendList(ID_Tag, 5);
        return getParticleObject(ID_Tag);
    }

    // List 내 생성된 파티클을 늘림
    private void extendList(GameObject ID_Tag, int extendSize)
    {
        int index = ID_Tag.transform.childCount;

        for (int i = index; i < index + extendSize; i++)
        {
            GameObject particleObj = Instantiate(ID_Tag.transform.GetChild(0).gameObject, Vector3.zero, Quaternion.identity);
            particleObj.name = ID_Tag.name + " (" + i + ")";
            particleObj.transform.parent = ID_Tag.transform;
            particleObj.SetActive(false);
            particleObj.layer = PARTICLE_LAYER;
        }

    }
}

