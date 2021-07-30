using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.BookAR.Scripts
{
    public class SpaceGameController : MonoBehaviour
    {
        private GameObject GameUI;
        private GameObject Gun;
        private GameObject SolarSystem;

        private void OnEnable()
        {
            //first add/enable gun stuck to the camera
            //secondly add/enable a script/object to each planet that can respond to hits
            //enable game gui: button available: exit game, shoot. also a label panel to communicate with the user
            
            GameUI = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SolarSystemUI").Find("GameUI").gameObject;
            Gun = GameObject.Find("AR Session Origin").transform.Find("AR Camera").Find("weapon01").gameObject;
            SolarSystem = transform.parent.Find("Solar System").gameObject;
            GameUI.SetActive(true);
            Gun.SetActive(true);
            
            GameUI.transform.Find("SunButton").GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    Debug.Log("SubButton clicked!");
                    var debugLabel = GameUI.transform.Find("DebugPanel").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
                    Debug.Log("Found the debug canvas label");
                    debugLabel.text = SolarSystem.transform.Find("Planets").Find("Sun").GetComponent<MeshRenderer>()
                        .bounds.size.ToString();
                    Debug.Log("button callback finished");
                });
            
        }

        private void OnDisable()
        {
            GameUI.SetActive(true);
            Gun.SetActive(true);
            GameUI.transform.Find("SunButton").GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}