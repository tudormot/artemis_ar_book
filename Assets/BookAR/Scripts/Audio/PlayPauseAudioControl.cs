using System;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    public class PlayPauseAudioControl : MonoBehaviour
    {
        [SerializeField] private Canvas PlayCanvas;
        [SerializeField] private Canvas PauseCanvas;

        private void OnEnable()
        {
            PlayCanvas.gameObject.SetActive(true);
            PauseCanvas.gameObject.SetActive(false);

        }

        public void OnPlayPressed()
        {
            PlayCanvas.gameObject.SetActive(false);
            PauseCanvas.gameObject.SetActive(true);
        }

        public void OnPausePressed()
        {
            PlayCanvas.gameObject.SetActive(true);
            PauseCanvas.gameObject.SetActive(false);
        }
    }
}