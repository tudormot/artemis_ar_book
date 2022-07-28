using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BookAR.Scripts.AssetControl._2D
{
    
    public class ARVideoControl : MonoBehaviour, IAssetController
    {   
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.VIDEO_ASSET_TYPE;

        [SerializeField]
        private VideoPlayer videoObject;
        [SerializeField]
        private Button playButton;

        private void OnEnable()
        {

            playButton.gameObject.SetActive(true);
            videoObject.gameObject.SetActive(false);
        }

        public void OnPlayButtonClick()
        {
            playButton.gameObject.SetActive(false);
            videoObject.gameObject.SetActive(true);
        }

        public void OnPauseButtonClick()
        {
            playButton.gameObject.SetActive(true);
            videoObject.gameObject.SetActive(false);
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