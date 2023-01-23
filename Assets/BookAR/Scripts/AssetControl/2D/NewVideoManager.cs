using System;
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
        public override void reactToCollapseRequest()
        {
            Debug.LogError("reactToCollapseRequest not implemented for video manager");
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            Debug.LogError("reactToOcclusionEvent not implemented for video manager");
        }

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
            AR_EXPERIENCE_DISABLED,
            TOUCH_TO_INTERACT_STATE,
            HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE,
            HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE
        }

        void onStateChanged(VideoPlayerState oldState, VideoPlayerState newState)
        {
            switch (newState)
            {
                case VideoPlayerState.TOUCH_TO_INTERACT_STATE:
                    revealAssetButton.gameObject.SetActive(true);
                    smallScreenCanvas.gameObject.SetActive(true);
                    bigScreenPlayer.gameObject.SetActive(false);
                    break;

                case VideoPlayerState.AR_EXPERIENCE_DISABLED:
                    if (oldState == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_BIG_PLAYER_ACTIVE ||
                        oldState == VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE)
                    {
                        bigScreenPlayer.Pause();
                        bigScreenPlayer.frame = 0;
                        smallScreenPlayer.Pause();
                        smallScreenPlayer.frame = 0;
                        bigScreenCanvas.SetActive(false);
                        smallScreenRawImage.gameObject.SetActive(false);
                        //GlobalSettingsSingleton.instance.state.isVideoPlayerUIInUse = false;
                        playButton.onClick.RemoveListener(onPlayButtonPress);
                        pauseButton.onClick.RemoveListener(onPauseButtonPress);
                        fullScreenButton.onClick.RemoveListener(onFullScreenButtonPress);
                        smallScreenButton.onClick.RemoveListener(onSmallScreenButtonPress);
                        collapseVideoButton.onClick.RemoveListener(onCollapseButtonPress);
                    }
                    smallScreenCanvas.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        void Update()
        {
            if (smallScreenPlayer.isPlaying || bigScreenPlayer.isPlaying)
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
            Debug.LogError("Collapse button is not implemented yet, but it is very important!");
        }

        private void OnEnable()
        {
            Debug.Log("in NewVideoManager OnEnable!");
            bigScreenCanvas =  GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find("FullScreenVideoCanvas").gameObject;
            bigScreenRawImage = bigScreenCanvas.transform.Find("FullScreenVideoPlayer").GetComponent<RawImage>();
            bigScreenPlayer = bigScreenCanvas.transform.Find("FullScreenVideoPlayer").GetComponent<VideoPlayer>();
            progress = bigScreenCanvas.transform.Find("VideoProgress").GetComponent<Image>();
            playButton = bigScreenCanvas.transform.Find("PlayButton").GetComponent<Button>();
            pauseButton = bigScreenCanvas.transform.Find("PauseButton").GetComponent<Button>();
            fullScreenButton = bigScreenCanvas.transform.Find("FullScreenButton").GetComponent<Button>();
            smallScreenButton = bigScreenCanvas.transform.Find("SmallScreenButton").GetComponent<Button>();
            collapseVideoButton = bigScreenCanvas.transform.Find("CollapseVideoButton").GetComponent<Button>();

            if (bigScreenCanvas == null || bigScreenRawImage == null || bigScreenPlayer == null ||
                progress == null || playButton == null || pauseButton == null ||
                fullScreenButton == null || smallScreenButton == null)
            {
                Debug.LogError("The NewVideoManagerScript this not manage to find all the required components in the fullScreenCanvas. Please Check");
            }

            bigScreenPlayer.clip = smallScreenPlayer.clip;
            var videoOriginalWidth = smallScreenPlayer.clip.width;
            var videoOriginalHeight = smallScreenPlayer.clip.height;
            var textureBestWidth =(int) Math.Pow(2 ,(int)Math.Log(videoOriginalWidth, 2));
            var textureBestHeight =(int) Math.Pow(2, (int)Math.Log(videoOriginalHeight, 2));
            smallScreenCanvas.worldCamera = Camera.main;
            
        
            revealAssetButton.onClick.AddListener(
                () =>
                {
                    //if (GlobalSettingsSingleton.instance.state.isVideoPlayerUIInUse)
                    if (false)

                    {
                        Debug.Log("Another video player instance is using the video player UI, collapse that player before trying to start this video (or turn the page).");
                    }
                    else
                    {
                        //GlobalSettingsSingleton.instance.state.isVideoPlayerUIInUse = true;
                        revealAssetButton.gameObject.SetActive(false);
                        bigScreenCanvas.gameObject.SetActive(true);
                        bigScreenPlayer.gameObject.SetActive(false);
                        smallScreenPlayer.gameObject.SetActive(true);
                        smallScreenPlayer.Play();
                        fullScreenButton.gameObject.SetActive(true);
                        smallScreenButton.gameObject.SetActive(false);
                        playButton.gameObject.SetActive(false);
                        pauseButton.gameObject.SetActive(true);
                        playButton.onClick.AddListener(onPlayButtonPress);
                        pauseButton.onClick.AddListener(onPauseButtonPress);
                        fullScreenButton.onClick.AddListener(onFullScreenButtonPress);
                        smallScreenButton.onClick.AddListener(onSmallScreenButtonPress);
                        collapseVideoButton.onClick.AddListener(onCollapseButtonPress);
                        state = VideoPlayerState.HAS_CONTROL_OF_VIDEO_PLAYER_UI_SMALL_PLAYER_ACTIVE;
                    }
                }
            );

            //gameObject.GetComponent<ARExperienceQuitter>().ARExperienceChanged += onARExperienceVisibilityChanged;


        }

        /*private void onARExperienceVisibilityChanged(object obj, ARExperienceQuitter.ARExperienceState visibilityState)
        {
            if (visibilityState == ARExperienceQuitter.ARExperienceState.AR_EXPERIENCE_ENABLED)
            {
                state = VideoPlayerState.TOUCH_TO_INTERACT_STATE;
            }
            else
            {
                //AR experience disabled
                state = VideoPlayerState.AR_EXPERIENCE_DISABLED;
            }
        }*/

        private void OnDisable()
        {
            //gameObject.GetComponent<ARExperienceQuitter>().ARExperienceChanged -= onARExperienceVisibilityChanged;
            state = VideoPlayerState.AR_EXPERIENCE_DISABLED;
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

  
    }
}

