using System;
using System.Collections.Generic;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.Global;
using Scenes.BookAR.Scripts;
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
                Debug.Log("Detected a trackable!");
                prefabImagePairManager.m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab);
                if (prefab == null)
                {
                    Debug.LogError("Could not find a prefab to instantiate for the detected image. Check the ImagePairManager, whether the pair exists.");
                }

                var posReporter = createPosReporter(GlobalSettingsSingleton.instance.state
                    .smoothPositionReporting, this, trackedImage);
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
                Debug.LogError("trackedImages removed event cleanup not implemented yet");
            }
            
        }

        private void OnGlobalSettingsChanged(object sender, GlobalSettingsEventData data)
        {
            Debug.Log("OnGlobalSettingsChanged!");
            if (data.oldState.placementUpdateMode != data.newState.placementUpdateMode)
            {
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
            if (data.oldState.smoothPositionReporting != data.newState.smoothPositionReporting)
            {
                var newManagedControlPairs = new List<PlacementControlPair>();
                foreach (var placementControlPair in managedControlPairs)
                {
                    var newPosReporter = createPosReporter(
                        data.newState.smoothPositionReporting,
                        this,
                        placementControlPair.posReporter.giveUpPositionReporting()
                    );
                    placementControlPair.placementController.changePositionReporter(newPosReporter);
                    var newPair = new PlacementControlPair(
                        newPosReporter,
                        placementControlPair.placementController
                    );
                    newManagedControlPairs.Add(newPair);
                }
                managedControlPairs = newManagedControlPairs;
            }

            
            
        }

        private IPositionReporter createPosReporter( bool smoothPosReportingMode,
            MonoBehaviour context, ARTrackedImage trackedImage)
        {
            if (trackedImage == null)
            {
                Debug.Log("TrackedImage is null in createPosReporter!");
            }

            if (smoothPosReportingMode)
            {
                return new SmoothPositionReporter(
                    trackedImage,
                    context
                );
            }
            else
            {
                return new RawPositionReporter(
                    trackedImage
                );
            }
           
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
        
    }
}