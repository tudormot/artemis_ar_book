using BookAR.Scripts.AR.PlacementControllers;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.AssetControl;
using BookAR.Scripts.AssetControl.Common;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ButtonBasedPlacementController : IPlacementController
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
            posReporter.TrackingStateChanged += notifyAssetAboutTrackingStateChange;
            controlledAsset = prefabInstantiatedAlready
                ? prefab
                : Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            state = PlacementControllerState.AR_ASSET_ENABLED;
            scaler = new AssetScaler(controlledAsset);
            updatePositionButton.onClick.AddListener(onUpdateButtonClick);
            onUpdateButtonClick(); // call once manually
        }

        public GameObject giveUpPrefabPlacementControl()
        {
            posReporter.TrackingStateChanged -= notifyAssetAboutTrackingStateChange;
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
            controlledAsset.transform.localPosition = imageData.pos;
            controlledAsset.transform.localRotation = imageData.rot;
            controlledAsset.transform.localScale =
                scaler.computeScalingForAsset(imageData.imageSize);
        /*
        if (imageData.isTracked != CustomTrackingState.OCCLUDED)
        {
            if (state == PlacementControllerState.AR_ASSET_DISABLED)
            {
                state = PlacementControllerState.AR_ASSET_ENABLED;
                var assetControl = controlledAsset.GetComponent<IAssetController>();
                if (assetControl != null )
                {
                    assetControl.reactToOcclusionEvent(OcclusionEvent.IMAGE_NOT_OCCLUDED);
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
                var assetControl = controlledAsset.GetComponent<IAssetController>();
                if (assetControl != null )
                {
                    assetControl.reactToOcclusionEvent(OcclusionEvent.IMAGE_OCCLUDED);

                }
                else
                {
                    Debug.LogError("ERROR, disabling an AR experience is not yet implemented! Quitter is not added to this asset");
                }
                
            }
        }*/
        
        }
        private bool
            isOccluded =
                false; // we need this lil state here as we are treating both FULL_TRACKING and LIMITED as one state from this point onwards

        private void notifyAssetAboutTrackingStateChange(CustomTrackingState newState)
        {
            var controller = controlledAsset.GetComponent<IAssetController>();
            if (newState == CustomTrackingState.OCCLUDED)
            {
                controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_OCCLUDED);
                isOccluded = true;

            }
            else if (isOccluded)
            {
                isOccluded = false;
                controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_NOT_OCCLUDED);

            }
        }

    }
}