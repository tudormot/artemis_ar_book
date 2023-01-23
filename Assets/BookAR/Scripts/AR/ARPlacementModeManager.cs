using System;
using System.Collections.Generic;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.AR.PositionReporters;
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

        struct PlacementControlTrio
        {
            internal TrackingStateReporter trackingStateReporter;
            internal IPositionReporter posReporter;
            internal IPlacementController placementController;

            internal PlacementControlTrio(IPositionReporter posReporter, IPlacementController placementController,TrackingStateReporter trackingStateReporter)
            {
                this.posReporter = posReporter;
                this.placementController = placementController;
                this.trackingStateReporter = trackingStateReporter;
            }
        }

        private List<PlacementControlTrio> managedControlTrios =
             new List<PlacementControlTrio>();

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

                var trackingStateReporter = createTrackingStateReporter(
                    GlobalSettingsSingleton.instance.state.smoothTrackingStateReporting,
                    trackedImage
                );
                var posReporter = createPosReporter(GlobalSettingsSingleton.instance.state
                    .smoothPositionReporting, this, trackedImage, trackingStateReporter);
                
                var newTrio = new PlacementControlTrio(
                    posReporter,
                    createPlaceController(
                        posReporter,
                        GlobalSettingsSingleton.instance.state.placementUpdateMode,
                        this
                    ),
                    trackingStateReporter
                );
                managedControlTrios.Add(newTrio);
                newTrio.placementController.startPrefabPlacementControl(prefab,prefabInstantiatedAlready:false);
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
                var newManagedControlTrios = new List<PlacementControlTrio>();
                foreach (var placementControlTrio in managedControlTrios)
                {
                    var newTrio = new PlacementControlTrio(
                        placementControlTrio.posReporter,
                        createPlaceController(
                            placementControlTrio.posReporter,
                            data.newState.placementUpdateMode,
                            this),
                        placementControlTrio.trackingStateReporter
                    );
                    var managedObject = placementControlTrio.placementController.giveUpPrefabPlacementControl();
                    newTrio.placementController.startPrefabPlacementControl(managedObject,prefabInstantiatedAlready:true);
                    newManagedControlTrios.Add(newTrio);
                }
                managedControlTrios = newManagedControlTrios;
            }
            if (data.oldState.smoothPositionReporting != data.newState.smoothPositionReporting)
            {
                var newManagedControlTrios = new List<PlacementControlTrio>();
                foreach (var placementControlTrio in managedControlTrios)
                {
                    var newPosReporter = createPosReporter(
                        data.newState.smoothPositionReporting,
                        this,
                        placementControlTrio.posReporter.giveUpPositionReporting(),
                        placementControlTrio.trackingStateReporter
                    );
                    placementControlTrio.placementController.changePositionReporter(newPosReporter);
                    var newTrio = new PlacementControlTrio(
                        newPosReporter,
                        placementControlTrio.placementController,
                        placementControlTrio.trackingStateReporter
                    );
                    newManagedControlTrios.Add(newTrio);
                }
                managedControlTrios = newManagedControlTrios;
            }
            if (data.oldState.smoothTrackingStateReporting != data.newState.smoothTrackingStateReporting)
            {
                foreach (var placementControlTrio in managedControlTrios)
                {
                    placementControlTrio.trackingStateReporter.changeTrackingMode(
                        data.newState.smoothTrackingStateReporting
                        );
                }
            }
        }

        private TrackingStateReporter createTrackingStateReporter( bool smoothTrackingStateReporting, ARTrackedImage trackedImage)
        {
            return new TrackingStateReporter(Camera.main, trackedImage, smoothTrackingStateReporting);
        }
        private IPositionReporter createPosReporter( bool smoothPosReportingMode,
            MonoBehaviour context, ARTrackedImage trackedImage, TrackingStateReporter trackingStateReporter)
        {
            if (trackedImage == null)
            {
                Debug.Log("TrackedImage is null in createPosReporter!");
            }

            if (smoothPosReportingMode)
            {
                return new SmoothPositionReporter(
                    trackedImage,
                    context,
                    trackingStateReporter
                );
            }
            else
            {
                return new RawPositionReporter(
                    trackedImage,
                    trackingStateReporter
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