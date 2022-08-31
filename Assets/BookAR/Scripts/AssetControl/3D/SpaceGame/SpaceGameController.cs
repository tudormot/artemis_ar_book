using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scenes.BookAR.Scripts
{
    public class SpaceGameController : MonoBehaviour
    {
        private GameObject GameUI;
        private TextMeshProUGUI userInfoText;
        private GameObject ShootingModule;
        private GameObject ProjectileStartPosition;
        [SerializeField]private GameObject SolarSystem;
        private ParticleSystem GunParticleSystem;
        
        [SerializeField]
        private GameObject Projectile;

        private void OnEnable()
        {
            //first add/enable gun stuck to the camera
            //secondly add/enable a script/object to each planet that can respond to hits
            //enable game gui: button available: exit game, shoot. also a label panel to communicate with the user
            
            GameUI = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("SolarSystemUI").Find("GameUI").gameObject;
            userInfoText = GameUI.transform.Find()
            ShootingModule = Camera.main.transform.Find("Shooting").gameObject;
            ProjectileStartPosition = ShootingModule.transform.Find("MissileLaunchPoint").gameObject;
            GunParticleSystem = ShootingModule.transform
                .Find("Electricity").GetComponent<ParticleSystem>();
            GameUI.SetActive(true);
            ShootingModule.SetActive(true);
            
        
            GameUI.transform.Find("ShootButton").GetComponent<Button>().onClick.AddListener(ShootProjectile);
            SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = false;
            SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = false;

        }

       

        private void ShootProjectile()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "planet")
                 {
                     Debug.Log("Yo we just hit a planet that's cray!");
                     var projObj = Instantiate(Projectile,ProjectileStartPosition.transform.position,new Quaternion());
                     var scale = getSmartRocketScaling(projObj);
                     projObj.transform.localScale = new Vector3(scale, scale, scale);
                     projObj.GetComponent<FlyTowards>().Target = hit.collider.gameObject;
                     projObj.SetActive(true);
                     GunParticleSystem.Play();

                 }
            }

        }

        private float sunAvgSize = 0f;
        private float projectileAvgSize = 0f;

        private float getSmartRocketScaling(GameObject projectile)
        {
            if (sunAvgSize == 0f)
            {
                //rocket size should be 4 times smaller than the sun lets say
                sunAvgSize = SolarSystem.transform.Find("Planets").Find("Sun").GetComponent<MeshRenderer>()
                    .bounds.size.magnitude;
                projectileAvgSize = projectile.transform.Find("Rocket12_Red").GetComponent<MeshRenderer>().bounds.size
                    .magnitude;
            }

            
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