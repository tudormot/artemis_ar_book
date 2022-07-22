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
        }

        public Transform getTransform()
        {
            return trackableInfo.transform;
        }
        
    }
}