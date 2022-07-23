using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    internal class PositionReporter :IPositionReporter
    {
        private ARTrackedImage trackableInfo;
        internal PositionReporter(ARTrackedImage trackableInfo )
        {
            this.trackableInfo = trackableInfo;
            var minLocalScalar = Mathf.Min(trackableInfo.size.x, trackableInfo.size.y) / 2;
            Debug.Log($"We have reached DEBUGGY, scaling asset by a scalar of {minLocalScalar}");
            Debug.Log($"Local scale before setting: {trackableInfo.transform.localScale}");
            trackableInfo.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
            Debug.Log($"Local scale AFTER setting: {trackableInfo.transform.localScale}");
        }

        public Transform getTransform()
        {
            return trackableInfo.transform;
        }

        public Vector2 getImageSize()
        {
            return trackableInfo.size;
        }

    }
}