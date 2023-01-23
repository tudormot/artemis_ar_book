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
            trackingStateReporter.TrackingStateChanged += propagateEventFurther;

            if (trackableInfo == null)
            {
                Debug.Log("TrackableInfo null in RawPositionReporter-constructor!");
            }
            
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

            trackingStateReporter.TrackingStateChanged -= propagateEventFurther;

            return trackableInfo;
        }

        private void propagateEventFurther(CustomTrackingState state)
        {
            TrackingStateChanged?.Invoke(state);
        }

        public event IPositionReporter.TrackingEvent TrackingStateChanged;
    }
}