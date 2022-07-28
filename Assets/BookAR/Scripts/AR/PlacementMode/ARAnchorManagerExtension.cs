using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARTrackedImageManager))]

    public class ARAnchorManagerExtension : MonoBehaviour
    {
        private ARTrackedImageManager trackedImageManager;
        private ARAnchorManager anchorManager;

        void Awake()
        {
            trackedImageManager = GetComponent<ARTrackedImageManager>();
            anchorManager = GetComponent<ARAnchorManager>();
        }
        void OnEnable()
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
        void OnDisable()
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

        }
        
        void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                androidOnlyAddAnchor(eventArgs);
            }
        }

        private void androidOnlyAddAnchor(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                if (trackedImage.pending)
                {
                    Debug.LogError("We are instantiating and using trackedImage Position while it is still pending. We should not do this, please take into account this situation!");
                }

                var trackedImageTransform = trackedImage.transform;
                var result = anchorManager.subsystem.TryAttachAnchor(
                    trackedImage.trackableId,
                    new Pose(
                        trackedImageTransform.localPosition,
                        trackedImageTransform.localRotation
                    ),
                    out var anchor
                );

            }

            foreach (var trackedImage in eventArgs.removed)
            {
                Debug.LogError("trackedImages removed event cleanup not implemented yet");
            }
        }
    }
}