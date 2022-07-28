using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using UnityEngine;

namespace BookAR.Scripts.AR.PlacementMode
{
    public interface IPlacementController
    {
        void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready);
        GameObject giveUpPrefabPlacementControl();

        void changePositionReporter(IPositionReporter newReporter);

        
        public static Vector3 calculateScaleFromImSize(Vector2 imageSize)
        {
            var minLocalScalar = Mathf.Min(imageSize.x, imageSize.y);
            return new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
        }
    }
}