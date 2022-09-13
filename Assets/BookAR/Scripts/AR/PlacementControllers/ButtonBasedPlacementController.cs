using BookAR.Scripts.AR.PlacementControllers;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.AssetControl.Common;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ButtonBasedPlacementController: IPlacementController
    {
        private enum PlacementControllerState
        {
            AR_ASSET_ENABLED,
            AR_ASSET_DISABLED  
        }
        private PlacementControllerState state;
        
        private AssetScaler scaler;
        private IPositionReporter posReporter;
        private GameObject controlledAsset;
        private Button updatePositionButton;
        public ButtonBasedPlacementController(IPositionReporter posReporter)
        {
            this.posReporter = posReporter;
            var buttonObj = GameObject.Find("ManualAssetUpdateButton");
            if (buttonObj == null)
            {
                Debug.LogError("Could not find button that would update asset position on click");
            }
            updatePositionButton = buttonObj.transform.GetComponent<Button>(); 
        }

        public void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready = false)
        {

            controlledAsset = prefabInstantiatedAlready ? prefab : Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            state = PlacementControllerState.AR_ASSET_ENABLED;
            scaler = new AssetScaler(controlledAsset);
            updatePositionButton.onClick.AddListener(onUpdateButtonClick);
            onUpdateButtonClick(); // call once manually
        }
        
        public GameObject giveUpPrefabPlacementControl()
        {
            var assetOnWhichToGiveUp = controlledAsset;
            controlledAsset = null;
            updatePositionButton.onClick.RemoveListener(onUpdateButtonClick);
            return assetOnWhichToGiveUp;
        }

        public void changePositionReporter(IPositionReporter newReporter)
        {
            posReporter = newReporter;
        }

        private void onUpdateButtonClick()
        {
            var imageData = posReporter.getImageData();
            if (imageData.isTracked)
            {
                if (state == PlacementControllerState.AR_ASSET_DISABLED)
                {
                    state = PlacementControllerState.AR_ASSET_ENABLED;
                    var quitter = controlledAsset.GetComponent<ARExperienceQuitter>();
                    if (quitter != null )
                    {
                        quitter.enableARExperience();
                    }
                    else
                    {
                        Debug.LogError("ERROR, reenabling an AR experience is not yet implemented! Quitter is not added to this asset");
                    };
                }

                controlledAsset.transform.localPosition = imageData.pos;
                controlledAsset.transform.localRotation = imageData.rot;
                controlledAsset.transform.localScale = 
                    scaler.computeScalingForAsset(imageData.imageSize);
            }
            else
            {
                if (state == PlacementControllerState.AR_ASSET_ENABLED)
                {
                    state = PlacementControllerState.AR_ASSET_DISABLED;
                    var quitter = controlledAsset.GetComponent<ARExperienceQuitter>();
                    if (quitter != null )
                    {
                        quitter.disableARExperience();
                    }
                    else
                    {
                        Debug.LogError("ERROR, disabling an AR experience is not yet implemented! Quitter is not added to this asset");
                    }
                    
                }
            }




        }

    }
}