using System;
using System.Collections.Generic;
using UnityEngine;

namespace BookAR.Scripts.AssetControl.Common
{
    public class ARExperienceQuitter:MonoBehaviour
    {
        [SerializeField] private List<GameObject> gameObjectsToDisable;
        [SerializeField] private String mainUIComponentName;

        private GameObject mainUIComponent;

        private List<bool> savedStatesGameObjectIsActive;
        private bool savedStateUIIsActive;
        
        public event EventHandler<ARExperienceState> ARExperienceChanged;

        public enum ARExperienceState
        {
            AR_EXPERIENCE_ENABLED,
            AR_EXPERIENCE_DISABLED
        }

        private void OnEnable()
        {
            savedStatesGameObjectIsActive = new List<bool>(new bool[gameObjectsToDisable.Count]);
            if (!string.IsNullOrEmpty(mainUIComponentName))
            {
                mainUIComponent = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find(mainUIComponentName).gameObject;
            }
        }

        public void enableARExperience()
        {
            ARExperienceChanged?.Invoke(this, ARExperienceState.AR_EXPERIENCE_ENABLED);
            for (var i = 0; i < gameObjectsToDisable.Count; i++)
            {
                gameObjectsToDisable[i].SetActive(savedStatesGameObjectIsActive[i]);
            }
            mainUIComponent?.SetActive(savedStateUIIsActive);
        }
        public void disableARExperience()
        {
            ARExperienceChanged?.Invoke(this, ARExperienceState.AR_EXPERIENCE_DISABLED);

            for (var i = 0; i < gameObjectsToDisable.Count; i++)
            {
                savedStatesGameObjectIsActive[i] = gameObjectsToDisable[i].activeSelf;
                gameObjectsToDisable[i].SetActive(false);
            }

            if (mainUIComponent != null)
            {
                savedStateUIIsActive = mainUIComponent.activeSelf;
            }
            mainUIComponent?.SetActive(false);

        }
        
    }
}