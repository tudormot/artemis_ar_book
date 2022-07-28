using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode.PositionReporters
{
    public interface IPositionReporter
    {
        TrackedImageData getImageData();
        public ARTrackedImage giveUpPositionReporting();
        

        
    }
    public struct TrackedImageData
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector2 imageSize;
    }
    
    

}