using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace BookAR.Scripts.AR.PlacementMode
{
    public interface IPositionReporter
    {
        TransformData getTransform();
        Vector2 getImageSize();

        public ARTrackedImage giveUpPositionReporting();
    }

    public struct TransformData
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;

    }
    
}