using System;
using System.Collections.Generic;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BookAR.Scripts.AR.PositionReporters
{
    internal class RawPositionReporter :IPositionReporter
    {
        private ARTrackedImage trackableInfo;
        private TrackingStateReporter trackingStateReporter;
        internal RawPositionReporter(ARTrackedImage trackableInfo, TrackingStateReporter trackingStateReporter )
        {
            this.trackableInfo = trackableInfo;
            this.trackingStateReporter = trackingStateReporter;

            if (trackableInfo == null)
            {
                Debug.Log("TrackableInfo null in RawPositionReporter-constructor!");
            }
            
            /*TODO: removed the following two lines from here, this functionality should not be here, it should be the job of the placement controllers to scale assets
             But if problems arise, investigate*/
            //var minLocalScalar = Mathf.Min(trackableInfo.size.x, trackableInfo.size.y) / 2;
            //trackableInfo.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
        }

        public TrackedImageData getImageData()
        {
            var transform = trackableInfo.transform;
            return new TrackedImageData()
            {
                pos = transform.localPosition,
                rot = transform.localRotation,
                imageSize = trackableInfo.size,
                isTracked = trackingStateReporter.getTrackedImageState()
            };
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