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
    }
}