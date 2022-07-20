using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportPositionOnButtonClick : MonoBehaviour
{
    [SerializeField]
    GameObject obj1;
    [SerializeField]
    GameObject obj2;
    [SerializeField]
    GameObject obj3;

    
    // Start is called before the first frame update
    void Start()
    {
        // ARcamera = GameObject.Find("AR Camera");
        // sessionOrigin = GameObject.Find("AR Session Origin");
        // content = GameObject.Find("Content");

    }

    void OnGUI()
    {
        // Make a background box
        // GUI.Box(new Rect(10, 10, 100, 90), "Debug Menu");

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if (GUI.Button(new Rect(20, 500, 200, 200), "PrintPosition"))
        {
            if (obj1 != null)
            {
                printObjectPositionInfo(obj1);
            }
            if (obj2 != null)
            {
                printObjectPositionInfo(obj2);
            }
            if (obj3 != null)
            {
                printObjectPositionInfo(obj3);
            }
            
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

}
