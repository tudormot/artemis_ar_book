using BookAR.Scripts.AssetControl;
using UnityEditor;
using UnityEngine;

namespace BookAR.Scripts.AR.PlacementControllers
{
    public class AssetScaler
    {
        private AssetControllerType? assetType;
        public AssetScaler(GameObject obj)
        {
            assetType = obj.GetComponentInChildren<IAssetController>()?.type;
            if (assetType == null)
            {
                Debug.LogError($"This asset does not contain an IAssetController! asset name: {obj.name}");
            }
        }

        public Vector3 computeScalingForAsset(Vector2 imageSize)
        {
            if (assetType == AssetControllerType.VIDEO_ASSET_TYPE)
            {
                return new Vector3(imageSize.x * 1.05f, 1f, imageSize.y * 1.05f);
            }
            else
            {
                var minLocalScalar = Mathf.Min(imageSize.x, imageSize.y);
                return new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
            }
        }
    }
}