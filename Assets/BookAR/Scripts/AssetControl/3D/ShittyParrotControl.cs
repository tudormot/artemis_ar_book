using UnityEngine;

namespace BookAR.Scripts.AssetControl._3D
{
    public class ShittyParrotControl : MonoBehaviour, IAssetController
    {
        AssetControllerType IAssetController.type { get; set; } = AssetControllerType.DEFAULT_ASSET_TYPE;
        public Animation parrotAnimation;
        private int State = 0;

        void Update() {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // Construct a ray from the current touch coordinates
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    // Create a particle if hit
                    if (Physics.Raycast(ray))
                    {
                        if (State == 0)
                        {
                            State = 1;
                            play_idle_anim();
                        }
                        else
                        {
                            State = 0;
                            play_startflight_anim();
                        }
                    }
                }
            }
        }
    

        public void play_idle_anim()
        {
            parrotAnimation.Play("Idle_anim");

        }
        
        public void play_startflight_anim()
        {
            parrotAnimation.Play("Takeflight_anim");

        }
    }
}