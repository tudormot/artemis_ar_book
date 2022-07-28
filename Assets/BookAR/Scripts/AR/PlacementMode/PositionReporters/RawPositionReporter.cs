using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode.PositionReporters
{
    internal class RawPositionReporter :IPositionReporter
    {
        private ARTrackedImage trackableInfo;
        internal RawPositionReporter(ARTrackedImage trackableInfo )
        {
            this.trackableInfo = trackableInfo;
            if (trackableInfo == null)
            {
                Debug.Log("TrackableInfo null in RawPositionReporter-constructor!");
            }
            var minLocalScalar = Mathf.Min(trackableInfo.size.x, trackableInfo.size.y) / 2;
            trackableInfo.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
        }

        public TrackedImageData getImageData()
        {
            var transform = trackableInfo.transform;
            return new TrackedImageData()
            {
                pos = transform.localPosition,
                rot = transform.localRotation,
                imageSize = trackableInfo.size
            };
        }

        public Vector2 getImageSize()
        {
            return trackableInfo.size;
        }

        public ARTrackedImage giveUpPositionReporting()
        {
            /*nothing to deinitialize or do*/
            if (trackableInfo == null)
            {
                Debug.Log("TrackableInfo null in RawPositionReporter-giveUpPositionReporting");
            }

            return trackableInfo;
        }
    }
}