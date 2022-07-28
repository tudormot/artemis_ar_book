using BookAR.Scripts.AR.PlacementMode.PositionReporters;
using UnityEngine;

namespace BookAR.Scripts.AR.PlacementMode
{
    public interface IPlacementController
    {
        void startPrefabPlacementControl(GameObject prefab, bool prefabInstantiatedAlready);
        GameObject giveUpPrefabPlacementControl();

        void changePositionReporter(IPositionReporter newReporter);

        
    }
}