using UnityEngine;
using UnityEngine.UI;

namespace ThirdPartyAssets.solar_system.Scripts
{
    public class ToggleChange : MonoBehaviour {
        [SerializeField]public Toggle pathsToggle;
        [SerializeField]public Toggle labelsToggle;
        [SerializeField]public GameObject SolarSystem;

        public void TogglePaths () {
            Debug.Log("TogglePaths was called!");
            SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = pathsToggle.isOn;
        }

        public void ToggleLabels()
        {
            SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = labelsToggle.isOn;
        }


    }
}
