using System.Collections;
using BookAR.Scripts.AR.PlacementControllers;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
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

        private enum PlacementControllerState
        {
            AR_ASSET_ENABLED,
            AR_ASSET_DISABLED  
        }

        private PlacementControllerState state;

        public ContinuousPlacementController(IPositionReporter posReporter, MonoBehaviour context)
        {
            this.posReporter = posReporter;
            this.context = context;
        }

        public void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready )
        {
            state = PlacementControllerState.AR_ASSET_ENABLED;
            mainCamera = Camera.main;
            controlledAsset = prefabInstantiatedAlready ? prefab : Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            scaler = new AssetScaler(controlledAsset);
            controlCoroutine = context.StartCoroutine(updatePositionContinuously());
        }
        
        public GameObject giveUpPrefabPlacementControl()
        {
            context.StopCoroutine(controlCoroutine);
            var assetOnWhichToGiveUp = controlledAsset;
            controlledAsset = null;
            return assetOnWhichToGiveUp;
        }

        public void changePositionReporter(IPositionReporter newReporter)
        {
            posReporter = newReporter;
        }

        private IEnumerator updatePositionContinuously()
        {
            while (true)
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
                        var uvCoord = mainCamera.WorldToViewportPoint(imageData.pos);
                        var isInCameraFrustum = uvCoord.x is >= 0 and <= 1 &&
                                                uvCoord.y is >= 0 and <= 1 &&
                                                uvCoord.z >= 0;
                        if (isInCameraFrustum)
                        {
                            //if tracking state is limited but object is still in camera frustrum, it is most likely that the image has disappeared, IE the page of the AR book has
                            //been turned. Hence, stop the AR experience
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

                
                yield return new WaitForEndOfFrame();
            }
        }

    }
}