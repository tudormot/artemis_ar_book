using System;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    public class FlyTowards : MonoBehaviour
    {
        [SerializeField] public GameObject Target;
        [SerializeField] private float Speed = 10;

        private void OnEnable()
        {
            // var collider = transform.GetChild(0).GetComponent<MeshCollider>();
            // collider.
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("Seems like our rocket has hit something: " + other.ToString());
        }

        private void Update()
        {
            var step = Speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);
            transform.LookAt(Target.transform);
        }
    }
}