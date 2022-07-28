using UnityEngine;

namespace BookAR.Scripts.AssetControl.Audio
{
    public class PlayPauseAudioControl : MonoBehaviour, IAssetController
    {
        [SerializeField] private Canvas PlayCanvas;
        [SerializeField] private Canvas PauseCanvas;
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;

        private void OnEnable()
        {
            PlayCanvas.gameObject.SetActive(true);
            PauseCanvas.gameObject.SetActive(false);

        }

        public void OnPlayPressed()
        {
            PlayCanvas.gameObject.SetActive(false);
            PauseCanvas.gameObject.SetActive(true);
        }

        public void OnPausePressed()
        {
            PlayCanvas.gameObject.SetActive(true);
            PauseCanvas.gameObject.SetActive(false);
        }

    }
}