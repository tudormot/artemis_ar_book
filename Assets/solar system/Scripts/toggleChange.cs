using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleChange : MonoBehaviour {
    public Toggle pathsToggle;
    public Toggle labelsToggle;
    public GameObject SolarSystem;

	public void TogglePaths () {
        SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = pathsToggle.isOn;
    }

    public void ToggleLabels()
    {
        SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = labelsToggle.isOn;
    }


}
