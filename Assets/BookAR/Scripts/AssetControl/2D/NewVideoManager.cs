using System;
using System.Net.Mail;
using BookAR.Scripts.AssetControl.Common;
using BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace BookAR.Scripts.AssetControl._2D
{
    public class NewVideoManager : IAssetController, IDragHandler, IPointerDownHandler
    {
        public override AssetControllerType type { get; protected set; } = AssetControllerType.VIDEO_ASSET_TYPE;

        [SerializeField] private Button revealAssetButton;
        [SerializeField] private VideoPlayer smallScreenPlayer;
        [SerializeField] private RawImage smallScreenRawImage;
        [SerializeField] private Canvas smallScreenCanvas;

        private VideoPlayer bigScreenPlayer;
        private RawImage bigScreenRawImage;
        private GameObject bigScreenCanvas;

        private Image progress;
        private Button playButton;
        private Button pauseButton;
        private Button fullScreenButton;
        private Button smallScreenButton;
        private Button collapseVideoButton;
        private Button forwardButton;
        private Button reverseButton;

        private VideoPlayerState _state;
        private VideoPlayerState state
        {
            get => _state;
            set
            {
                onStateChanged(_state, value);
                _state = value;

            }
        }

        private enum VideoPlayerState
        {
            TOUCH_TO_INTERACT_STATE,
            HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE,
            HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE
        }

        void onStateChanged(VideoPlayerState oldState, VideoPlayerState newState)
        {
            switch (newState)
            {
                case VideoPlayerState.TOUCH_TO_INTERACT_STATE:
                    break;
                
                case VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE:
                    break;
                
                default:
                    break;
            }
        }

        void Update()
        {
            bool hasVideoControl = state != VideoPlayerState.TOUCH_TO_INTERACT_STATE &&
                                   !isOccluded;
            if (hasVideoControl && (smallScreenPlayer.isPlaying || bigScreenPlayer.isPlaying))
            {
                var currentPlayer = smallScreenPlayer;
                if (state == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE)
                {
                    currentPlayer = bigScreenPlayer;
                }

                if (currentPlayer.frameCount > 0)
                {
                    progress.fillAmount = (float)currentPlayer.frame / (float)currentPlayer.frameCount;

                }
            }
        }

        private void onPlayButtonPress()
        {
            playButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
                    
            var currentPlayer = smallScreenPlayer;
            if (state == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE)
            {
                currentPlayer = bigScreenPlayer;
            }
            currentPlayer.Play();
        }
        private void onPauseButtonPress()
        {
            playButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
            var currentPlayer = smallScreenPlayer;
            if (state == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE)
            {
                currentPlayer = bigScreenPlayer;
            }
            currentPlayer.Pause();
        }
        private void onFullScreenButtonPress()
        {
            fullScreenButton.gameObject.SetActive(false);
            smallScreenButton.gameObject.SetActive(true);
                    
                    
            bigScreenPlayer.frame = smallScreenPlayer.frame;
            bigScreenPlayer.gameObject.SetActive(true);
            if (smallScreenPlayer.isPlaying)
            {
                bigScreenPlayer.Play();
            }
            smallScreenPlayer.Pause();
            state = VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE;
        }
        private void onSmallScreenButtonPress()
        {
            fullScreenButton.gameObject.SetActive(true);
            smallScreenButton.gameObject.SetActive(false);
                    
            smallScreenPlayer.frame = bigScreenPlayer.frame;
            bigScreenPlayer.gameObject.SetActive(false);
            if (bigScreenPlayer.isPlaying)
            {
                smallScreenPlayer.Play();
            }
            bigScreenPlayer.Pause();
            state = VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE;

        }
        private void onCollapseButtonPress()
        {
            unbindFromBigUI();
            smallScreenPlayer.Pause();
            smallScreenPlayer.frame = 0;
            smallScreenRawImage.gameObject.SetActive(false);
            revealAssetButton.gameObject.SetActive(true);
            smallScreenCanvas.gameObject.SetActive(true);
            state = VideoPlayerState.TOUCH_TO_INTERACT_STATE;
        }

        private void bindToBigUI()
        {
            bigScreenCanvas =  GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find("FullScreenVideoCanvas").gameObject;
            bigScreenRawImage = bigScreenCanvas.transform.Find("FullScreenVideoPlayer").GetComponent<RawImage>();
            bigScreenPlayer = bigScreenCanvas.transform.Find("FullScreenVideoPlayer").GetComponent<VideoPlayer>();
            progress = bigScreenCanvas.transform.Find("VideoProgress").GetComponent<Image>();
            playButton = bigScreenCanvas.transform.Find("PlayButton").GetComponent<Button>();
            pauseButton = bigScreenCanvas.transform.Find("PauseButton").GetComponent<Button>();
            fullScreenButton = bigScreenCanvas.transform.Find("FullScreenButton").GetComponent<Button>();
            smallScreenButton = bigScreenCanvas.transform.Find("SmallScreenButton").GetComponent<Button>();
            collapseVideoButton = bigScreenCanvas.transform.Find("CollapseVideoButton").GetComponent<Button>();
            forwardButton = bigScreenCanvas.transform.Find("FastForwardButton").GetComponent<Button>();
            reverseButton = bigScreenCanvas.transform.Find("FastBackwardsButton").GetComponent<Button>();
            
            if (bigScreenCanvas == null || bigScreenRawImage == null || bigScreenPlayer == null ||
                progress == null || playButton == null || pauseButton == null ||
                fullScreenButton == null || smallScreenButton == null)
            {
                Debug.LogError("The NewVideoManagerScript this not manage to find all the required components in the fullScreenCanvas. Please Check");
            }
            
            
            playButton.onClick.AddListener(onPlayButtonPress);
            pauseButton.onClick.AddListener(onPauseButtonPress);
            fullScreenButton.onClick.AddListener(onFullScreenButtonPress);
            smallScreenButton.onClick.AddListener(onSmallScreenButtonPress);
            collapseVideoButton.onClick.AddListener(onCollapseButtonPress);
            forwardButton.onClick.AddListener(
                ()=> SSTools.ShowMessage("DEMO, not implemented.",SSTools.Position.top,SSTools.Time.threeSecond)
                );
            reverseButton.onClick.AddListener(
                ()=> SSTools.ShowMessage("DEMO, not implemented.",SSTools.Position.top,SSTools.Time.threeSecond)
            );
            
            bigScreenPlayer.clip = smallScreenPlayer.clip;
            
        }

        private void unbindFromBigUI()
        {
            bigScreenPlayer.Pause();
            bigScreenPlayer.frame = 0;
            bigScreenPlayer.gameObject.SetActive(false);
            bigScreenCanvas.SetActive(false);
            
            playButton.onClick.RemoveListener(onPlayButtonPress);
            pauseButton.onClick.RemoveListener(onPauseButtonPress);
            fullScreenButton.onClick.RemoveListener(onFullScreenButtonPress);
            smallScreenButton.onClick.RemoveListener(onSmallScreenButtonPress);
            collapseVideoButton.onClick.RemoveListener(onCollapseButtonPress);
            forwardButton.onClick.RemoveAllListeners();
            reverseButton.onClick.RemoveAllListeners();

            bigScreenCanvas = null;
            bigScreenRawImage = null;
            bigScreenPlayer = null;
            progress = null;
            playButton = null;
            pauseButton = null;
            fullScreenButton = null;
            smallScreenButton = null;
            collapseVideoButton = null;
            forwardButton = null;
            reverseButton = null;
        }

        private void OnEnable()
        {

            smallScreenCanvas.worldCamera = Camera.main;
            revealAssetButton.onClick.AddListener(
                () =>
                {
                    Debug.Log("DEBUG1");
                    base.onTouchToInteractButtonPressed();
                    Debug.Log("DEBUG2");

                    bindToBigUI();
                    Debug.Log("DEBUG3");

                    revealAssetButton.gameObject.SetActive(false);
                    Debug.Log("DEBUG4");

                    bigScreenCanvas.gameObject.SetActive(true);
                    Debug.Log("DEBUG5");

                    bigScreenPlayer.gameObject.SetActive(false);
                    Debug.Log("DEBUG6");

                    smallScreenPlayer.gameObject.SetActive(true);
                    Debug.Log("DEBUG7");

                    fullScreenButton.gameObject.SetActive(true);
                    Debug.Log("DEBUG8");

                    smallScreenButton.gameObject.SetActive(false);
                    Debug.Log("DEBUG9");

                    onPlayButtonPress();
                    Debug.Log("DEBUG10");

                    state = VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE;
                    Debug.Log("DEBUG11");

                }
            );
            state = VideoPlayerState.TOUCH_TO_INTERACT_STATE;
        }

        

        private void OnDisable()
        {
            //gameObject.GetComponent<ARExperienceQuitter>().ARExperienceChanged -= onARExperienceVisibilityChanged;
            //state = VideoPlayerState.AR_EXPERIENCE_DISABLED;
        }

        public void OnDrag(PointerEventData eventData)
        {
            TrySkip(eventData);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            TrySkip(eventData);
        }
        private void SkipToPercent(float pct)
        {
            var currentPlayer = smallScreenPlayer;
            if (state == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE)
            {
                currentPlayer = bigScreenPlayer;
            }

            var frame = currentPlayer.frameCount * pct;
            currentPlayer.frame = (long)frame;
        }
        private void TrySkip(PointerEventData eventData)
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    progress.rectTransform, eventData.position, null, out localPoint))
            {
                float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
                SkipToPercent(pct);
            }
        }

        //private VideoPlayerState preOcclusionState
        private bool isOccluded = false;
        public override void reactToCollapseRequest()
        {
            if(state != VideoPlayerState.TOUCH_TO_INTERACT_STATE){
                onCollapseButtonPress();
            }
            if (isOccluded)
            {
                smallScreenCanvas.gameObject.SetActive(false);
            }
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            if (e == OcclusionEvent.IMAGE_OCCLUDED)
            {
                isOccluded = true;
                if (state == VideoPlayerState.TOUCH_TO_INTERACT_STATE)
                {
                    revealAssetButton.gameObject.SetActive(false);
                }
                else 
                {
                    onPauseButtonPress();
                    smallScreenCanvas.gameObject.SetActive(false);
                    if (bigScreenCanvas != null)
                    {
                        bigScreenCanvas?.gameObject.SetActive(false);
                    }
                }

            }
            else
            {
                isOccluded = false;
                if (state == VideoPlayerState.TOUCH_TO_INTERACT_STATE)
                {
                    revealAssetButton.gameObject.SetActive(true);
                }
                else 
                {
                    onPauseButtonPress();
                    smallScreenCanvas.gameObject.SetActive(true);
                    if (bigScreenCanvas != null)
                    {
                        bigScreenCanvas?.gameObject.SetActive(true);
                    }
                }            }
        }

  
    }
}

