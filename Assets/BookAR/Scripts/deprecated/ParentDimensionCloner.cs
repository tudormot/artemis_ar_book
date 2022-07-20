using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Scenes.BookAR.Scripts
{
    public class ParentDimensionCloner : MonoBehaviour
    {
        private void OnEnable()
        {
            Debug.LogError("This script is currently obsolette. Might be reinstated later");
            var parent = gameObject.GetComponentInParent<ARTrackedImage>();
            if (parent == null)
            {
                Debug.Log("What? Parent of this asset is not an ARTrackedImage?");
            }

            Debug.Log("Size of this asset before cloning ARImage size: " + parent.transform.localScale);
            Debug.Log("Size of the image itself as reported by ARCore: " + parent.size);
            //now modify the local size
            parent.gameObject.transform.localScale *= 1.7f;
            Debug.Log("Size after hacky hacky " + parent.transform.localScale);

        }
    }
}