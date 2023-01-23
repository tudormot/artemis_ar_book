using System;
using System.Collections;
using UnityEngine;
using BookAR.Scripts.AssetControl;
using BookAR.Scripts.Global;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace BookAR.Scripts.AssetControl._3D
{
    class DinosaurControl : IAssetController, IStatefulController<DinosaurState> 
    {
        public override AssetControllerType type { get; protected set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        public override void reactToCollapseRequest()
        {
            if ((this as IStatefulController<DinosaurState>).state == DinosaurState.WALKING_STATE)
            {
                (this as IStatefulController<DinosaurState>).state = DinosaurState.TOUCH_TO_INTERACT_PHASE;

            }

        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            if (e == OcclusionEvent.IMAGE_OCCLUDED)
            {
                (this as IStatefulController<DinosaurState>).state = DinosaurState.OCCLUDED_STATE;

            }
            else
            {
                (this as IStatefulController<DinosaurState>).state = DinosaurState.TOUCH_TO_INTERACT_PHASE;

            }

        }


        [SerializeField] private Button touchToInteractButton;
        [SerializeField] private Canvas touchToInteractCanvas;
        [SerializeField] private Animator mainAsset;


        private void OnEnable()
        {
            touchToInteractCanvas.worldCamera = Camera.main;
            (this as IStatefulController<DinosaurState>).state = DinosaurState.TOUCH_TO_INTERACT_PHASE;
            touchToInteractButton.onClick.AddListener(
                () =>
                {
                    base.onTouchToInteractButtonPressed();
                    (this as IStatefulController<DinosaurState>).state = DinosaurState.WALKING_STATE;
                }
            );
        }

        private void OnDisable()
        {
            touchToInteractButton.onClick.RemoveAllListeners();
        }


        public DinosaurState _state { get; set; }

        void IStatefulController<DinosaurState>.OnStateChanged(DinosaurState oldState, DinosaurState newState)
        {
            switch (newState)
            {
                case DinosaurState.TOUCH_TO_INTERACT_PHASE:
                    touchToInteractCanvas.gameObject.SetActive(true);
                    mainAsset.gameObject.SetActive(false);
                    break;
                case DinosaurState.WALKING_STATE:
                    touchToInteractCanvas.gameObject.SetActive(false);
                    mainAsset.gameObject.SetActive(true);

                    break;
                case DinosaurState.OCCLUDED_STATE:
                    touchToInteractCanvas.gameObject.SetActive(false);
                    mainAsset.gameObject.SetActive(false);
                    break;

            }
        }
    }
    enum DinosaurState
    {
        OCCLUDED_STATE,
        TOUCH_TO_INTERACT_PHASE,
        WALKING_STATE,
    }


}