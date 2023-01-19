using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BookAR.Scripts.UI
{
    public class GlobalUIHider : MonoBehaviour
    {
        [SerializeField] private bool enableUI = true;
        [SerializeField] private GameObject debugCanvas1;
        [SerializeField] private GameObject debugCanvas2;
        [SerializeField] private GameObject debugCanvas3;
        private bool debugCanvas1SavedStateIsVisible;
        private bool debugCanvas2SavedStateIsVisible;
        private bool debugCanvas3SavedStateIsVisible;

        private GameObject consoleObj;

        
        [SerializeField] private Toggle isUIVisibleToggle;
        [SerializeField] private GameObject assetUIsObject;

        private void changeUIVisibility(bool shouldBeVisible)
        {
            if (shouldBeVisible)
            {
                if (enableUI == true)
                {
                    debugCanvas1?.SetActive(debugCanvas1SavedStateIsVisible);
                    debugCanvas2?.SetActive(debugCanvas2SavedStateIsVisible);
                    debugCanvas3?.SetActive(debugCanvas3SavedStateIsVisible);
                }
                assetUIsObject.SetActive(true);
                consoleObj?.SetActive(true);
                
            }
            else
            {
                debugCanvas1SavedStateIsVisible = debugCanvas1.activeSelf;
                debugCanvas2SavedStateIsVisible = debugCanvas2.activeSelf;
                debugCanvas3SavedStateIsVisible = debugCanvas3.activeSelf;
                assetUIsObject.SetActive(false);
                debugCanvas1?.SetActive(false);
                debugCanvas2?.SetActive(false);
                debugCanvas3?.SetActive(false);
                consoleObj?.SetActive(false);

            }
        }


        private void OnEnable()
        {
            consoleObj = GameObject.Find("IngameDebugConsole");
            if (consoleObj == null)
            {
                Debug.Log("In GlobalUIHider. Could not find an ingame console object!");
            }

            debugCanvas1SavedStateIsVisible = debugCanvas1.activeSelf;
            debugCanvas2SavedStateIsVisible = debugCanvas2.activeSelf;
            debugCanvas3SavedStateIsVisible = debugCanvas3.activeSelf;

            isUIVisibleToggle.onValueChanged.AddListener(
                (bool isUIsActive) =>
                {
                    changeUIVisibility(isUIsActive);
                }
            );
            changeUIVisibility(isUIVisibleToggle.isOn);
        }
    }
}