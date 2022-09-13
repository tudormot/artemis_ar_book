using System;
using System.Collections;
using System.Collections.Generic;
using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class SmoothPositionReporter:IPositionReporter
    {
        private readonly ARTrackedImage trackableRawData;
        private readonly TrackingStateReporter trackingStateReporter;

        private readonly MonoBehaviour context;
        private readonly Coroutine posCalculatingCoroutine;
        private readonly Queue<Vector3> positions = new Queue<Vector3>();

        public Vector3 smoothedPosition;
        public Quaternion smoothedRotation;

        public SmoothPositionReporter(ARTrackedImage trackable, MonoBehaviour context, TrackingStateReporter trackingStateReporter)
        {
            this.context = context;
            this.trackingStateReporter = trackingStateReporter;
            trackableRawData = trackable;
            
            if (trackableRawData == null)
            {
                Debug.Log("trackableRawData is null in SmoothPositionReporter-constructor!!!");

            }

            posCalculatingCoroutine = context.StartCoroutine(
                calculateSmoothTransform());
            if (posCalculatingCoroutine == null)
            {
                Debug.Log("posCalculatingCoroutine is null ! giveUpPositionReporting");
            }
        }

        public ARTrackedImage giveUpPositionReporting()
        {
            if (trackableRawData == null)
            {
                Debug.Log("wtf, how if trackableRawDAta null here?");
            }

            if (posCalculatingCoroutine == null)
            {
                Debug.Log("posCalculatingCoroutine is null ! giveUpPositionReporting");
            }

            context.StopCoroutine(posCalculatingCoroutine);
            if (trackableRawData == null)
            {
                Debug.Log("wtf, how if trackableRawDAta null here?");
            }
            return trackableRawData;
        }

        public TrackedImageData getImageData()
        {
            return new TrackedImageData()
            {
                pos = smoothedPosition,
                rot = smoothedRotation,
                imageSize = trackableRawData.size,
                isTracked = trackingStateReporter.getTrackedImageState()

            };
        }

        private IEnumerator calculateSmoothTransform()
            /*see wikipedia article on moving averages.
             Chose something which I think is pretty much equivalent to EMA for quaternion 
             average, as I don't want to get involved with quaternion arithmetics*/
        {
            int measIntervalInFrames = 30; //across how many frames the running mean is computed
            int measGapInFrames = 1;          //gap between recording of raw position data
            float RotationEMAParam = 0.2f;

            var queueMaxDim = measIntervalInFrames / measGapInFrames;
            Vector3 posSum = Vector3.zero;
            smoothedRotation = trackableRawData.transform.localRotation;

            while(true)
            {
                //calculate SMA(single moving average) for local 
                var rawPos = trackableRawData.transform.localPosition;
                positions.Enqueue(rawPos);
                posSum += rawPos;
                if (positions.Count >= queueMaxDim)
                {
                    posSum-=positions.Dequeue();
                }


                smoothedPosition = posSum / positions.Count;
                smoothedRotation = Quaternion.Slerp(smoothedRotation, trackableRawData.transform.localRotation, RotationEMAParam);



                for (var waitFrame = 0; waitFrame < measGapInFrames; waitFrame++)
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }
}