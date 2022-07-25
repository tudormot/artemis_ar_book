using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
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

        public TransformData getTransform()
        {
            var transform = trackableInfo.transform;
            return new TransformData()
            {
                pos = transform.localPosition,
                rot = transform.localRotation,
                scale = transform.localScale
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