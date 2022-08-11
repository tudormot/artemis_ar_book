using System;
using System.Collections.Generic;
using System.Linq;
using BookAR.Scripts.AssetControl.Common.ARLabels;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace BookAR.Scripts.AssetControl._3D.SkullAndBrain
{
    [Serializable]
    public class ARAnnotationPair
    {
        [SerializeField]
        [Tooltip("The visualization GameObject that will become active when the object is hovered over.")]
        GameObject m_AnnotationVisualization;
        
        /// <summary>
        /// The visualization <see cref="GameObject"/> that will become active when the user hovers over this object.
        /// </summary>
        public GameObject annotationVisualization
        {
            get => m_AnnotationVisualization;
            set => m_AnnotationVisualization = value;
        }
        
        
        [SerializeField]
        public Transform labelLineStartPoint;
        [SerializeField]
        public Transform labelLineEndPoint;

                
        [SerializeField]
        [Tooltip("The parent of the annotation.")]
        private GameObject m_AnnotationParent;
        public GameObject AnnotationParent
        {
            get => m_AnnotationParent;
            set => m_AnnotationParent = value;
        }
    }
    public class LabelController : MonoBehaviour
    {
            
            [SerializeField]
            [Tooltip("Maximum angle (in radians) off of FOV horizontal center to show annotation.")]
            float m_MaxFOVCenterOffsetAngle = 0.25f;
        
            /// <summary>
            /// Maximum angle (in radians) off of FOV horizontal center to show annotation.
            /// </summary>
            public float maxFOVCenterOffsetAngle
            {
                get => m_MaxFOVCenterOffsetAngle;
                set => m_MaxFOVCenterOffsetAngle = value;
            }
        
            [SerializeField]
            [Tooltip("Minimum range to show annotation at.")]
            float m_MinAnnotationRange;
        
            /// <summary>
            /// Minimum range to show annotation at.
            /// </summary>
            public float minAnnotationRange
            {
                get => m_MinAnnotationRange;
                set => m_MinAnnotationRange = value;
            }
        
            [SerializeField]
            [Tooltip("Maximum range to show annotation at.")]
            float m_MaxAnnotationRange = 10f;
            /// <summary>
            /// Maximum range to show annotation at.
            /// </summary>
            public float maxAnnotationRange
            {
                get => m_MaxAnnotationRange;
                set => m_MaxAnnotationRange = value;
            }

        [SerializeField]
        Material annotationLineMaterial;

        [SerializeField]
        List<ARAnnotationPair> annotationPairs;


        

        private LabelControllerState _state = LabelControllerState.CONTROLLER_UNINITIALIZED;
        public LabelControllerState state
        {
            get => _state;
            set
            {
                onStateChanged(_state, value);
                _state = value;
                
            }
        }

        private void OnEnable()
        {
            if (state == LabelControllerState.CONTROLLER_UNINITIALIZED)
            {
                state = LabelControllerState.LABELS_HIDDEN;
            }

        }

        private void OnDisable()
        {
            state = LabelControllerState.LABELS_HIDDEN;
        }

        private void onStateChanged(LabelControllerState oldState, LabelControllerState newState)
        {
            if (oldState == LabelControllerState.CONTROLLER_UNINITIALIZED)
            {
                Debug.Log("LabelController OnEnable called");
                foreach (var pair in annotationPairs)
                {
                    if (pair.annotationVisualization == null || pair.AnnotationParent == null)
                    {
                        Debug.LogError("Invalid label pair!");
                    }

                    var interactable = pair.AnnotationParent.GetComponent<CustomARAnnotationInteractable>();
                    pair.annotationVisualization.SetActive(false);
                    if (interactable == null)
                    {
                        interactable = pair.AnnotationParent.AddComponent<CustomARAnnotationInteractable>();
                        interactable.annotations.Add(
                            new CustomARAnnotation()
                            {
                                maxAnnotationRange = this.maxAnnotationRange,
                                maxFOVCenterOffsetAngle = this.maxFOVCenterOffsetAngle,
                                minAnnotationRange = this.minAnnotationRange,
                                annotationVisualization = pair.annotationVisualization,
                                labelLineStartPoint = pair.labelLineStartPoint,
                                labelLineEndPoint = pair.labelLineEndPoint
                            }
                        );
                        interactable.lineDrawer = new LineDrawer(pair.annotationVisualization.name, annotationLineMaterial);
                        Debug.Log($"added a line drawer for {pair.annotationVisualization.name}");
                        interactable.enabled = state == LabelControllerState.LABELS_SHOWN;
                    }
                }


                GetComponentsInChildren<Canvas>().Select(
                    (r) => {
                        if (r.renderMode == RenderMode.WorldSpace && r.worldCamera == null) {
                            r.worldCamera = Camera.main;
                        }
                        return r;
                    } 
                );
            }
            foreach (var pair in annotationPairs)
            {
                pair.AnnotationParent.GetComponent<CustomARAnnotationInteractable>().enabled =
                    newState == LabelControllerState.LABELS_SHOWN;
            
                pair.annotationVisualization.SetActive(
                    newState == LabelControllerState.LABELS_SHOWN
                );
            
            }
        }

    }

    public enum LabelControllerState
    {
        CONTROLLER_UNINITIALIZED, LABELS_SHOWN, LABELS_HIDDEN
    }
}