using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SolarSystemManager : MonoBehaviour {
    public float GlobalSpeed=1;

    private bool m_ShowPaths = true;
    public bool ShowPaths
    {
	    get {return m_ShowPaths;}
	    set
	    {
		    m_ShowPaths = value;
		    var planets = transform.Find("paths");
		    planets.gameObject.SetActive(m_ShowPaths);

	    }
    }

    private bool m_ShowLabels = true;
    public bool ShowLabels
    {
	    get {return m_ShowLabels;}
	    set
	    {
		    m_ShowLabels = value;
		    var planets = transform.Find("Planets");
		    ApplyFunToObjRecursive(planets.gameObject, (obj) =>
		    {

			    if (obj.CompareTag("label"))
			    {
				    obj.SetActive(m_ShowLabels);
			    }
		    });
	    }
    }

    public static void ApplyFunToObjRecursive(GameObject Obj, Action<GameObject> fun)
    {
	    fun(Obj);
	    if (Obj.transform.childCount != 0)
	    {
		    foreach (Transform child in Obj.transform)
		    {
			    ApplyFunToObjRecursive(child.gameObject, fun);
		    }
	    }
    }

    private void OnEnable()
    {
	    //set labels to face camera
	    var planets = transform.Find("Planets");
	    ApplyFunToObjRecursive(planets.gameObject, (obj) =>
	    {
		    if (obj.CompareTag("label"))
		    {
			    obj.GetComponent<LookAtCamera>().LookAtTheCamera = Camera.main.transform;
		    }
	    });
    }

    // Update is called once per frame
    // void Update () {
    //        Paths.SetActive(ShowPaths);
    //        Labels.SetActive(ShowLabels);
    // }
}
