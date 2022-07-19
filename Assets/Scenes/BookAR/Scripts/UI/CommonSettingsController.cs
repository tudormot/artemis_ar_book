using System;
using Scenes.BookAR.Scripts.Global;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace Scenes.BookAR.Scripts.UI
{
    public class CommonSettingsController : MonoBehaviour
    {
        [SerializeField] private GameObject expandedUI;
        [SerializeField] private GameObject contractedUI;
        
        [SerializeField] private EventTrigger expandCanvasTrigger;
        [SerializeField] private EventTrigger contractCanvasTrigger;

        [SerializeField] private Animation expandAnimation;
        [SerializeField] private Animation contractAnimation;
        
        [SerializeField] private Toggle manualAssetPositionUpdateToggle;
        [SerializeField] public Button manualAssetPositionUpdateButton;
        
        

        private SettingsUIState _state = SettingsUIState.SETTINGS_PANEL_CONTRACTED;
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
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener( ( data ) => state = SettingsUIState.SETTINGS_PANEL_EXPANDED);
            expandCanvasTrigger.triggers.Add( entry );
            entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entry.callback.AddListener( ( data ) => state = SettingsUIState.SETTINGS_PANEL_CONTRACTED);
            contractCanvasTrigger.triggers.Add( entry );
            manualAssetPositionUpdateToggle.onValueChanged.AddListener(
                (bool isToggleChecked) =>
                {
                    GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
                    {
                        automatedAssetPlacementUpdating = isToggleChecked
                    };
                }
            );
        }

        private void OnDisable()
        {
            foreach (var trigger in expandCanvasTrigger.triggers)
            {
                trigger.callback.RemoveAllListeners();
            }
            foreach (var trigger in contractCanvasTrigger.triggers)
            {
                trigger.callback.RemoveAllListeners();
            }
            manualAssetPositionUpdateToggle.onValueChanged.RemoveAllListeners();
        }

        private void OnStateChanged(SettingsUIState oldState, SettingsUIState newState)
        {
            switch (newState)
            {
                case SettingsUIState.SETTINGS_PANEL_EXPANDED:
                    
                    break;
                case SettingsUIState.SETTINGS_PANEL_CONTRACTED:
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