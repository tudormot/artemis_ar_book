﻿using UnityEngine;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public enum MenuType
    {
        ARtemisMenu,
        SamplesMenu,
        ImageTracking,
        FaceTracking,
        PlaneDetection,
        BodyTracking,
        Meshing,
        Depth,
        LightEstimation
    }

    public static class ActiveMenu
    {
        public static MenuType currentMenu { get; set; } = MenuType.ARtemisMenu;
    }
}
