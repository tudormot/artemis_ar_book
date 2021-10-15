using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine
{
    public class ARTrackedImageExtended : UnityEngine.MonoBehaviour
    {
        public bool classicBehaviour = false ;
        public bool staticBehaviour = true ;
        public bool automaticUpdate = true;
        public bool patentBehaviour; //TODO ;)
        
        public GameObject Asset = null;
        public GameObject PlacedAsset = null;
        private ARTrackedImage BaseTrackable;

        private Vector3 RunningMeanPosition;
        private Quaternion RunningMeanRotation;

        private void OnEnable()
        {
            SafetyCheck();
            if (classicBehaviour)
            {
                Debug.Log("ClassicBehaviourNotImplemented!");
            }
            else
            {
                // StartCoroutine(executeFunPeriodically(() => printObjectPositionInfo(transform.gameObject),0.5f));
                StartCoroutine(PlaceAssetInitially());
            }
        }

        private void Update()
        {
            // throw new NotImplementedException();
        }

        void OnGUI()
        {

            if (GUI.Button(new Rect(20, 500, 200, 200), "Update"))
            {
                PlacedAsset.transform.localPosition = RunningMeanPosition;
                PlacedAsset.transform.localRotation = RunningMeanRotation;
            }
        }

        private IEnumerator PlaceAssetInitially()
        {
            int ContinuousPlacementNrFrames = 30;
            float ARCorePlacementDelay = 1f; // On ArCore, the trackable is first spawned at 0,0,0, and only after some delay is reported properly
            
            yield return new WaitForSeconds(ARCorePlacementDelay);
            StartCoroutine(ComputeRunningMeanTransform());
            PlacedAsset = Instantiate(Asset, GameObject.Find("/_Dynamic").transform);
            Debug.Log("At This point the AR Asset should be placed");
            PlacedAsset.transform.localPosition = transform.localPosition;
            PlacedAsset.transform.localRotation = transform.localRotation;
            PlacedAsset.transform.localScale = transform.localScale;
            //initially, update position of Asset every frame, as normal placement would do
            for (var i = 0; i < ContinuousPlacementNrFrames; i++)
            {
                yield return null;
                PlacedAsset.transform.localPosition = transform.localPosition;
                PlacedAsset.transform.localRotation = transform.localRotation;
            }
        }

        private IEnumerator ComputeRunningMeanTransform()
        /*see wikipedia article on moving averrages for some background.
         Chose something which I think is pretty much equivalent to EMA for quaternion 
         average, as I don't want to get involved with quaternion arithmetics*/
        {
            float MeasurementInterval = 0.02f;
            int NrTotalMembers = 50;
            float RotationEMAParam = 0.2f;

            int QueueDim = 1;
            Queue<Vector3> Positions = new Queue<Vector3>();
            Positions.Enqueue(transform.localPosition);
            Vector3 Sum = transform.localPosition;

            RunningMeanPosition = transform.localPosition;
            RunningMeanRotation = transform.localRotation;

            for (;;)
            {
                yield return new WaitForSeconds(MeasurementInterval);
                //calculate SMA(single moving average) for local position
                Positions.Enqueue(transform.localPosition);
                Sum += transform.localPosition;

                if (QueueDim < NrTotalMembers)
                {
                    QueueDim++;
                    RunningMeanPosition = Sum / QueueDim;

                }
                else
                {
                    Debug.Assert((QueueDim == NrTotalMembers),"errorerror!");
                    Sum -= Positions.Dequeue();
                    RunningMeanPosition = Sum / NrTotalMembers;
                }
                //now calculate what I thnk translates to an EMA using quaternion slerp
                RunningMeanRotation = Quaternion.Slerp(RunningMeanRotation, transform.localRotation, RotationEMAParam);
                
                
                if (automaticUpdate)
                {
                    PlacedAsset.transform.localPosition = RunningMeanPosition;
                    PlacedAsset.transform.localRotation = RunningMeanRotation;
                }
            }

            



        }

        private void printObjectPositionInfo(GameObject obj)
        {
            Debug.Log(/*obj.name + ". */"Position: " + obj.transform.position.ToString("F8"));
            Debug.Log(/*obj.name + */". Local position: " + obj.transform.localPosition.ToString("F8"));
        }

        private IEnumerator executeFunPeriodically(Action fun, float waitTime = 1f) 
        {
            for(;;) 
            {
                fun();
                yield return new WaitForSeconds(waitTime);
            }
        }

        private void SafetyCheck()
        {
            BaseTrackable = transform.GetComponent<ARTrackedImage>();
            if (BaseTrackable == null)
            {
                throw new Exception(
                    "This script should only be instantiated by the AR Image library. This means that the object containing this script should also contain an ARTrackableImage");
            }
            if (Asset == null)
            {
                throw new Exception(
                    "Asset to spawn is null. Make sure the calling script sets the script, and then enables this object.");
            }


        }
    }
}