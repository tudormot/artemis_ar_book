using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Scenes.BookAR.Scripts
{
    
    public class ARVideoControl : MonoBehaviour
    {
        private GameObject rawImage;
        private GameObject playButton;
        private GameObject hideButton;
        private GameObject BlurringCube;
        private void OnEnable()
        {
            playButton = transform.GetChild(0).gameObject;
            rawImage = transform.GetChild(1).gameObject;
            hideButton = transform.GetChild(2).gameObject;
            performVideoScaling();
            playButton.SetActive(true);
            rawImage.SetActive(false);
        }

        public void OnPlayButtonClick()
        {
            //not much to do, just hide this button and enable the raw image containing the video
            playButton.SetActive(false);
            rawImage.SetActive(true);
            BlurringCube.SetActive(true);
            hideButton.SetActive(true);
        }

        public void OnPauseButtonClick()
        {
            playButton.SetActive(true);
            rawImage.SetActive(false);
            hideButton.SetActive(false);
            BlurringCube.SetActive(false);


        }

        private void performVideoScaling()
        {
            var sizeContainer = transform.parent.GetComponent<ImageDimensionAware>();
            if (sizeContainer == null)
            {
                Debug.LogError("This should not happen. This component requires the asset to be dimension aware");
            }

            // var parent = transform.parent;
            // parent.localScale = new Vector3(sizeContainer.dimXaxis,1,sizeContainer.dimYaxis);
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(sizeContainer.dimXaxis, sizeContainer.dimYaxis);
            

            var playButtonSize = transform.Find("PlayButton").GetComponent<RectTransform>();
            var minLocalScalar = Mathf.Min(sizeContainer.dimXaxis, sizeContainer.dimYaxis);
            playButtonSize.sizeDelta = new Vector2(minLocalScalar, minLocalScalar);
            
            //If this asset by any chance contains a "Blurring cube", then scale that as well:
            BlurringCube = transform.parent.Find("BlurringCuber").gameObject;
            if (BlurringCube != null)
            {
                BlurringCube.transform.localScale = new Vector3( sizeContainer.dimXaxis*2,sizeContainer.dimYaxis*2, 0.01f);
            }
            else
            {
                Debug.Log("Detected that this video asset does not have a 'BlurringCuber'. Wouldnt it be nice if it had one? ");
            }

        }
    }
}