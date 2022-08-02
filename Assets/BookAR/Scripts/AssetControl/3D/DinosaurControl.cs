using System;
using System.Collections;
using UnityEngine;
using BookAR.Scripts.AssetControl;
using BookAR.Scripts.Global;
using DG.Tweening;
using UnityEngine.UI;

namespace BookAR.Scripts.AssetControl._3D
{
    public class DinosaurControl : MonoBehaviour, IAssetController, IStatefulController<DinosaurState>
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        DinosaurState IStatefulController<DinosaurState>._state { get; set; }

        [SerializeField] private Button touchToInteractButton;
        [SerializeField] private Canvas touchToInteractCanvas;
        [SerializeField] private Animator mainAsset;


        private void OnEnable()
        {
            touchToInteractCanvas.worldCamera = Camera.main;
            (this as IStatefulController<DinosaurState>).state = DinosaurState.TOUCH_TO_INTERACT_PHASE;
            touchToInteractButton.onClick.AddListener(
                ()=> (this as IStatefulController<DinosaurState>).state = DinosaurState.ASSET_INTRO_STATE
                );
        }

        private void OnDisable()
        {
            touchToInteractButton.onClick.RemoveAllListeners();
        }
        
        void IStatefulController<DinosaurState>.OnStateChanged(DinosaurState oldState, DinosaurState newState)
        {
            switch (newState)
            {
                case DinosaurState.TOUCH_TO_INTERACT_PHASE:
                    touchToInteractCanvas.gameObject.SetActive(true);
                    mainAsset.gameObject.SetActive(false);
                    break;
                case DinosaurState.ASSET_INTRO_STATE:
                    StartCoroutine(AssetIntroManualAnimation());
                    break;
                case DinosaurState.WALKING_STATE:
                    // animator.SetBool("isWalkingState",true);
                    // animator.SetBool("isHeadMovingState", false);
                    Debug.Log("DEBUG, we are in DinosaurState.WALKING_STATE");
                    // mainAsset.
                    mainAsset.SetBool("isWalking",true);
                    
                    break;
                case DinosaurState.MOVING_HEAD_STATE:
                    // animator.SetBool("isHeadMovingState", true);
                    // animator.SetBool("isWalkingState",false);
                    mainAsset.SetBool("isWalking",true);
                    break;

                    
            }
        }

        private IEnumerator AssetIntroManualAnimation()
        {
            float inflationTimeInSeconds = 5f;
            var goalScale = mainAsset.transform.localScale;
            mainAsset.transform.localScale = new Vector3(0, 0, 0);
            mainAsset.gameObject.SetActive(true);
            touchToInteractCanvas.gameObject.SetActive(false);
            mainAsset.transform.DOScale(goalScale, inflationTimeInSeconds);
            yield return new WaitForSeconds(inflationTimeInSeconds);
            (this as IStatefulController<DinosaurState>).state = DinosaurState.WALKING_STATE;


        }

    }
    enum DinosaurState
    {
        TOUCH_TO_INTERACT_PHASE,
        ASSET_INTRO_STATE,
        WALKING_STATE,
        MOVING_HEAD_STATE
    }


}