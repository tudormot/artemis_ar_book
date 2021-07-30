using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.BookAR.Scripts
{
    public class SolarSystemAssetControl : MonoBehaviour
    {
        private GameObject rootUIObj;
        private GameObject touchToInteractObj;
        private GameObject solarSystemAsset;
        private GameObject scriptsAsset;
        private GameObject soundAsset;
        private void OnEnable()
        {
            //set callback for touchtointeractbutton, to enable the solarsystemasset
            //perform all the linking between solarsystemandui
            //enable the touch to interact asset, disable the solar system asset

            
            rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SolarSystemUI").gameObject;
            touchToInteractObj = transform.parent.Find("TouchToInteractCanvas").gameObject;
            solarSystemAsset = transform.parent.Find("Solar System").gameObject;
            scriptsAsset = transform.parent.Find("Scripts").gameObject;
            soundAsset = transform.parent.Find("Space Audio Source").gameObject;
            
            touchToInteractObj.transform.GetChild(0).GetComponent<Canvas>().worldCamera = Camera.main;
            touchToInteractObj.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(swapToMainAsset);

            //set up things related to main solar system asset
            scriptsAsset.GetComponent<GlobalSoundManager>().sndbutton =
                rootUIObj.transform.Find("StandardUI").transform.Find("SoundOnOff").GetComponent<Button>();
            scriptsAsset.GetComponent<OpenCloseCanvas>().canvas = rootUIObj.transform.Find("InfoUI").gameObject;
            var obj = scriptsAsset.GetComponent<toggleChange>();
            obj.labelsToggle = rootUIObj.transform.Find("StandardUI").transform.Find("Toggle labels").GetComponent<Toggle>();
            obj.pathsToggle = rootUIObj.transform.Find("StandardUI").transform.Find("Toggle paths").GetComponent<Toggle>();
            obj.SolarSystem = solarSystemAsset;
            var obj2 = scriptsAsset.GetComponent<ShowInfo>();
            obj2.DescriptionCanvas = rootUIObj.transform.Find("InfoUI").gameObject;
            obj2.description = obj2.DescriptionCanvas.transform.Find("Scroll View").GetChild(0).GetChild(0).GetChild(0)
                .GetComponent<Text>();

            //now connect stuff to the UI:
            rootUIObj.transform.Find("StandardUI").Find("Slider").GetComponent<sliderChange>().SolarSystem =
                solarSystemAsset;
            rootUIObj.transform.Find("StandardUI").Find("SoundOnOff").GetComponent<Button>().onClick.AddListener(scriptsAsset.GetComponent<GlobalSoundManager>().SoundManager);
            rootUIObj.transform.Find("StandardUI").Find("Toggle paths").GetComponent<Toggle>().onValueChanged.AddListener(b=>scriptsAsset.GetComponent<toggleChange>().TogglePaths());
            rootUIObj.transform.Find("StandardUI").Find("Toggle labels").GetComponent<Toggle>().onValueChanged.AddListener(b=>scriptsAsset.GetComponent<toggleChange>().ToggleLabels());
            rootUIObj.transform.Find("InfoUI").Find("Close").GetComponent<Button>().onClick.AddListener(scriptsAsset.GetComponent<OpenCloseCanvas>().close);
            rootUIObj.transform.Find("StandardUI").Find("CollapseAssetButton").GetComponent<Button>().onClick.AddListener(swapToIntroAsset);
            rootUIObj.transform.Find("StandardUI").Find("GameModeButton").GetComponent<Button>().onClick.AddListener(startGameMode);
            
            //finally enable introAsset to start the experience:
            touchToInteractObj.SetActive(true);
            


        }

        private void swapToMainAsset()
        {
            touchToInteractObj.SetActive(false);
            solarSystemAsset.SetActive(true);
            rootUIObj.SetActive(true);
            scriptsAsset.SetActive(true);
            soundAsset.SetActive(true);
        }

        private void swapToIntroAsset()
        {
            touchToInteractObj.SetActive(true);
            solarSystemAsset.SetActive(false);
            rootUIObj.SetActive(false);
            scriptsAsset.SetActive(false);
            soundAsset.SetActive(false);
            
        }

        private void startGameMode()
        {
            rootUIObj.transform.Find("StandardUI").gameObject.SetActive(false);
            rootUIObj.transform.Find("InfoUI").gameObject.SetActive(false);
            scriptsAsset.GetComponent<ShowInfo>().enabled = false;
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