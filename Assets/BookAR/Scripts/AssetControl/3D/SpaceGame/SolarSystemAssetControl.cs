using Scenes.BookAR.Scripts;
using ThirdPartyAssets.solar_system.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace BookAR.Scripts.AssetControl._3D
{
    /* These shitty required components are all thirs party scripts, part of the original solar system asset */
    [RequireComponent(typeof(GlobalSoundManager))]
    [RequireComponent(typeof(ShowInfo))]
    [RequireComponent(typeof(OpenCloseCanvas))]
    [RequireComponent(typeof(SpaceGameController))]
    public class SolarSystemAssetControl : MonoBehaviour, IAssetController
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;

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
            rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SolarSystemUI").gameObject;
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
            rootUIObj.transform.Find("StandardUI").gameObject.SetActive(false);
            rootUIObj.transform.Find("InfoUI").gameObject.SetActive(false);
            showInfoScript.enabled = false;
            Debug.Log("got here, enabling spaceGameController script");
            transform.GetComponent<SpaceGameController>().enabled = true;
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
            rootUIObj.SetActive(false);
        }
    }
}