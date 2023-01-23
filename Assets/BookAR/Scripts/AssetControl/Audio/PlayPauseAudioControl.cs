using UnityEngine;

namespace BookAR.Scripts.AssetControl.Audio
{
    public class PlayPauseAudioControl : IAssetController
    {
        [SerializeField] private Canvas PlayCanvas;
        [SerializeField] private Canvas PauseCanvas;
        public override AssetControllerType type { get; protected set; }

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

        public override void reactToCollapseRequest()
        {
            throw new System.NotImplementedException();
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}