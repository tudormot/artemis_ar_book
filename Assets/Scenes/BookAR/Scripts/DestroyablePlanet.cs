using System;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    // [RequireComponent(typeof(ParticleSystem))]
    public class DestroyablePlanet : MonoBehaviour
    {
        [SerializeField] private int PlanetHP = 30;
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
                    Debug.Log("Planet: " + transform.name + ". Life left: " + PlanetHP.ToString());
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
            PlanetExplosionParticleSystem.Play();
            Destroy(gameObject);
        }




    }
}