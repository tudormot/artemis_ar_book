using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace BookAR.Scripts.AR.PlacementMode
{
    public class ContinuousPlacementController:IPlacementController
    {
        private IPositionReporter posReporter;
        private MonoBehaviour context;
        private GameObject controlledAsset;
        private Coroutine controlCoroutine;
        public ContinuousPlacementController(IPositionReporter posReporter, MonoBehaviour context)
        {
            this.posReporter = posReporter;
            this.context = context;
        }

        public void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready )
        {
            controlledAsset = prefabInstantiatedAlready ? prefab : Object.Instantiate(prefab, GameObject.Find("/_Dynamic").transform);
            controlCoroutine = context.StartCoroutine(updatePositionContinuously());
        }
        
        public GameObject giveUpPrefabPlacementControl()
        {
            context.StopCoroutine(controlCoroutine);
            var assetOnWhichToGiveUp = controlledAsset;
            controlledAsset = null;
            return assetOnWhichToGiveUp;
        }

        public void changePositionReporter(IPositionReporter newReporter)
        {
            posReporter = newReporter;
        }

        private IEnumerator updatePositionContinuously()
        {
            while (true)
            {
                var transform = posReporter.getTransform();
                controlledAsset.transform.localPosition = transform.pos;
                controlledAsset.transform.localRotation = transform.rot;
                controlledAsset.transform.localScale = transform.scale;
                yield return new WaitForEndOfFrame();
            }
        }

    }
}