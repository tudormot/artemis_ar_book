using UnityEngine;
using System.Collections;

public class RotatorMultiAxe : MonoBehaviour {
    public bool X, Y, Z;
    public float RotationSpeed=5;

    private int Xfactor, Yfactor, Zfactor;
    private float GlobalSpeed;

    void Start()
    {
        // (X)?X
        Xfactor = X ? 1 : 0;
        Yfactor = Y ? 1 : 0;
        Zfactor = Z ? 1 : 0;
        
    }
    // Update is called once per frame
    void Update () {
        GlobalSpeed = find.FindParentWithTag(transform.gameObject, "SolarSystem").GetComponent<SolarSystemManager>().GlobalSpeed;
        transform.Rotate (new Vector3 (Xfactor * 10, Yfactor*10, Zfactor*10) * GlobalSpeed * RotationSpeed * Time.deltaTime);
	}


    
}
