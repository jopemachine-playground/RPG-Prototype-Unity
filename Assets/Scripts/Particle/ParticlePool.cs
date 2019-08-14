// ==============================+===============================================================
// @ Author : jopemachine
// @ Created : 2019-02-21, 11:02:28
// ==============================+===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// enum으로 타입을 분류해, 관리하는 파티클 풀을 다르게 할 수 있다. attackPool과 getItemPool를 이용해 싱글톤처럼 쓰지만, 싱글톤이 아님에 주의.
/// 하지만, 각 파티클 풀은 1개만 있어야 한다. 파티클을 미리 생성하고 활성화 해 놓기 때문에, 모든 파티클은 PlayOnAwake가 false여야 한다.
/// 던전 씬에서 바로 시작하면 FindObjectsOfType가 이 시점에 particleLists를 찾지 못해 파티클 풀이 생성되지 않는 버그가 있다.
/// 마을 등의 다른 씬에서 시작하면 이 버그는 발생하지 않는다.
/// </summary>

namespace UnityChanRPG
{
    public class ParticlePool : MonoBehaviour
    {
        public static ParticlePool attackParticle;
        public static ParticlePool pickupItemParticle;

        private LayerMask PARTICLE_LAYER;

        public enum PoolType
        {
            Attack,
            PickUpItem
        }

        public PoolType type;

        private delegate void InitPool();

        private InitPool init;

        private void Start()
        {
            PARTICLE_LAYER = LayerMask.NameToLayer("Particle");

            switch (type)
            {
                case PoolType.Attack:
                    attackParticle = this;
                    init += InitAttackParticlePool;
                    break;
                case PoolType.PickUpItem:
                    pickupItemParticle = this;
                    init += InitPickUpItemParticlePool;
                    break;
            }

            init();

        }

        private void InitAttackParticlePool()
        {
            AttackParticleList[] particleLists = FindObjectsOfType<AttackParticleList>();

            for (int i = 0; i < particleLists.Length; i++)
            {
                for (int j = 0; j < particleLists[i].particleList.Count; j++)
                {
                    GameObject ID_Tag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Destroy(ID_Tag.gameObject.GetComponent<Collider>());
                    Destroy(ID_Tag.gameObject.GetComponent<MeshRenderer>());
                    ID_Tag.name = particleLists[i].particleList[j].ID + "";
                    ID_Tag.transform.parent = gameObject.transform;
                    ID_Tag.layer = PARTICLE_LAYER;

                    for (int k = 0; k < particleLists[i].particleList[j].defaultParticlesNumber; k++)
                    {
                        GameObject particleObj = Instantiate(particleLists[i].particleList[j].particle.gameObject, Vector3.zero, Quaternion.identity);
                        particleObj.name = ID_Tag.name + " (" + k + ")";
                        particleObj.transform.parent = ID_Tag.transform;
                        particleObj.layer = PARTICLE_LAYER;
                    }
                    particleLists[i].particleList[j].currentParticlesNumber = particleLists[i].particleList[j].defaultParticlesNumber;
                }
            }
        }

        private void InitPickUpItemParticlePool()
        {
            PickUpItemParticleList pickUpItemParticles = gameObject.GetComponent<PickUpItemParticleList>();

            for (int i = 0; i < pickUpItemParticles.particleList.Count; i++)
            {
                GameObject ID_Tag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Destroy(ID_Tag.gameObject.GetComponent<Collider>());
                Destroy(ID_Tag.gameObject.GetComponent<MeshRenderer>());
                ID_Tag.name = pickUpItemParticles.particleList[i].ID + "";
                ID_Tag.transform.parent = gameObject.transform;

                for (int j = 0; j < pickUpItemParticles.particleList[i].defaultParticlesNumber; j++)
                {
                    GameObject particle = Instantiate(pickUpItemParticles.particleList[i].particle.gameObject, Vector3.zero, Quaternion.identity);
                    particle.transform.parent = ID_Tag.transform;
                    particle.name = pickUpItemParticles.particleList[i].ID + " (" + j + ")";
                }
            }

        }

        public void CallParticle(int particleID, Vector3 emitPosition)
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
}