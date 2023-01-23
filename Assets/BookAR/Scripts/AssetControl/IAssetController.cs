using BookAR.Scripts.Global;
using Unity.Collections;
using UnityEngine;

namespace BookAR.Scripts.AssetControl
{
    public abstract class IAssetController:MonoBehaviour
    {
        public abstract AssetControllerType type { get; protected set; }
        public abstract void reactToCollapseRequest();
        public abstract void reactToOcclusionEvent(OcclusionEvent e);

        protected virtual void onTouchToInteractButtonPressed()
        {
            if (GlobalSettingsSingleton.instance.state.assetCurrentlyDisplayed != null)
            {
                GlobalSettingsSingleton.instance.state.assetCurrentlyDisplayed.reactToCollapseRequest();
            }
            GlobalSettingsSingleton.instance.state = GlobalSettingsSingleton.instance.state with
            {
                assetCurrentlyDisplayed = this
            };
        }

    }
}
public enum OcclusionEvent
{
    IMAGE_OCCLUDED,
    IMAGE_NOT_OCCLUDED,
}
public enum AssetControllerType
{
    DEFAULT_ASSET_TYPE,
    VIDEO_ASSET_TYPE,
}