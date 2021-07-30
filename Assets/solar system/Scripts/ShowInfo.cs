using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ShowInfo : MonoBehaviour {
	public GameObject DescriptionCanvas;
    public Text description;
     
		
	void  Update()
   	 {
        if (Input.GetMouseButtonDown(0))
	  {
        RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject()) //If the mouse is not over a UI element
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "planet")
                    {
                        var PrnFile = Resources.Load("PlanetsDescriptions/" + hit.collider.name) as TextAsset;
                        description.GetComponent<Text>().text = PrnFile.text;
                        StartCoroutine("showDescCanvas");
                    }
                }
            }
	  }
   	 }



    IEnumerator showDescCanvas()
    {
        yield return new WaitForSeconds(0.2f);

        DescriptionCanvas.SetActive(true);
    }
        
}
