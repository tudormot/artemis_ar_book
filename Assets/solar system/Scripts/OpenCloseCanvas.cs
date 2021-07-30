using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseCanvas : MonoBehaviour {

    public GameObject canvas;

	public void close () {
        canvas.SetActive(false);
	}

    public void open()
    {
        canvas.SetActive(true);
    }
}
