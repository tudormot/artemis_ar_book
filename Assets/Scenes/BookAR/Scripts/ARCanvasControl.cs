using System;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    public class ARCanvasControl : MonoBehaviour
    {
        private void Awake()
        {
            var worldARCamera = FindObjectOfType<Camera>();
            if (worldARCamera == null)
            {
                Debug.Log("Cant find world ar camera!");
            }

            gameObject.GetComponent<Canvas>().worldCamera = worldARCamera;
        }

        public void onStartButtonPressed()
        {
            Debug.Log("Pressed the start button! yay!");
        }
    }
}