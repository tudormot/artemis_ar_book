using UnityEngine;

namespace BookAR.Scripts.AR.PlacementMode
{
    public interface IPositionReporter
    {
        Transform getTransform();
    }
}