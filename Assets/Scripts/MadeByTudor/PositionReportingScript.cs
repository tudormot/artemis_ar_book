using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionReportingScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject ARcamera;
    void Start()
    {
        ARcamera = GameObject.Find("AR Camera");
        if (ARcamera != null)
        {
            StartCoroutine(executeFunPeriodically(() => printObjectPositionInfo(ARcamera)));
        }
        else
        {
            Debug.Log("AR Camera object is null. perhaps it was not spawned yet? order of script execution?");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void printObjectPositionInfo(GameObject obj)
    {
        Debug.Log(obj.name + ". Position: " + obj.transform.position.ToString());
        Debug.Log(obj.name + ". Local position: " + obj.transform.localPosition.ToString());
    }

    IEnumerator executeFunPeriodically(Action fun, float waitTime = 1f) 
    {
        for(;;) 
        {
            fun();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
