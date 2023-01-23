using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode.PositionReporters
{
    public interface IPositionReporter
    {
        TrackedImageData getImageData();
        public ARTrackedImage giveUpPositionReporting();
        public delegate void TrackingEvent(CustomTrackingState state);
        public event TrackingEvent TrackingStateChanged;

        
    }
    public struct TrackedImageData
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector2 imageSize;
        public CustomTrackingState isTracked;
    }
    
    

}