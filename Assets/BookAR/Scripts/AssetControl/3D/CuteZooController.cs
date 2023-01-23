using System;
using System.Collections.Generic;
using BookAR.Scripts.AssetControl._3D.ExplodingRocket;
using BookAR.Scripts.AssetControl.Common;
using BookAR.Scripts.AssetControl.Common.AssetQuitting;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace BookAR.Scripts.AssetControl._3D
{
    

    public enum CuteZooState
    {
        TOUCH_TO_INTERACT_STATE,READY_TO_INTERACT, READY_TO_INTERACT_OCCLUDED, TOUCH_TO_INTERACT_OCCLUDED
    }


    public class CuteZooController : IAssetController, IStatefulController<CuteZooState>
    {
        public override AssetControllerType type { get; protected set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        

        [SerializeField] private Button touchToInteractButton;
        [SerializeField] private Canvas touchToInteractCanvas;

        [SerializeField] private GameObject fullCuteZoo;
        [SerializeField] private List<Animator> animalAnimators;

        private ARExperienceHider hider = null;
        private String currentPlayingAnimationName = null;

        private String ANIMATION_MAGIC_VARIABLE_NAME = "animation";

        private Dictionary<String, int> buttonNameToAnimValue =
            new Dictionary<String, int>
            {
                {"IdleButton", 0 },
                {"WalkButton",1 },
                {"RunButton",2 },
                {"JumpButton",3 },
                {"EatButton",4 },
                {"RestButton",5 },
                {"AttackButton",6 },
                {"DamageButton",7 },
                {"DieButton",8 }
            };

        private GameObject mainCuteZooUI;
        CuteZooState IStatefulController<CuteZooState>._state { get; set; } = CuteZooState.TOUCH_TO_INTERACT_STATE;
        

        void IStatefulController<CuteZooState>.OnStateChanged(CuteZooState oldState, CuteZooState newState)
        {
            switch (newState)
            {
                case CuteZooState.TOUCH_TO_INTERACT_STATE:
                    hider?.enableARExperience();
                    hider = null;
                    touchToInteractCanvas.gameObject.SetActive(true);
                    fullCuteZoo.SetActive(false);
                    break;
                case CuteZooState.READY_TO_INTERACT:
                    hider?.enableARExperience();
                    hider = null;
                    touchToInteractCanvas.gameObject.SetActive(false);
                    mainCuteZooUI.SetActive(true);
                    fullCuteZoo.SetActive(true);
                    setAllAnimators(buttonNameToAnimValue[currentPlayingAnimationName]);
                    break;
                case CuteZooState.READY_TO_INTERACT_OCCLUDED:
                    Debug.Log("CuteZoo in READY_TO_INTERACT_OCCLUDED");
                    hider = new ARExperienceHider(new List<GameObject>() { fullCuteZoo });
                    hider.disableARExperience();
                    break;
                case CuteZooState.TOUCH_TO_INTERACT_OCCLUDED:
                    Debug.Log("CuteZoo in TOUCH_TO_INTERACT_OCCLUDED");

                    hider = new ARExperienceHider(new List<GameObject>() { touchToInteractCanvas.gameObject });
                    hider.disableARExperience();
                    break;
                default:
                    break;
            }
        }

        void OnEnable()
        {
            //find and connect all buttons
            mainCuteZooUI = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find("CuteZooUI").gameObject;
            if (mainCuteZooUI == null)
            {
                Debug.LogError($"we could not find the cutezoo main UI gameObj. No good.");
            }
            
            //touchToInteractObj is a world canvas, that apparently needs to have its camera set. Do that here:
            touchToInteractCanvas.worldCamera = Camera.main;

            var buttonGroup = mainCuteZooUI.transform.Find("ButtonGroup");
            currentPlayingAnimationName = "IdleButton";
            foreach (var entry in buttonNameToAnimValue)
            {
                Button button = buttonGroup.Find(entry.Key).GetComponent<Button>();
                if (button == null)
                {
                    Debug.LogError($"we could not find the cutezoo button named: {entry.Key}");
                }

                button.onClick.AddListener(
                    () =>
                    {
                        setAllAnimators(entry.Value);
                        currentPlayingAnimationName = entry.Key;
                    }
                );
            }

            touchToInteractButton.onClick.AddListener(onTouchToInteractButtonPressed);
            (this as IStatefulController<CuteZooState>).state = CuteZooState.TOUCH_TO_INTERACT_STATE;
        }

        protected override void onTouchToInteractButtonPressed()
        {
            base.onTouchToInteractButtonPressed();
            (this as IStatefulController<CuteZooState>).state = CuteZooState.READY_TO_INTERACT;
        }

        private void setAllAnimators(int animVariableValue)
        {
            foreach (var animator in animalAnimators)
            {
                animator.SetInteger(ANIMATION_MAGIC_VARIABLE_NAME, animVariableValue);
            }
        }
        
        public override void reactToCollapseRequest()
        {
            (this as IStatefulController<CuteZooState>).state = CuteZooState.TOUCH_TO_INTERACT_STATE;
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            if (e == OcclusionEvent.IMAGE_OCCLUDED)
            {
                switch ((this as IStatefulController<CuteZooState>).state)
                {
                    case CuteZooState.TOUCH_TO_INTERACT_STATE:
                        (this as IStatefulController<CuteZooState>).state = CuteZooState.TOUCH_TO_INTERACT_OCCLUDED;
                        break;
                    case CuteZooState.READY_TO_INTERACT:
                        (this as IStatefulController<CuteZooState>).state = CuteZooState.READY_TO_INTERACT_OCCLUDED;
                        break;
                    default:
                        Debug.LogError("In IMAGE_OCCLUDED event, case impossible!");
                        break;
                }
            }
            else
            {
                if ((this as IStatefulController<CuteZooState>).state == CuteZooState.READY_TO_INTERACT_OCCLUDED)
                {
                    (this as IStatefulController<CuteZooState>).state = CuteZooState.READY_TO_INTERACT;
                }
                else
                {
                    (this as IStatefulController<CuteZooState>).state = CuteZooState.TOUCH_TO_INTERACT_STATE;

                }
            }

        }

    }
}

