using System;
using BookAR.Scripts.AssetControl;

namespace BookAR.Scripts.Global
{
    public sealed class GlobalSettingsSingleton
    {
        private static readonly Lazy<GlobalSettingsSingleton> lazy =
            new Lazy<GlobalSettingsSingleton>(() => new GlobalSettingsSingleton());

        public static GlobalSettingsSingleton instance { get { return lazy.Value; } }
        public const bool DEMO_MODE = true; //  a bool which controls whether some functionality should be hidden or not 
        
        public event EventHandler<GlobalSettingsEventData> GlobalSettingsChanged; 
        private State _state = new(){
            placementUpdateMode = AssetPlacementUpdateMode.CONTINUOUS_UPDATE,
            smoothPositionReporting = true,
            smoothTrackingStateReporting = true,
            enableOnScreenDebugMessages = false,
            depthFog = false,
            cameraGrain = false,
            assetCurrentlyDisplayed = null
        };
        public State state
        {
            get => _state;
            set
            {
                GlobalSettingsChanged?.Invoke(
                    this,
                    new GlobalSettingsEventData{
                        newState = value,
                        oldState = _state
                    }
                );
                _state = value;
                
            }
        }
        
        public record State
        {
            public AssetPlacementUpdateMode placementUpdateMode { get; set; }
            public bool enableOnScreenDebugMessages { get; set; }

            public bool smoothTrackingStateReporting { get; set; }
            public bool depthFog { get; set; }
            public bool smoothPositionReporting { get; set; }
            public bool cameraGrain { get; set; }
            
            public IAssetController assetCurrentlyDisplayed { get; set; }

           
        }  
        private GlobalSettingsSingleton()
        { }
    }

    public enum AssetPlacementUpdateMode
    {
        UPDATE_ON_BUTTON_CLICK,
        CONTINUOUS_UPDATE,
        SANDBOX_MODE
    }

    public class GlobalSettingsEventData : EventArgs
    {
        public GlobalSettingsSingleton.State oldState;
        public GlobalSettingsSingleton.State newState;

    }

}