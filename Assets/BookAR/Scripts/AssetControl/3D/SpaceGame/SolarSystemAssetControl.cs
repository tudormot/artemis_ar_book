using BookAR.Scripts.AssetControl;
using Scenes.BookAR.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace BookAR.Scripts.AssetControl._3D.SpaceGame
{

    /* These shitty required components are all third party scripts, part of the original solar system asset */
    [RequireComponent(typeof(GlobalSoundManager))]
    [RequireComponent(typeof(ShowInfo))]
    [RequireComponent(typeof(OpenCloseCanvas))]
    [RequireComponent(typeof(SpaceGameController))]

    public class SolarSystemAssetControl : IAssetController
    {
        public override AssetControllerType type { get; protected set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        private bool isCurrentlyOccluded = false;
        private bool isCurrentlyInTouchToInteractState = true;
        private bool isSpaceGameActive = false;


        private GameObject rootUIObj;
        private GlobalSoundManager soundManagerScript;
        private ShowInfo showInfoScript;
        private OpenCloseCanvas openCloseCanvasScript;
        private SpaceGameController spaceGameController;

        [SerializeField]
        private Canvas touchToInteractCanvas;
        [SerializeField]
        private Button touchToInteractButton;
        [SerializeField]
        private GameObject solarSystemAsset;
        [SerializeField]
        private GameObject soundAsset;
        private void OnEnable()
        {   
            rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find("SolarSystemUI").gameObject;
            if(rootUIObj == null){
                Debug.LogError("Did not find solarSystem main canvas!");
            }
            
            touchToInteractCanvas.worldCamera = Camera.main;
            touchToInteractButton.onClick.AddListener(swapToMainAsset);

            soundManagerScript = GetComponent<GlobalSoundManager>();
            showInfoScript = GetComponent<ShowInfo>();
            openCloseCanvasScript = GetComponent<OpenCloseCanvas>();
            spaceGameController = GetComponent<SpaceGameController>();


            //set up things related to main solar system asset
            soundManagerScript.sndbutton =
                rootUIObj.transform.Find("StandardUI").transform.Find("SoundOnOff").GetComponent<Button>();
            openCloseCanvasScript.canvas = rootUIObj.transform.Find("InfoUI").gameObject;

            rootUIObj.transform.Find("StandardUI").Find("Toggle labels").GetComponent<Toggle>().onValueChanged.AddListener(
                b => { solarSystemAsset.GetComponent<SolarSystemManager>().ShowLabels = b; });
            rootUIObj.transform.Find("StandardUI").Find("Toggle paths").GetComponent<Toggle>().onValueChanged.AddListener(
                b => { solarSystemAsset.GetComponent<SolarSystemManager>().ShowPaths = b; });

            showInfoScript.DescriptionCanvas = rootUIObj.transform.Find("InfoUI").gameObject;
            showInfoScript.description = showInfoScript.DescriptionCanvas.transform.Find("Scroll View").GetChild(0).GetChild(0).GetChild(0)
                .GetComponent<Text>();


            //now connect stuff to the UI:
            rootUIObj.transform.Find("GameUI").Find("Buttons").Find("BackButton").GetComponent<Button>().onClick.AddListener(
                stopGameMode
            );
            rootUIObj.transform.Find("StandardUI").Find("Slider").GetComponent<sliderChange>().SolarSystem =
                solarSystemAsset;
            rootUIObj.transform.Find("StandardUI").Find("SoundOnOff").GetComponent<Button>().onClick.AddListener(soundManagerScript.SoundManager);
            rootUIObj.transform.Find("InfoUI").Find("Close").GetComponent<Button>().onClick.AddListener(openCloseCanvasScript.close);
            rootUIObj.transform.Find("StandardUI").Find("CollapseAssetButton").GetComponent<Button>().onClick.AddListener(swapToIntroAsset);
            rootUIObj.transform.Find("StandardUI").Find("GameModeButton").GetComponent<Button>().onClick.AddListener(startGameMode);
            
            //finally enable introAsset to start the experience:
            touchToInteractCanvas.gameObject.SetActive(true);
            spaceGameController.enabled = false;
            solarSystemAsset.SetActive(false);

        }

        private void swapToMainAsset()
        {
            base.onTouchToInteractButtonPressed();
            isCurrentlyInTouchToInteractState = false;
            touchToInteractCanvas.gameObject.SetActive(false);
            solarSystemAsset.SetActive(true);
            rootUIObj.SetActive(true);
            rootUIObj.transform.Find("StandardUI").gameObject.SetActive(true);

            soundManagerScript.enabled = true;
            showInfoScript.enabled = true;
            openCloseCanvasScript.enabled = true;


            soundAsset.SetActive(true);
        }

        private void swapToIntroAsset()
        {
            if (isSpaceGameActive)
            {
                stopGameMode();
            }

            isCurrentlyInTouchToInteractState = true;
            touchToInteractCanvas.gameObject.SetActive(true);
            solarSystemAsset.SetActive(false);
            rootUIObj.SetActive(false);

            soundManagerScript.enabled = false;
            showInfoScript.enabled = false;
            openCloseCanvasScript.enabled = false;

            soundAsset.SetActive(false);
            
        }

        private void startGameMode()
        {
            isSpaceGameActive = true;
            rootUIObj.transform.Find("StandardUI").gameObject.SetActive(false);
            rootUIObj.transform.Find("InfoUI").gameObject.SetActive(false);
            showInfoScript.enabled = false;
            spaceGameController.enabled = true;
        }

        private void stopGameMode()
        {
            isSpaceGameActive = false;
            rootUIObj.transform.Find("StandardUI").gameObject.SetActive(true);
            showInfoScript.enabled = true;
            spaceGameController.enabled = false;
            solarSystemAsset.GetComponent<SolarSystemManager>().ShowLabels = rootUIObj.transform
                .Find("StandardUI").Find("Toggle labels").GetComponent<Toggle>().isOn;
            solarSystemAsset.GetComponent<SolarSystemManager>().ShowPaths = rootUIObj.transform.Find("StandardUI")
                .Find("Toggle paths").GetComponent<Toggle>().isOn;

        }
        public override void reactToCollapseRequest()
        {
            swapToIntroAsset();
            if (isCurrentlyOccluded)
            {
                touchToInteractCanvas.gameObject.SetActive(false);
            }
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            if (e == OcclusionEvent.IMAGE_OCCLUDED)
            {
                isCurrentlyOccluded = true;
                if (isCurrentlyInTouchToInteractState)
                {
                    touchToInteractCanvas.gameObject.SetActive(false);
                }
                else
                {
                    if (isSpaceGameActive)
                    {
                        spaceGameController.HideGame();
                    }
                    else
                    {
                        rootUIObj.transform.Find("StandardUI").gameObject.SetActive(false);
                        rootUIObj.transform.Find("InfoUI").gameObject.SetActive(false);
                        showInfoScript.enabled = false;
                    }
                    solarSystemAsset.SetActive(false);
                    soundAsset.SetActive(false);
                    

                }
            }
            else
            {
                isCurrentlyOccluded = false;
                if (isCurrentlyInTouchToInteractState)
                {
                    touchToInteractCanvas.gameObject.SetActive(true);
                }
                else
                {
                    solarSystemAsset.SetActive(true);
                    soundAsset.SetActive(true);
                    if (isSpaceGameActive)
                    {
                        spaceGameController.RevealGame();
                    }
                    else
                    {
                        rootUIObj.transform.Find("StandardUI").gameObject.SetActive(true);
                        showInfoScript.enabled = true;
                    }
                }
            }
        }

        private void OnDisable()
        {
            //remove all the callbacks registered with the UI
            rootUIObj.transform.Find("StandardUI").Find("Slider").GetComponent<sliderChange>().SolarSystem =
                null;
            rootUIObj.transform.Find("StandardUI").Find("SoundOnOff").GetComponent<Button>().onClick.RemoveAllListeners();
            rootUIObj.transform.Find("StandardUI").Find("Toggle paths").GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            rootUIObj.transform.Find("StandardUI").Find("Toggle labels").GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            rootUIObj.transform.Find("InfoUI").Find("Close").GetComponent<Button>().onClick.RemoveAllListeners();
            rootUIObj.transform.Find("StandardUI").Find("CollapseAssetButton").GetComponent<Button>().onClick.RemoveAllListeners();
            rootUIObj.transform.Find("StandardUI").Find("GameModeButton").GetComponent<Button>().onClick.RemoveAllListeners();
            rootUIObj.transform.Find("GameUI").Find("Buttons").Find("BackButton").GetComponent<Button>().onClick
                .RemoveAllListeners();
            rootUIObj.SetActive(false);
        }
    }
}