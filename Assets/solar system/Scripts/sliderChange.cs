using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sliderChange : MonoBehaviour {
    public GameObject SolarSystem;
	public void RotationChange()
    {
        SolarSystem.GetComponent<SolarSystemManager>().GlobalSpeed = GetComponent<Slider>().value;
	}
		
}
