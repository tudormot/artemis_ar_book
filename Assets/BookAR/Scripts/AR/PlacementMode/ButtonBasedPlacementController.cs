using Scenes.BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ButtonBasedPlacementController: IPlacementController
    {
        private IPositionReporter posReporter;
        private MonoBehaviour context;
        private GameObject controlledAsset;
        private Coroutine controlCoroutine;
        private Button updatePositionButton;
        public ButtonBasedPlacementController(IPositionReporter posReporter, MonoBehaviour context)
        {
            this.posReporter = posReporter;
            this.context = context;
            var buttonObj = GameObject.Find("ManualAssetUpdateButton");
            if (buttonObj == null)
            {
                Debug.LogError("Could not find button that would update asset position on click");
            }

            updatePositionButton = buttonObj.transform.GetComponent<Button>(); 
        }

        public void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready = false)
        {
            if (!prefabInstantiatedAlready)
            {
                controlledAsset = Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            }

            updatePositionButton.onClick.AddListener(onUpdateButtonClick);
        }
        
        public GameObject giveUpPrefabPlacementControl()
        {
            context.StopCoroutine(controlCoroutine);
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
            var transform = posReporter.getTransform();
            controlledAsset.transform.localPosition = transform.localPosition;
            controlledAsset.transform.localRotation = transform.localRotation;
            controlledAsset.transform.localScale = transform.localScale;
        }

        // private IEnumerator updatePositionContinuously()
        // {
        //     var transform = posReporter.getTransform();
        //     controlledAsset.transform.localPosition = transform.localPosition;
        //     controlledAsset.transform.localRotation = transform.localRotation;
        //     controlledAsset.transform.localScale = transform.localScale;
        //     yield return new WaitForEndOfFrame();
        // }
    }
}