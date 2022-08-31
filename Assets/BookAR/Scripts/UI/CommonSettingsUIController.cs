using System;
using BookAR.Scripts.Global;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace BookAR.Scripts.UI
{
    [RequireComponent(typeof(Animator))]
    public class CommonSettingsUIController : MonoBehaviour
    {

        [SerializeField] private Button expandCanvasButton;
        [SerializeField] private Button contractCanvasButton;
        
        private Animator globalSettingsUIAnimator;
        
        [SerializeField] private Toggle screenDebuggingToggle;
        [SerializeField] private Toggle smoothPositionToggle;
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

        private void contractCanvas()
        {
            Debug.Log("We are clicking the transparent canvas!");
            state = SettingsUIState.SETTINGS_PANEL_CONTRACTED;
            expandCanvasButton.onClick.RemoveListener(contractCanvas);
            expandCanvasButton.onClick.AddListener(expandCanvas);
        }
        private void expandCanvas()
        {
            Debug.Log("We are clicking the settings image!");
            state = SettingsUIState.SETTINGS_PANEL_EXPANDED;
            expandCanvasButton.onClick.RemoveListener(expandCanvas);
            expandCanvasButton.onClick.AddListener(contractCanvas);
        }

        private void OnEnable()
        {
            screenDebuggingToggle.isOn = GlobalSettingsSingleton.instance.state.enableOnScreenDebugMessages;
            smoothPositionToggle.isOn = GlobalSettingsSingleton.instance.state.smoothPositionReporting;

            manualAssetPositionUpdateToggle.isOn = GlobalSettingsSingleton.instance.state.placementUpdateMode ==
                                                   AssetPlacementUpdateMode.UPDATE_ON_BUTTON_CLICK;
            manualAssetPositionUpdateButton.interactable = GlobalSettingsSingleton.instance.state.placementUpdateMode ==
                                                           AssetPlacementUpdateMode.UPDATE_ON_BUTTON_CLICK;
            
            globalSettingsUIAnimator = GetComponent<Animator>();
            expandCanvasButton.onClick.AddListener(
                expandCanvas
            );
            contractCanvasButton.onClick.AddListener(
                () =>
                {
                    Debug.Log("Button disabled for the time being");
                }
            );
            screenDebuggingToggle.onValueChanged.AddListener(
                (bool isToggleChecked) =>
                {
                    GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
                    {
                        enableOnScreenDebugMessages = isToggleChecked
                    };
                }
            );
            smoothPositionToggle.onValueChanged.AddListener(
                (bool isToggleChecked) =>
                {
                    GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
                    {
                        smoothPositionReporting = isToggleChecked
                    };
                }
            );

            manualAssetPositionUpdateToggle.onValueChanged.AddListener(
                (bool isToggleChecked) =>
                {
                    GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
                    {
                        placementUpdateMode = isToggleChecked ? AssetPlacementUpdateMode.UPDATE_ON_BUTTON_CLICK
                                                              : AssetPlacementUpdateMode.CONTINUOUS_UPDATE
                    };
                    manualAssetPositionUpdateButton.interactable = isToggleChecked;
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