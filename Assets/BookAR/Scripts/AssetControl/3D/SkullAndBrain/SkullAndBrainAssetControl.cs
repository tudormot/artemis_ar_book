using BookAR.Scripts.Utils.Coroutines;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.AR;

namespace BookAR.Scripts.AssetControl._3D.SkullAndBrain
{
    public enum SkullAssetState
    {
        TOUCH_TO_INTERACT_STATE, MINIMIZED_SKULL, EXPANDED_SKULL, LABELED_SKULL
    }

    public class SkullAndBrainAssetControl : MonoBehaviour, IAssetController
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        [SerializeField]private Button touchToInteractButton;
        [SerializeField]private Canvas touchToInteractCanvas;
        [SerializeField] private LabelController labelController;
        [SerializeField]private Animation mainObjAnimation;
        [SerializeField]private ARSelectionInteractable selectInteractable;

        private GameObject rootUIObj;
    
        private Button expandSkullButton;
        private Button retractSkullButton;
        private Button collapseAssetButton;
        private Button showLabelsButton;
        private Button hideLabelsButton;

        private SkullAssetState _state;
        public SkullAssetState state
        {
            get => _state;
            set
            {
                onStateChanged(value);
                _state = value;
                
            }
        }

        private void onStateChanged(SkullAssetState newState)
        {
            switch (newState)
            {
                case SkullAssetState.TOUCH_TO_INTERACT_STATE:
                    //touchToInteractObj is a world canvas, that apparently needs to have its camera set. Do that here:
                    touchToInteractCanvas.worldCamera = Camera.main;
                    mainObjAnimation.gameObject.SetActive(false); 
                    touchToInteractCanvas.gameObject.SetActive(true);
                    rootUIObj.SetActive(false);
                    labelController.state = LabelControllerState.LABELS_HIDDEN;
                    break;
                case SkullAssetState.MINIMIZED_SKULL:
                    // rootUIObj.SetActive(true);
                    touchToInteractCanvas.gameObject.SetActive(false);
                    mainObjAnimation.gameObject.SetActive(true);
                    expandSkullButton.gameObject.SetActive(true);
                    retractSkullButton.gameObject.SetActive(false);
                    collapseAssetButton.gameObject.SetActive(true);
                    showLabelsButton.gameObject.SetActive(false);
                    hideLabelsButton.gameObject.SetActive(false);
                    labelController.state = LabelControllerState.LABELS_HIDDEN;
                    break;
                case SkullAssetState.EXPANDED_SKULL:
                    expandSkullButton.gameObject.SetActive(false);
                    retractSkullButton.gameObject.SetActive(true);
                    collapseAssetButton.gameObject.SetActive(true);
                    showLabelsButton.gameObject.SetActive(true);
                    hideLabelsButton.gameObject.SetActive(false);
                    labelController.state = LabelControllerState.LABELS_HIDDEN;
                    break;
                case SkullAssetState.LABELED_SKULL:
                    expandSkullButton.gameObject.SetActive(false);
                    retractSkullButton.gameObject.SetActive(true);
                    collapseAssetButton.gameObject.SetActive(true);
                    showLabelsButton.gameObject.SetActive(false);
                    hideLabelsButton.gameObject.SetActive(true);
                    labelController.state = LabelControllerState.LABELS_SHOWN;
                    break;
                    


            }
        }

        void OnEnable()
        {
            Debug.Log("DEBUG1");
            rootUIObj = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SkullAndBrainUI").gameObject;
            Debug.Log("DEBUG1.1");

            if (touchToInteractButton == null || rootUIObj == null || mainObjAnimation == null)
            {
                Debug.LogError("Having trouble initialising Skull and Brain asset");
            }
            Debug.Log("DEBUG1.2");

        
            //Add listeners to UI Buttons. These listeners need to be removed when this object is destroyed:
            collapseAssetButton = rootUIObj.transform.Find("CollapseAssetButton").gameObject.GetComponent<Button>();
            expandSkullButton = rootUIObj.transform.Find("ExpandButton").gameObject.GetComponent<Button>();
            retractSkullButton = rootUIObj.transform.Find("JoinButton").gameObject.GetComponent<Button>();
            showLabelsButton = rootUIObj.transform.Find("ShowLabelsButton").gameObject.GetComponent<Button>();
            hideLabelsButton = rootUIObj.transform.Find("HideLabelsButton").gameObject.GetComponent<Button>();
            Debug.Log("DEBUG1.3");


            if (retractSkullButton == null || expandSkullButton == null || collapseAssetButton == null || showLabelsButton == null || hideLabelsButton == null )
            {
                Debug.LogError("Problem with finding required UI components for Skull and Brain Asset");
            }

            Debug.Log("DEBUG1.4");

            touchToInteractButton.onClick.AddListener(
                () =>
                {
                    state = SkullAssetState.MINIMIZED_SKULL;
                });
            Debug.Log("DEBUG1.42");

            selectInteractable.selectEntered.AddListener( onSelectEntered);
            selectInteractable.selectExited.AddListener(onSelectExited);
            Debug.Log("DEBUG1.5");

            collapseAssetButton.onClick.AddListener(() =>
            {
                state = SkullAssetState.TOUCH_TO_INTERACT_STATE;
            });
            retractSkullButton.onClick.AddListener(()=>{
                state = SkullAssetState.MINIMIZED_SKULL;
            });
            expandSkullButton.onClick.AddListener(() =>
            {
                mainObjAnimation.Play("expand_tudor_mod");
                StartCoroutine(
                    ConditionalCoroutineUtils.ConditionalExecutionCoroutine(
                        conditional: () => mainObjAnimation.isPlaying == false,
                        () => state = SkullAssetState.EXPANDED_SKULL,
                        timeout: 10 * 60 * 60
                    )
                );
            });
            Debug.Log("DEBUG2");

            
            retractSkullButton.onClick.AddListener(() =>
            {
                mainObjAnimation.Play("join_tudor_mod");
                StartCoroutine(
                    ConditionalCoroutineUtils.ConditionalExecutionCoroutine(
                        conditional: () => mainObjAnimation.isPlaying == false,
                        () => state = SkullAssetState.MINIMIZED_SKULL,
                        timeout: 10 * 60 * 60
                    )
                );
            });
            showLabelsButton.onClick.AddListener(
                () =>
                {
                    state = SkullAssetState.LABELED_SKULL;
                }
            );
            Debug.Log("DEBUG2.5");

            hideLabelsButton.onClick.AddListener(
                () =>
                {
                    Debug.Log("Hide labels button was clicked!");
                    state = SkullAssetState.EXPANDED_SKULL;
                }
            );

            state = SkullAssetState.TOUCH_TO_INTERACT_STATE;
            Debug.Log("DEBUG3");

        }
        

        private void onSelectEntered(SelectEnterEventArgs e)
        {   
            rootUIObj.SetActive(true);
        }

        void onSelectExited(SelectExitEventArgs e)
        {
            if (e.isCanceled)
            {
                rootUIObj.SetActive(false);
            }
            else
            {
                Debug.Log("What is happening ? :)");
            }
        }

        private void OnDisable()
        {
            expandSkullButton.onClick.RemoveAllListeners();
            retractSkullButton.onClick.RemoveAllListeners();
            collapseAssetButton.onClick.RemoveAllListeners();
            showLabelsButton.onClick.RemoveAllListeners();
            hideLabelsButton.onClick.RemoveAllListeners();
            selectInteractable.selectEntered.RemoveAllListeners();
            selectInteractable.selectExited.RemoveAllListeners();
            rootUIObj.SetActive(false);
        }
    }
}
