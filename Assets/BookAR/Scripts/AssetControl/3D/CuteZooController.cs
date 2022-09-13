using System;
using System.Collections.Generic;
using BookAR.Scripts.AssetControl._3D.ExplodingRocket;
using UnityEngine;
using UnityEngine.UI;

namespace BookAR.Scripts.AssetControl._3D
{
    

    public enum CuteZooState
    {
        TOUCH_TO_INTERACT_STATE,READY_TO_INTERACT
    }


    public class CuteZooController : MonoBehaviour, IAssetController
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        [SerializeField] private Button touchToInteractButton;
        [SerializeField] private Canvas touchToInteractCanvas;

        [SerializeField] private GameObject fullCuteZoo;
        [SerializeField] private List<Animator> animalAnimators;

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

        private CuteZooState _state;
        public CuteZooState state
        {
            get => _state;
            set
            {
                onStateChanged(value);
                _state = value;
            }
        }

        private void onStateChanged(CuteZooState newState)
        {
            switch (newState)
            {
                case CuteZooState.TOUCH_TO_INTERACT_STATE:
                    //touchToInteractObj is a world canvas, that apparently needs to have its camera set. Do that here:
                    touchToInteractCanvas.worldCamera = Camera.main;
                    touchToInteractCanvas.gameObject.SetActive(true);
                    fullCuteZoo.SetActive(false);
                    break;
                case CuteZooState.READY_TO_INTERACT:
                    touchToInteractCanvas.gameObject.SetActive(false);
                    mainCuteZooUI.SetActive(true);
                    fullCuteZoo.SetActive(true);
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

            var buttonGroup = mainCuteZooUI.transform.Find("ButtonGroup");
            foreach (var entry in buttonNameToAnimValue)
            {
                Button button = buttonGroup.Find(entry.Key).GetComponent<Button>();
                if (button == null)
                {
                    Debug.LogError($"we could not find the cutezoo button named: {entry.Key}");
                }

                button.onClick.AddListener(
                    ()=>setAllAnimators(entry.Value)
                );
            }
            touchToInteractButton.onClick.AddListener(
                () =>
                {
                    state = CuteZooState.READY_TO_INTERACT;
                });
            state = CuteZooState.TOUCH_TO_INTERACT_STATE;
        }

        private void setAllAnimators(int animVariableValue)
        {
            foreach (var animator in animalAnimators)
            {
                animator.SetInteger(ANIMATION_MAGIC_VARIABLE_NAME, animVariableValue);
            }
        }
    }
}

