using System;
using System.Collections.Generic;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BookAR.Scripts.AR
{
    public class TrackingStateReporter
    {
        private int SMOOTHED_TRACKING_STATE_RECORDING_INTERVAL = 30; 
        private int nrRecordedTrackingStates = 0;
        private int averageTrackingState = 0;
        private ARTrackedImage rawTrackableData;
        private bool isSmoothingEnabled;
        private List<int> trackingStates = new List<int>();
        private Camera worldCamera;

        private CustomTrackingState currentTrackingState = CustomTrackingState.FULL_TRACKING;
        public event IPositionReporter.TrackingEvent TrackingStateChanged;

        
        internal TrackingStateReporter(Camera worldCamera, ARTrackedImage rawTrackableData, bool isSmoothingInitiallyEnabled )
        {
            changeTrackingMode(isSmoothingInitiallyEnabled);
            this.rawTrackableData = rawTrackableData;
            this.worldCamera = worldCamera;

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
        
        private bool isInImageDetectionFrustum(Vector3 position)
        {
            /*detectionFrustumConstant has to do with the fact that the image does not get detected by the
             framework as soon as its center enters the camera frustum. Hence, the "imageDetectionFrustum"
             is smaller than the camera frustum*/
            const float detectionFrustumConstant = 0.2f;
            var uvCoord = worldCamera.WorldToViewportPoint(position);
            var isInImageDetectionFrustum = uvCoord.x is >= detectionFrustumConstant and <= (1-detectionFrustumConstant) &&
                                            uvCoord.y is >= detectionFrustumConstant and <= (1-detectionFrustumConstant) &&
                                            uvCoord.z >= 0;
            return isInImageDetectionFrustum;
        }

        private int redetectionSmoothing = 0;
        public CustomTrackingState getTrackedImageState()
        {
            if (isSmoothingEnabled)
            {
                if (rawTrackableData.trackingState == TrackingState.Tracking)
                {
                    if (currentTrackingState != CustomTrackingState.FULL_TRACKING)
                    {
                        redetectionSmoothing = 0;
                        currentTrackingState = CustomTrackingState.FULL_TRACKING;
                        TrackingStateChanged?.Invoke(CustomTrackingState.FULL_TRACKING);
                    }
                    return CustomTrackingState.FULL_TRACKING;
                }
                else
                {
                    if (isInImageDetectionFrustum(rawTrackableData.transform.localPosition))
                    {
                        
                        if (redetectionSmoothing >= 0)
                        {
                            redetectionSmoothing -= 1;
                            return CustomTrackingState.LIMITED;
                        }
                        else
                        {
                            if (currentTrackingState != CustomTrackingState.OCCLUDED)
                            {
                                Debug.Log("InTrackingStateReporter, OCCLUSION event!");
                                currentTrackingState = CustomTrackingState.OCCLUDED;
                                TrackingStateChanged?.Invoke(CustomTrackingState.OCCLUDED);
                            }
                            return CustomTrackingState.OCCLUDED;
                        }
                    }
                    else
                    {
                        if (redetectionSmoothing <= 60)
                        {
                            redetectionSmoothing++;
                        }
                        if (currentTrackingState != CustomTrackingState.LIMITED)
                        {
                            currentTrackingState = CustomTrackingState.LIMITED;
                            TrackingStateChanged?.Invoke(CustomTrackingState.LIMITED);
                        }
                        return CustomTrackingState.LIMITED;
                    }

                }
                /* old code:
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
                */
            }
            else
            {
                throw new Exception("Non smoothing image state not implemented for the time being");
                return CustomTrackingState.FULL_TRACKING;
            }

            
        }
    }
}

public enum CustomTrackingState
{
    FULL_TRACKING, LIMITED, OCCLUDED
}