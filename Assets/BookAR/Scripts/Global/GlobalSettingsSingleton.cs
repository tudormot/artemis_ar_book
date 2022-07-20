using System;

namespace Scenes.BookAR.Scripts.Global
{
    public sealed class GlobalSettingsSingleton
    {
        private static readonly Lazy<GlobalSettingsSingleton> lazy =
            new Lazy<GlobalSettingsSingleton>(() => new GlobalSettingsSingleton());

        public static GlobalSettingsSingleton instance { get { return lazy.Value; } }
        
        
        public event EventHandler GlobalSettingsChanged; 
        private State _state = new(){
            automatedAssetPlacementUpdating = true,
            freeAssetPlacement = false,
            assetInSandboxMode = false,
            depthFog = false,
            cameraGrain = false,
        };
        public State state
        {
            get => _state;
            set
            {
                _state = value;
                GlobalSettingsChanged?.Invoke(this,EventArgs.Empty);
            }
        }
        
        public record State
        {
            public bool automatedAssetPlacementUpdating { get; set; }
            public bool freeAssetPlacement { get; set; }
            public bool assetInSandboxMode { get; set; }
            public bool depthFog { get; set; }
            public bool cameraGrain { get; set; }
           
        }  
        private GlobalSettingsSingleton()
        { }
    }
    
}