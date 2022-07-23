using System;
using System.Collections.Generic;
using Scenes.BookAR.Scripts;
using Scenes.BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    [RequireComponent(typeof(ARTrackedImageManager))]
    [RequireComponent(typeof(CustomPrefabImagePairManager))]

    public class ARPlacementModeManager : MonoBehaviour
    {        
        private ARTrackedImageManager trackedImageManager;
        private CustomPrefabImagePairManager prefabImagePairManager;

        struct PlacementControlPair
        { 
            internal IPositionReporter posReporter;
            internal IPlacementController placementController;

            internal PlacementControlPair(IPositionReporter posReporter, IPlacementController placementController)
            {
                this.posReporter = posReporter;
                this.placementController = placementController;
            }
        }

        private List<PlacementControlPair> managedControlPairs =
             new List<PlacementControlPair>();

        void Awake()
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();
            prefabImagePairManager = GetComponent<CustomPrefabImagePairManager>();
        }

        void OnEnable()
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
            GlobalSettingsSingleton.instance.GlobalSettingsChanged += OnGlobalSettingsChanged;
        }

        void OnDisable()
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
            GlobalSettingsSingleton.instance.GlobalSettingsChanged -= OnGlobalSettingsChanged;

        }
        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                prefabImagePairManager.m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab);
                if (prefab == null)
                {
                    Debug.LogError("Could not find a prefab to instantiate for the detected image. Check the ImagePairManager, whether the pair exists.");
                }

                var posReporter = new PositionReporter(trackedImage);
                var newPair = new PlacementControlPair(
                    posReporter,
                    createPlaceController(
                        posReporter,
                        GlobalSettingsSingleton.instance.state.placementUpdateMode,
                        this
                    )
                );
                managedControlPairs.Add(newPair);
                newPair.placementController.startPrefabPlacementControl(prefab,prefabInstantiatedAlready:false);
            }

            foreach (var trackedImage in eventArgs.removed)
            {
                throw new Exception("trackedImages removed event cleanup not implemented yet");
            }
            
        }

        private void OnGlobalSettingsChanged(object sender, GlobalSettingsEventData data)
        {
            Debug.Log("OnGlobalSettingsChanged!");
            if (data.oldState.placementUpdateMode == data.newState.placementUpdateMode) return;
            
            var newManagedControlPairs = new List<PlacementControlPair>();
            foreach (var placementControlPair in managedControlPairs)
            {
                var newPair = new PlacementControlPair(
                    placementControlPair.posReporter,
                    createPlaceController(
                        placementControlPair.posReporter,
                        data.newState.placementUpdateMode,
                        this)
                );
                var managedObject = placementControlPair.placementController.giveUpPrefabPlacementControl();
                newPair.placementController.startPrefabPlacementControl(managedObject,prefabInstantiatedAlready:true);
                newManagedControlPairs.Add(newPair);
            }
            managedControlPairs = newManagedControlPairs;
            
        }

        private IPlacementController createPlaceController(IPositionReporter posRep, AssetPlacementUpdateMode mode,
            MonoBehaviour context)
        {
            switch (mode)
            {
                case AssetPlacementUpdateMode.CONTINUOUS_UPDATE:
                {
                    return new ContinuousPlacementController(
                        posRep,
                        context
                    );
                }
                case AssetPlacementUpdateMode.UPDATE_ON_BUTTON_CLICK:
                {
                    return new ButtonBasedPlacementController(
                        posRep
                    );
                }
                default:
                {
                    throw new Exception("This control mode is not yet implemented, oops!");
                }
            }
        }

        private void AssignPrefab(ARTrackedImage trackedImage)
        {
            
            // Give the initial image a reasonable default scale
            //     var minLocalScalar = Mathf.Min(trackedImage.size.x, trackedImage.size.y) / 2;
            //     Debug.Log("In PrefabImagePairManager. Setting trackable to scale: "+minLocalScalar.ToString());
            //     trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
            // AssignPrefab(trackedImage);
            
            
            // if (m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab))
            // {
            //     if (ExtendedTrackableImageMode)
            //     {
            //         if (prefab == null)
            //         {
            //             Debug.Log("Prefab reported as null. PROBLEM");
            //         }
            //
            //         var ExtendedTrackable = trackedImage.transform.GetComponent<ARTrackedImageExtended>();
            //         if (ExtendedTrackable == null)
            //         {
            //             Debug.Log("ExtendedTrackable reported as null. PROBLEM");
            //         }
            //         ExtendedTrackable.Asset = prefab;
            //         ExtendedTrackable.enabled = true;
            //     }
            //     else
            //     {
            //         m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);
            //     }
            // }
            // else
            // {
            //     throw new Exception("Is this even possible? ");
            // }
        }
        
    }
}