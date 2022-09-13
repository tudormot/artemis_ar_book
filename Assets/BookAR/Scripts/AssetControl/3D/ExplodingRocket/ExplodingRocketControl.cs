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
        TOUCH_TO_INTERACT_STATE,PREPARING_FOR_TAKEOFF, FLYING_ROCKET, EXPLODING_ROCKET
    }


    public class ExplodingRocketControl : MonoBehaviour, IAssetController, IStatefulController<RocketAssetState>
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;


        [SerializeField]private Button touchToInteractButton;
        [SerializeField]private Canvas touchToInteractCanvas;
        
        [SerializeField]private GameObject fullRocket;
        [SerializeField]private MeshRenderer destroyablePartOfRocket;
        
        [SerializeField]private ParticleSystem propulsionParticles1;
        [SerializeField]private ParticleSystem propulsionParticles2;
        [SerializeField]private ParticleSystem explosionParticles1;
        [SerializeField]private ParticleSystem explosionParticles2;


        private float TAKEOFF_PERIOD = 0.01f;
        private float FLYING_PERIOD = 5f;
        private float FLYING_DISTANCE = 2.5f;
        private float EXPLOSION_PERIOD = 1f;
        private RocketAssetState state;


        RocketAssetState IStatefulController<RocketAssetState>._state
        {
            get => state;
            set => state = value;
        }

        void IStatefulController<RocketAssetState>.OnStateChanged(RocketAssetState oldState, RocketAssetState newState)
        {
            switch (newState)
            {
                case RocketAssetState.TOUCH_TO_INTERACT_STATE:
                    Debug.Log("we are in TOUCH_TO_INTERACT_STATE");
                    touchToInteractCanvas.worldCamera = Camera.main;
                    touchToInteractCanvas.gameObject.SetActive(true);
                    fullRocket.SetActive(false);
                    break;
                case RocketAssetState.PREPARING_FOR_TAKEOFF:
                    touchToInteractCanvas.gameObject.SetActive(false);
                    fullRocket.SetActive(true);

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
                        () => { state = RocketAssetState.EXPLODING_ROCKET; }));
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
                case RocketAssetState.EXPLODING_ROCKET:
                    break;
            }
        }

        void OnEnable()
        {
            touchToInteractButton.onClick.AddListener(
                () =>
                {
                    (this as IStatefulController<RocketAssetState>).state = RocketAssetState.PREPARING_FOR_TAKEOFF;
                });
            (this as IStatefulController<RocketAssetState>).state = RocketAssetState.TOUCH_TO_INTERACT_STATE;

        }
        

    }
}
