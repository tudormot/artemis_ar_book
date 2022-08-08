using System;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
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
        [Tooltip("The parent of the annotation.")]
        GameObject m_AnnotationParent;
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
        List<ARAnnotationPair> annotationPairs;

        private LabelControllerState _state = LabelControllerState.LABELS_HIDDEN;
        public LabelControllerState state
        {
            get => _state;
            set
            {
                onStateChanged(value);
                _state = value;
                
            }
        }

        private void OnEnable()
        {
            foreach (var pair in annotationPairs)
            {
                var interactable = pair.AnnotationParent.GetComponent<ARAnnotationInteractableDEBUG>();
                pair.annotationVisualization.SetActive(false);
                if (interactable == null)
                {
                    interactable = pair.AnnotationParent.AddComponent<ARAnnotationInteractableDEBUG>();
                    interactable.annotations.Add(
                        new ARAnnotationDEBUG()
                        {
                            maxAnnotationRange = this.maxAnnotationRange,
                            maxFOVCenterOffsetAngle = this.maxFOVCenterOffsetAngle,
                            minAnnotationRange = this.minAnnotationRange,
                            annotationVisualization = pair.annotationVisualization
                        }
                    );
                    interactable.enabled = state == LabelControllerState.LABELS_SHOWN;
                }
            }
        }

        private void OnDisable()
        {
            state = LabelControllerState.LABELS_HIDDEN;
        }

        private void onStateChanged(LabelControllerState newState)
        {
            foreach (var pair in annotationPairs)
            {
                Debug.Log($"DEBUG, WE are here, newState = {newState}");
                pair.AnnotationParent.GetComponent<ARAnnotationInteractableDEBUG>().enabled =
                    newState == LabelControllerState.LABELS_SHOWN;
                pair.annotationVisualization.SetActive(
                    newState == LabelControllerState.LABELS_SHOWN
                );
            }

        }

    }

    public enum LabelControllerState
    {
        LABELS_SHOWN, LABELS_HIDDEN
    }
}