using Unity.Collections;
using UnityEngine;

namespace BookAR.Scripts.AssetControl
{
    public interface IAssetController
    {
        public AssetControllerType type { get; protected set; }
    }
}

public enum AssetControllerType
{
    DEFAULT_ASSET_TYPE,
    VIDEO_ASSET_TYPE,
}