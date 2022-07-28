using BookAR.Scripts.AR.PlacementControllers;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using Scenes.BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ButtonBasedPlacementController: IPlacementController
    {
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
            scaler = new AssetScaler(controlledAsset);
            updatePositionButton.onClick.AddListener(onUpdateButtonClick);
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
            Debug.Log("In changePositionReporter, this is not implemented yet");
            throw new System.NotImplementedException();
        }

        private void onUpdateButtonClick()
        {
            var imageData = posReporter.getImageData();
            controlledAsset.transform.localPosition = imageData.pos;
            controlledAsset.transform.localRotation = imageData.rot;
            controlledAsset.transform.localScale = 
                scaler.computeScalingForAsset(imageData.imageSize);

            
        }

    }
}