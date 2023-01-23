using BookAR.Scripts.AssetControl._3D.SkullAndBrain;
using BookAR.Scripts.Utils.Coroutines;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;
using UnityEngine.UI;

namespace BookAR.Scripts.AssetControl._3D.ExplodingRocket
{
    public enum RocketAssetState
    {
        TOUCH_TO_INTERACT_STATE,PREPARING_FOR_TAKEOFF, FLYING_ROCKET, OCCLUDED_STATE
    }


    public class ExplodingRocketControl : IAssetController, IStatefulController<RocketAssetState>
    {
        public override AssetControllerType type { get; protected set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        public override void reactToCollapseRequest()
        {
            if ((this as IStatefulController<RocketAssetState>).state != RocketAssetState.OCCLUDED_STATE)
            {
                (this as IStatefulController<RocketAssetState>).state = RocketAssetState.TOUCH_TO_INTERACT_STATE;
            }
        }

        public override void reactToOcclusionEvent(OcclusionEvent e)
        {
            if (e == OcclusionEvent.IMAGE_OCCLUDED)
            {
                (this as IStatefulController<RocketAssetState>).state = RocketAssetState.OCCLUDED_STATE;
            }
            else
            {
                (this as IStatefulController<RocketAssetState>).state = RocketAssetState.TOUCH_TO_INTERACT_STATE;
            }
        }


        [SerializeField]private Button touchToInteractButton;
        [SerializeField]private Canvas touchToInteractCanvas;
        
        [SerializeField]private GameObject fullRocket;
        [SerializeField]private MeshRenderer destroyablePartOfRocket;
        
        [SerializeField]private ParticleSystem propulsionParticles1;
        [SerializeField]private ParticleSystem propulsionParticles2;
        [SerializeField]private ParticleSystem explosionParticles1;
        [SerializeField]private ParticleSystem explosionParticles2;

        private Transform initialRocketPosition;
        private float TAKEOFF_PERIOD = 0.01f;
        private float FLYING_PERIOD = 5f;
        private float FLYING_DISTANCE = 2f;
        private float EXPLOSION_PERIOD = 1f;


        public RocketAssetState _state { get; set; }

        void IStatefulController<RocketAssetState>.OnStateChanged(RocketAssetState oldState, RocketAssetState newState)
        {
            switch (newState)
            {
                case RocketAssetState.TOUCH_TO_INTERACT_STATE:
                    propulsionParticles1.Stop();
                    propulsionParticles2.Stop();
                    explosionParticles1.Stop();
                    explosionParticles2.Stop();

                    touchToInteractCanvas.gameObject.SetActive(true);
                    fullRocket.SetActive(false);
                    destroyablePartOfRocket.gameObject.SetActive(true);
                    break;
                case RocketAssetState.OCCLUDED_STATE:
                    Debug.Log("in OCCLUDED STATE");
                    propulsionParticles1.Stop();
                    propulsionParticles2.Stop();
                    explosionParticles1.Stop();
                    explosionParticles2.Stop();
                    touchToInteractCanvas.gameObject.SetActive(false);
                    fullRocket.SetActive(false);
                    break;
                case RocketAssetState.PREPARING_FOR_TAKEOFF:
                    touchToInteractCanvas.gameObject.SetActive(false);
                    fullRocket.SetActive(true);
                    fullRocket.transform.DOLocalMoveY(0, 0);

                    propulsionParticles1.Play();
                    propulsionParticles2.Play();

                    StartCoroutine(ConditionalCoroutineUtils.ExecuteOnceAfterPeriod(
                        TAKEOFF_PERIOD,
                        () =>
                        {
                            (this as IStatefulController<RocketAssetState>).state = RocketAssetState.FLYING_ROCKET;

                        }));
                    break;
                case RocketAssetState.FLYING_ROCKET:
                    fullRocket.transform.DOLocalMoveY(FLYING_DISTANCE, FLYING_PERIOD)
                        .SetEase(Ease.InOutCubic);

                    StartCoroutine(ConditionalCoroutineUtils.ExecuteOnceAfterPeriod(
                        FLYING_PERIOD,
                        () => { (this as IStatefulController<RocketAssetState>).state = RocketAssetState.TOUCH_TO_INTERACT_STATE; }));
                    StartCoroutine(ConditionalCoroutineUtils.ExecuteOnceAfterPeriod(
                        FLYING_PERIOD * 3 / 4,
                        () =>
                        {
                            propulsionParticles1.Stop();
                            propulsionParticles2.Stop();
                            explosionParticles1.Play();
                            explosionParticles2.Play();
                        }));

                    StartCoroutine(ConditionalCoroutineUtils.ExecuteOnceAfterPeriod(
                        FLYING_PERIOD * 5 / 6,
                        () => { destroyablePartOfRocket.gameObject.SetActive(false); }));
                    break;

            }
        }

        void OnEnable()
        {
            touchToInteractCanvas.worldCamera = Camera.main;
            initialRocketPosition = fullRocket.transform;
            touchToInteractButton.onClick.AddListener(
                () =>
                {
                    base.onTouchToInteractButtonPressed();
                    (this as IStatefulController<RocketAssetState>).state = RocketAssetState.PREPARING_FOR_TAKEOFF;
                });
            (this as IStatefulController<RocketAssetState>).state = RocketAssetState.TOUCH_TO_INTERACT_STATE;

        }
        

    }
}
