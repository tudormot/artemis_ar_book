using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	public Transform LookAtTheCamera;//The camera to look at
    float newScale;

    void Start()
    {
        newScale = transform.localScale.x * -1;
    }
	

    void Update()
    {
        if (LookAtTheCamera != null)
        {
            transform.LookAt(LookAtTheCamera);

            //change the X scale of your 3D text to -1, to flip it.        
            transform.localScale = new Vector3(newScale, transform.localScale.y, transform.localScale.z);
        }
    }
}
