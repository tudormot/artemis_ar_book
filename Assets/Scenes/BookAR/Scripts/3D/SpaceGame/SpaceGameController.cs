using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes.BookAR.Scripts
{
    public class SpaceGameController : MonoBehaviour
    {
        private GameObject GameUI;
        private GameObject ShootingModule;
        private GameObject ProjectileStartPosition;
        private GameObject SolarSystem;
        private ParticleSystem GunParticleSystem;
        
        [SerializeField]
        private GameObject Projectile;

        private void OnEnable()
        {
            //first add/enable gun stuck to the camera
            //secondly add/enable a script/object to each planet that can respond to hits
            //enable game gui: button available: exit game, shoot. also a label panel to communicate with the user
            
            GameUI = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SolarSystemUI").Find("GameUI").gameObject;
            ShootingModule = GameObject.Find("AR Session Origin").transform.Find("AR Camera").Find("Shooting").gameObject;
            SolarSystem = transform.parent.Find("Solar System").gameObject;
            ProjectileStartPosition = GameObject.Find("AR Session Origin").transform.Find("AR Camera").Find("Shooting").Find("MissileLaunchPoint").gameObject;
            GunParticleSystem = GameObject.Find("AR Session Origin").transform.Find("AR Camera").Find("Shooting")
                .Find("Electricity").GetComponent<ParticleSystem>();
            GameUI.SetActive(true);
            ShootingModule.SetActive(true);
            
            GameUI.transform.Find("SunButton").GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    Debug.Log("SubButton clicked!");
                    var debugLabel = GameUI.transform.Find("DebugPanel").Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
                    Debug.Log("Found the debug canvas label");
                    debugLabel.text = SolarSystem.transform.Find("Planets").Find("Sun").GetComponent<MeshRenderer>()
                        .bounds.size.ToString();
                    Debug.Log("button callback finished");
                });
            GameUI.transform.Find("ShootButton").GetComponent<Button>().onClick.AddListener(ShootProjectile);
            SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = false;
            SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = false;

        }

       

        private void ShootProjectile()
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (!EventSystem.current.IsPointerOverGameObject()) //If the mouse is not over a UI element
            {
                 if (Physics.Raycast(ray, out hit))
                 {
                     if (hit.collider.tag == "planet")
                     {
                         Debug.Log("Yo we just hit a planet that's cray!");
                         // var projObj = Instantiate(Projectile,ProjectileStartPosition.transform);
                         var projObj = Instantiate(Projectile,ProjectileStartPosition.transform.position,new Quaternion());
                         var scale = getSmartRocketScaling(projObj);
                         projObj.transform.localScale = new Vector3(scale, scale, scale);
                             // ProjectileStartPosition.transform.lossyScale;
                         projObj.GetComponent<FlyTowards>().Target = hit.collider.gameObject;
                         projObj.SetActive(true);
                         GunParticleSystem.Play();

                     }
                 }
            }
            
        }

        private float getSmartRocketScaling(GameObject projectile)
        {
            //rocket size should be 8 times smaller than the earth lets say
            var sunAvgSize = SolarSystem.transform.Find("Planets").Find("Sun").GetComponent<MeshRenderer>()
                .bounds.size.magnitude;
            var projectileAvgSize = projectile.transform.Find("Rocket12_Red").GetComponent<MeshRenderer>().bounds.size
                .magnitude;
            return sunAvgSize / projectileAvgSize / 4;



        }

        private void OnDisable()
        {
            GameUI.SetActive(false);
            ShootingModule.SetActive(false);
            GameUI.transform.Find("SunButton").GetComponent<Button>().onClick.RemoveAllListeners();
            GameUI.transform.Find("ShootButton").GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}