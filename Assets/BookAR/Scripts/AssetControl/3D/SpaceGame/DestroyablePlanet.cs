using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    // [RequireComponent(typeof(ParticleSystem))]
    public class DestroyablePlanet : MonoBehaviour
    {
        [SerializeField] public int PlanetHP = 30;
        public bool isShieldActive = false;

        [SerializeField] private ParticleSystem PlanetExplosionParticleSystem = null;
        [SerializeField] private ParticleSystem ShieldAbsorbParticleSystem = null;
        [SerializeField] private ParticleSystem TakeDamageParticleSystem = null;
        private void OnEnable()
        {
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
            Destroy(finalExplosion,3); //destroy the final explosion in 3 seconds, this should give it plenty of time to play until the end
            gameObject.GetComponent<MeshRenderer>().enabled = false;

            Destroy(gameObject,3);
        }




    }
}