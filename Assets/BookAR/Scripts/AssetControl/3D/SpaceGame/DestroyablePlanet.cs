using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.XR.CoreUtils;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DestroyablePlanet : MonoBehaviour
    {
        [NonSerialized]
        public const int INITIAL_PLANET_HP = 30;
        [SerializeField] public int PlanetHP = INITIAL_PLANET_HP;
        public bool isShieldActive = false;

        [SerializeField] private ParticleSystem PlanetExplosionParticleSystem = null;
        [SerializeField] private ParticleSystem ShieldAbsorbParticleSystem = null;
        [SerializeField] private ParticleSystem TakeDamageParticleSystem = null;

        private MeshRenderer meshRenderer;
        private void OnEnable()
        {
            meshRenderer = transform.GetComponent<MeshRenderer>();
            if (PlanetExplosionParticleSystem == null || ShieldAbsorbParticleSystem == null || TakeDamageParticleSystem == null)
            {
                Debug.Log("Could not initiate damage taking system correctly!");
            }
        }

        public void OnHit(int damage)
        {
            PlanetHP -= damage;
            if (PlanetHP <= 0)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false; //to prevent any more hits, until planet is destroyed
                ExplodePlanet();
            }
            else
            {
                if (!isShieldActive)
                {
                    TakeDamageParticleSystem.Play();
                }
                else
                {
                    ShieldAbsorbParticleSystem.Play();
                    Debug.Log("Impact absorbed by shield system");
                }


            }

        }
        
        private void ExplodePlanet()
        {
            Debug.Log("Planet " + transform.name + " just exploded! wow!");
            var finalExplosion = Instantiate(PlanetExplosionParticleSystem, gameObject.transform.position,
                gameObject.transform.rotation);
            PlanetExplosionParticleSystem.Play();
            finalExplosion.Play();
            
             //destroy the final explosion in 3 seconds, this should give it plenty of time to play until the end
            StartCoroutine(hidePlanetWithDelay(finalExplosion));
        }
        
        private IEnumerator hidePlanetWithDelay(ParticleSystem finalExplosion)
        {
            const int DELAY = 3;
            yield return new WaitForSeconds(DELAY);
            if (finalExplosion != null)
            {
                Destroy(finalExplosion,3);
            }
            meshRenderer.enabled = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void resetPlanet()
        {
            PlanetHP = INITIAL_PLANET_HP;
            meshRenderer.enabled = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }




    }
}