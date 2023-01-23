using System.Collections;
using BookAR.Scripts.AR.PlacementControllers;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.AssetControl;
using BookAR.Scripts.AssetControl.Common;
using UnityEngine;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ContinuousPlacementController:IPlacementController
    {
        private AssetScaler scaler;
        private IPositionReporter posReporter;
        private MonoBehaviour context;
        private GameObject controlledAsset;
        private Coroutine controlCoroutine;
        private Camera mainCamera;
        


        public ContinuousPlacementController(IPositionReporter posReporter, MonoBehaviour context)
        {
            this.posReporter = posReporter;
            this.context = context;
        }

        public void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready )
        {
            mainCamera = Camera.main;
            controlledAsset = prefabInstantiatedAlready ? prefab : Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            scaler = new AssetScaler(controlledAsset);
            controlCoroutine = context.StartCoroutine(updatePositionContinuously());
            posReporter.TrackingStateChanged += notifyAssetAboutTrackingStateChange;
        }
        
        public GameObject giveUpPrefabPlacementControl()
        {
            posReporter.TrackingStateChanged -= notifyAssetAboutTrackingStateChange;
            context.StopCoroutine(controlCoroutine);
            var assetOnWhichToGiveUp = controlledAsset;
            controlledAsset = null;
            return assetOnWhichToGiveUp;
        }

        public void changePositionReporter(IPositionReporter newReporter)
        {
            posReporter = newReporter;
        }

        private bool isOccluded = false; // we need this lil state here as we are treating both FULL_TRACKING and LIMITED as one state from this point onwards
        private void notifyAssetAboutTrackingStateChange(CustomTrackingState newState)
        {
            var controller = controlledAsset.GetComponent<IAssetController>();
            if (newState == CustomTrackingState.OCCLUDED)
            {
                Debug.Log("ContinuousPlacementController: sending OCCLUSION event");

                controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_OCCLUDED);
                isOccluded = true;

            }
            else if(isOccluded)
            {
                isOccluded = false;
                controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_NOT_OCCLUDED);

            }
        }


        private IEnumerator updatePositionContinuously()
        {
            while (true)
            {
                var imageData = posReporter.getImageData();
                controlledAsset.transform.localPosition = imageData.pos;
                controlledAsset.transform.localRotation = imageData.rot;
                controlledAsset.transform.localScale =
                    scaler.computeScalingForAsset(imageData.imageSize);
                
                /* old code:
                if (imageData.isTracked)
                {
                    if (state == PlacementControllerState.AR_ASSET_DISABLED)
                    {
                        state = PlacementControllerState.AR_ASSET_ENABLED;
                        var controller = controlledAsset.GetComponent<IAssetController>();
                        if (controller != null )
                        {
                            controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_NOT_OCCLUDED);
                        }
                        else
                        {
                            Debug.LogError("ERROR, reenabling an AR experience is not yet implemented! Quitter is not added to this asset");
                        }
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
                        if (isInImageDetectionFrustum(imageData.pos))
                        {
                            //if tracking state is limited but object is still in camera frustrum, it is most likely that the image has disappeared, IE the page of the AR book has
                            //been turned. Hence, stop the AR experience
                            state = PlacementControllerState.AR_ASSET_DISABLED;
                            var controller = controlledAsset.GetComponent<IAssetController>();
                            if (controller != null )
                            {
                                controller.reactToOcclusionEvent(OcclusionEvent.IMAGE_OCCLUDED);
                            }
                            else
                            {
                                Debug.LogError("ERROR, disabling an AR experience is not yet implemented! Quitter is not added to this asset");
                            }
                        }
                    }
                }*/

                
                yield return new WaitForEndOfFrame();
            }
        }

    }
}