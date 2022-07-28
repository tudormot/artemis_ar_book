using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace BookAR.Scripts.AssetControl._3D
{
    public class ToothAssetControl : MonoBehaviour, IAssetController
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;

        private GameObject touchToInteractObj;
        private GameObject mainObj;
        private GameObject rootUIObj;
        private Button swapAssetsButton;

    void Start()
    {
        rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("ToothUI").gameObject;
        touchToInteractObj = transform.Find("TouchToInteractCanvas").gameObject;
        mainObj = transform.Find("InteractableTooth").gameObject;
        if (touchToInteractObj == null || mainObj == null || rootUIObj == null )
        {
            Debug.Log("Having trouble initialising Tooth asset");
        }
        //touchToInteractObj is a world canvas, that apparently needs to have its camera set. Do that here:
        touchToInteractObj.transform.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;

        mainObj.SetActive(false);
        touchToInteractObj.SetActive(true);
        
        
        //Add listeners to UI Buttons. These listeners need to be removed when this object is destroyed:
        swapAssetsButton = rootUIObj.transform.Find("CollapseAssetButton").gameObject.GetComponent<Button>();

        
        swapAssetsButton.onClick.AddListener(swapToInteractObj);
        swapAssetsButton.onClick.AddListener(_hideAssetUI);
        rootUIObj.GetComponent<toothAnim>().anim_A =
            mainObj.transform.Find("tooth anatomy anim").GetComponent<Animation>();

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
        // mainObjAnimation.Play("idle");
    }

    public void showAssetUI(XRBaseInteractor i)
    {   
        rootUIObj.SetActive(true);
    }

    public void hideAssetUI(XRBaseInteractor i)
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
        rootUIObj.GetComponent<toothAnim>().anim_A = null;
        rootUIObj.SetActive(false);
    }
    }
}