using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

namespace Scenes.BookAR.Scripts
{
    
    public class ARVideoControl : MonoBehaviour
    {   
        [SerializeField]
        private VideoPlayer videoObject;
        [SerializeField]
        private Button playButton;
        // [SerializeField]
        // private GameObject hideButton;
        // private GameObject blurringCube;
        private void OnEnable()
        {
            // playButton = transform.GetChild(0).gameObject;
            // rawImage = transform.GetChild(1).gameObject;
            // hideButton = transform.GetChild(2).gameObject;
            // performVideoScaling();
            playButton.gameObject.SetActive(true);
            videoObject.gameObject.SetActive(false);
        }

        public void OnPlayButtonClick()
        {
            //not much to do, just hide this button and enable the raw image containing the video
            playButton.gameObject.SetActive(false);
            videoObject.gameObject.SetActive(true);
            // blurringCube.SetActive(true);
            // hideButton.SetActive(true);
        }

        public void OnPauseButtonClick()
        {
            playButton.gameObject.SetActive(true);
            videoObject.gameObject.SetActive(false);
            // hideButton.SetActive(false);
            // blurringCube.SetActive(false);


        }

        private void performVideoScaling()
        {
            throw new Exception("Perform scaling here is deprecated!");
            // var sizeContainer = transform.parent.GetComponent<ImageDimensionAware>();
            // if (sizeContainer == null)
            // {
            //     Debug.LogError("This should not happen. This component requires the asset to be dimension aware");
            // }
            //
            // // var parent = transform.parent;
            // // parent.localScale = new Vector3(sizeContainer.dimXaxis,1,sizeContainer.dimYaxis);
            // var rectTransform = GetComponent<RectTransform>();
            // rectTransform.sizeDelta = new Vector2(sizeContainer.dimXaxis, sizeContainer.dimYaxis);
            //
            //
            // var playButtonSize = transform.Find("PlayButton").GetComponent<RectTransform>();
            // var minLocalScalar = Mathf.Min(sizeContainer.dimXaxis, sizeContainer.dimYaxis);
            // playButtonSize.sizeDelta = new Vector2(minLocalScalar, minLocalScalar);
            //
            // //If this asset by any chance contains a "Blurring cube", then scale that as well:
            // blurringCube = transform.parent.Find("BlurringCube").gameObject;
            // if (blurringCube != null)
            // {
            //     blurringCube.transform.localScale = new Vector3( sizeContainer.dimXaxis*2,sizeContainer.dimYaxis*2, 0.01f);
            // }
            // else
            // {
            //     Debug.Log("Detected that this video asset does not have a 'BlurringCube'. Wouldnt it be nice if it had one? ");
            // }

        }
    }
}