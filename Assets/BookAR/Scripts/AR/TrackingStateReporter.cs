using System.Collections.Generic;
using BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BookAR.Scripts.AR
{
    public class TrackingStateReporter
    {
        private int SMOOTHED_TRACKING_STATE_RECORDING_INTERVAL = 60; 
        private int nrRecordedTrackingStates = 0;
        private int averageTrackingState = 0;
        private ARTrackedImage rawTrackableData;
        private bool isSmoothingEnabled;
        private List<int> trackingStates = new List<int>();
        
        internal TrackingStateReporter(ARTrackedImage rawTrackableData, bool isSmoothingInitiallyEnabled )
        {
            changeTrackingMode(isSmoothingInitiallyEnabled);
            this.rawTrackableData = rawTrackableData;
            
        }
        
        public void changeTrackingMode(bool isTrackingStateSmoothingEnabled)
        {
            isSmoothingEnabled = isTrackingStateSmoothingEnabled;
            if (isSmoothingEnabled)
            {
                trackingStates = new List<int>();
                nrRecordedTrackingStates = 0;
                averageTrackingState = 0;
            }
        }
        
        public bool getTrackedImageState()
        {
            if (isSmoothingEnabled)
            {
                var intRawTrackingState = rawTrackableData.trackingState == TrackingState.Tracking ? 1 : 0;
                if (nrRecordedTrackingStates < SMOOTHED_TRACKING_STATE_RECORDING_INTERVAL)
                {
                    trackingStates.Add(intRawTrackingState);
                    nrRecordedTrackingStates++;
                    averageTrackingState += rawTrackableData.trackingState == TrackingState.Tracking ? 1 : 0;
                }
                else
                {
                    trackingStates.Add(intRawTrackingState);
                    averageTrackingState += rawTrackableData.trackingState == TrackingState.Tracking ? 1 : 0;
                    averageTrackingState -= trackingStates[0];
                    trackingStates.RemoveAt(0);
                }

                var completeAverageTrackingState = (float)averageTrackingState / nrRecordedTrackingStates;
                return completeAverageTrackingState> 0.5;
            }
            else
            {
                return rawTrackableData.trackingState == TrackingState.Tracking;
            }

            
        }
    }
}