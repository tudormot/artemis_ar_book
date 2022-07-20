using System;
using Scenes.BookAR.Scripts.Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Scenes.BookAR.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class CommonSettingsUIController : MonoBehaviour
    {
        // [SerializeField] private GameObject expandedUI;
        // [SerializeField] private GameObject contractedUI;
        
        [SerializeField] private Button expandCanvasButton;
        [SerializeField] private Button contractCanvasButton;

        // [SerializeField] private Animation expandAnimation;
        // [SerializeField] private Animation contractAnimation;
        private Animator globalSettingsUIAnimator;

        
        [SerializeField] private Toggle manualAssetPositionUpdateToggle;
        [SerializeField] public Button manualAssetPositionUpdateButton;
        
        

        private SettingsUIState _state = SettingsUIState.SETTINGS_PANEL_CONTRACTED;
        private static readonly int isPanelExpandedHash = Animator.StringToHash("isPanelExpanded");

        private SettingsUIState state
        {
            get => _state;
            set
            {
                OnStateChanged(_state,value);
                _state = value;
            }
        }

        private void OnEnable()
        {
            globalSettingsUIAnimator = GetComponent<Animator>();
            expandCanvasButton.onClick.AddListener(
                () =>
                {
                    Debug.Log("We are clicking the settings image!");
                    state = SettingsUIState.SETTINGS_PANEL_EXPANDED;
                }
            );
            contractCanvasButton.onClick.AddListener(
                () =>
                {
                    Debug.Log("We are clicking the transparent canvas!");
                    state = SettingsUIState.SETTINGS_PANEL_CONTRACTED;
                }
            );

            manualAssetPositionUpdateToggle.onValueChanged.AddListener(
                (bool isToggleChecked) =>
                {
                    GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
                    {
                        automatedAssetPlacementUpdating = isToggleChecked
                    };
                }
            );
            manualAssetPositionUpdateButton.onClick.AddListener(
                ()=>Debug.Log("Sanity check. We are clicking this button.")
            );
        }

        private void OnDisable()
        {
            contractCanvasButton.onClick.RemoveAllListeners();
            expandCanvasButton.onClick.RemoveAllListeners();
            manualAssetPositionUpdateToggle.onValueChanged.RemoveAllListeners();
        }

        private void OnStateChanged(SettingsUIState oldState, SettingsUIState newState)
        {
            Debug.Log("OnStateChanged called!");
            switch (newState)
            {
                case SettingsUIState.SETTINGS_PANEL_EXPANDED:
                    globalSettingsUIAnimator.SetBool(isPanelExpandedHash,true);
                    break;
                case SettingsUIState.SETTINGS_PANEL_CONTRACTED:
                    globalSettingsUIAnimator.SetBool(isPanelExpandedHash,false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
        private enum SettingsUIState
        {
            SETTINGS_PANEL_EXPANDED,
            SETTINGS_PANEL_CONTRACTED
        }
    }
}