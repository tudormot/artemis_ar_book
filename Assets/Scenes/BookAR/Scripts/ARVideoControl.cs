using System;
using UnityEngine;

namespace Scenes.BookAR.Scripts
{
    
    public class ARVideoControl : MonoBehaviour
    {
        private GameObject rawImage;
        private GameObject playButton;
        private void OnEnable()
        {
            playButton = transform.GetChild(0).gameObject;
            rawImage = transform.GetChild(1).gameObject;
            playButton.SetActive(true);
            rawImage.SetActive(false);
        }

        public void OnPlayButtonClick()
        {
            //not much to do, just hide this button and enable the raw image containing the video
            playButton.SetActive(false);
            rawImage.SetActive(true);
        }
    }
}