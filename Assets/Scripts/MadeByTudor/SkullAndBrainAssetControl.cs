using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class SkullAndBrainAssetControl : MonoBehaviour
{

    private GameObject touchToInteractObj;
    private GameObject mainObj;
    private GameObject rootUIObj;
    
    private Button swapAssetsButton;
    private Button expandAssetButton;
    private Button collapseAssetButton;
    
    private Animation mainObjAnimation;
    void Start()
    {
        rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SkullAndBrainUI").gameObject;
        touchToInteractObj = transform.GetChild(0).gameObject;
        mainObj = transform.GetChild(1).gameObject;
        mainObjAnimation = mainObj.transform.GetChild(0).GetComponent<Animation>();
        if (touchToInteractObj == null || mainObj == null || rootUIObj == null || mainObjAnimation == null)
        {
            Debug.Log("Having trouble initialising Skull and Brain asset");
        }
        //touchToInteractObj is a world canvas, that apparently needs to have its camera set. Do that here:
        touchToInteractObj.transform.GetChild(1).GetComponent<Canvas>().worldCamera = Camera.main;

        mainObj.SetActive(false);
        touchToInteractObj.SetActive(true);
        
        //Add listeners internal to this asset.. I don't think that they need to be removed OnDestroy, as the observable get destroyed at the same time 
        touchToInteractObj.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(_swapToMainObj);
        var selectInteractable = mainObj.GetComponent<ARSelectionInteractable>();
        selectInteractable.onSelectEntered.AddListener(showAssetUI);
        selectInteractable.onSelectExited.AddListener(hideAssetUI);
        
        //Add listeners to UI Buttons. These listeners need to be removed when this object is destroyed:
        swapAssetsButton = rootUIObj.transform.Find("CollapseAssetButton").gameObject.GetComponent<Button>();
        expandAssetButton = rootUIObj.transform.Find("ExpandButton").gameObject.GetComponent<Button>();
        collapseAssetButton = rootUIObj.transform.Find("JoinButton").gameObject.GetComponent<Button>();
        if (swapAssetsButton == null || expandAssetButton == null || collapseAssetButton == null)
        {
            Debug.Log("Problem with finding required UI components for Skull and Brain Asset");
        }

        swapAssetsButton.onClick.AddListener(swapToInteractObj);
        swapAssetsButton.onClick.AddListener(_hideAssetUI);
        expandAssetButton.onClick.AddListener(() => mainObjAnimation.Play("expand"));
        collapseAssetButton.onClick.AddListener(() => mainObjAnimation.Play("join"));

    }

    void swapToMainObj(XRBaseInteractor i)
    {
        Debug.Log("swap to second prefab called");
        _swapToMainObj();
    }

    public void _swapToMainObj()
    {
        touchToInteractObj.SetActive(false); 
        mainObj.SetActive(true);
        mainObjAnimation.Play("idle");
    }

    public void showAssetUI(XRBaseInteractor i)
    {   
        rootUIObj.SetActive(true);
    }

    void hideAssetUI(XRBaseInteractor i)
    {
        _hideAssetUI();
    }

    void _hideAssetUI()
    {
        rootUIObj.SetActive(false);
    }

    public void swapToInteractObj()
    {
        mainObj.SetActive(false);
        touchToInteractObj.SetActive(true);
    }
    

    private void OnDestroy()
    {
        swapAssetsButton.onClick.RemoveAllListeners();
        rootUIObj.SetActive(false);
    }
}
