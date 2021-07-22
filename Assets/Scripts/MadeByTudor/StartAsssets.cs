using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class StartAsssets : MonoBehaviour
{
    [SerializeField]
    GameObject initialPrefab;
    [SerializeField]
    GameObject secondaryPrefab;

    // private GameObject firstObj;
    // private GameObject secondObj;
    private GameObject swapAssetsButton;
    void Start()
    {
        secondaryPrefab.SetActive(false);
        initialPrefab.SetActive(true);
        var selectInteractable = initialPrefab.GetComponent<ARSelectionInteractable>();
        Debug.Log(selectInteractable);
        if (selectInteractable == null)
        {
            Debug.Log("we have a problem");
        }

        selectInteractable.onSelectEntered.AddListener(swapToSecondPrefab);
        Debug.Log("just added swaptosendprefab listener, no problem so far");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void swapToSecondPrefab(XRBaseInteractor i)
    {
        Debug.Log("swap to second prefab called");
        initialPrefab.SetActive(false); //TODO: is do not think we have a memory leak here, but I am not sure..we are destroying the observable, not the listener
        // secondObj = Instantiate(secondaryPrefab, transform);
        secondaryPrefab.SetActive(true);
        //also on selecting the second Obj, create a button which allows swapping back to the first object
        var selectInteractable = secondaryPrefab.GetComponent<ARSelectionInteractable>();
        if (selectInteractable.isActiveAndEnabled)
        {
            Debug.Log("All good, seems like second asset's interactable is enabled");
        }

        Debug.Log("called find component");
        selectInteractable.onSelectEntered.AddListener(createSwapToFirstAssetButton);
        Debug.Log("Debuggy");

        Debug.Log("exited swap to second prefab");
    }

    public void createSwapToFirstAssetButton(XRBaseInteractor i)
    {   
        Debug.Log("trying to load resource");
        swapAssetsButton = Resources.Load<GameObject>("CollapseAssetButton");
        Debug.Log("managed to load the collapseassetbutton");

        swapAssetsButton = Instantiate(swapAssetsButton, GameObject.FindGameObjectWithTag("MainCanvas").transform);
        swapAssetsButton.GetComponent<Button>().onClick.AddListener(swapToFirstPrefab);
        var selectInteractable = secondaryPrefab.GetComponent<ARSelectionInteractable>();
        if (!selectInteractable.isActiveAndEnabled)
        {
            Debug.Log("something is not right xaxa");
        }
        selectInteractable.onSelectExited.AddListener(deleteSwapToFirstAssetButton);
    }

    public void deleteSwapToFirstAssetButton(XRBaseInteractor i)
    {
        Debug.Log("Deleting swap assets button");
        if (swapAssetsButton != null)
        {
            
            Destroy(swapAssetsButton);
            swapAssetsButton = null;
        }

    }

    public void swapToFirstPrefab()
    {
        // Destroy(secondObj);
        secondaryPrefab.SetActive(false);
        initialPrefab.SetActive(true);
        // firstObj = Instantiate(initialPrefab, transform);
        var selectInteractabe = initialPrefab.GetComponent<ARSelectionInteractable>();
        selectInteractabe.onSelectEntered.AddListener(swapToSecondPrefab);
        deleteSwapToFirstAssetButton(null);
        
    }

    public void simpleMessage(string message)
    {
        Debug.Log(message);
    }
}
