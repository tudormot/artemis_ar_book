using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class SmoothPositionReporter:IPositionReporter
    {
        /* On ARCore, the trackable is incorrectly reported at the beginning of image detection
         as being placed at (0,0,0). This tweak ensures that those bad positions are not taken into
         account in the moving average*/
        private bool badPositionsMode_ARCore = false;

        private readonly ARTrackedImage trackableRawData;
        private readonly MonoBehaviour context;
        private readonly Coroutine posCalculatingCoroutine;
        private readonly Queue<Vector3> positions = new Queue<Vector3>();

        public Vector3 smoothedPosition;
        public Quaternion smoothedRotation;

        public SmoothPositionReporter(ARTrackedImage trackable, MonoBehaviour context)
        {
            this.context = context;
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

        public TransformData getTransform()
        {
            return new TransformData()
            {
                pos = smoothedPosition,
                rot = smoothedRotation,
                scale = trackableRawData.transform.localScale
            };
        }

        public Vector2 getImageSize()
        {
            return trackableRawData.size;
        }
        
        private IEnumerator calculateSmoothTransform()
            /*see wikipedia article on moving averages.
             Chose something which I think is pretty much equivalent to EMA for quaternion 
             average, as I don't want to get involved with quaternion arithmetics*/
        {
            int measIntervalInFrames = 60; //across how many frames the running mean is computed
            int measGapInFrames = 2;          //gap between recording of raw position data
            float RotationEMAParam = 0.2f;

            var queueMaxDim = measIntervalInFrames / measGapInFrames;
            Vector3 posSum = Vector3.zero;
            smoothedRotation = trackableRawData.transform.localRotation;
            // Positions.Enqueue(transform.localPosition);
            // RunningMeanPosition = transform.localPosition;
            // RunningMeanRotation = transform.localRotation;

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