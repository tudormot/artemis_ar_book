using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scenes.BookAR.Scripts
{
    public class FlyTowards : MonoBehaviour
    {
        [SerializeField] public GameObject Target;
        [SerializeField] private float Speed = 10;
        [SerializeField] private ParticleSystem MissileDestroyedEffect;

        private const int damage = 10;

        private void OnEnable()
        {
            // var collider = transform.GetChild(0).GetComponent<MeshCollider>();
            // collider.
        }

        private void OnCollisionEnter(Collision other)
        {
            
            Assert.AreEqual(Target,other.gameObject,"Hmm, we hit another target for some reason? "+ Target.ToString() + other.gameObject.ToString());
            other.gameObject.GetComponent<DestroyablePlanet>().OnHit(damage);

            DestroyMissile();

        }

        private void Update()
        {
            if (Target != null)
            {
                var step = Speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);
                transform.LookAt(Target.transform);
            }
            else
            {
                //this means that the planet was destroyed while this missile is in the air.. set this missile to autodestroy aswell
                DestroyMissile();
            }
        }

        private void DestroyMissile()
        {
            if (MissileDestroyedEffect != null)
            {
                MissileDestroyedEffect.Play();
            }
            Destroy(gameObject);
        }

    }
}