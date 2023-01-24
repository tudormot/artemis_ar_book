using System;
using System.Collections.Generic;
using System.Linq;
using ThirdPartyAssets.solar_system.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

namespace Scenes.BookAR.Scripts
{
    public class SpaceGameController : MonoBehaviour
    {
        private GameObject GameUI;
        private TextMeshProUGUI userInfoText;
        private Toggle showLabelsToggle;
        private Toggle showPathsToggle;
        private Animator userInfoPanelAnimator;
        private GameObject ShootingModule;
        private GameObject ProjectileStartPosition;
        [SerializeField]private GameObject SolarSystem;
        private ParticleSystem GunParticleSystem;

        [SerializeField]
        private List<DestroyablePlanet> allDestroyablePlanets;

        [SerializeField]
        private GameObject Projectile;

        private static readonly List<string> allPlanets = new List<string>{
            "Saturn",
            "Jupiter",
            "Sun",
            "Mercury",
            "Venus",
            "Earth",
            "Moon",
            "Mars",
            "Uranus",
            "Neptune"
        };

        private List<String> planets = allPlanets.ToList();
        


        private string currentPlanetTargetName;

        private void OnEnable()
        {
            //first add/enable gun stuck to the camera
            //secondly add/enable a script/object to each planet that can respond to hits
            //enable game gui: button available: exit game, shoot. also a label panel to communicate with the user
            GameUI = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("StandaloneAssetUIs").Find("SolarSystemUI").Find("GameUI").gameObject;
            userInfoText = GameUI.transform.Find("UserInfoPanel").GetChild(0).GetComponent<TextMeshProUGUI>();
            
            GameUI.transform.Find("Buttons").transform.Find("Toggle labels").GetComponent<Toggle>().onValueChanged.AddListener(
                b => { SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = b; });
            GameUI.transform.Find("Buttons").transform.Find("Toggle paths").GetComponent<Toggle>().onValueChanged.AddListener(
                b => { SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = b; });
            

            
            ShootingModule = Camera.main.transform.Find("Shooting").gameObject;
            ProjectileStartPosition = ShootingModule.transform.Find("MissileLaunchPoint").gameObject;
            GunParticleSystem = ShootingModule.transform
                .Find("Electricity").GetComponent<ParticleSystem>();

            GameUI.SetActive(true);
            ShootingModule.SetActive(true);

            userInfoPanelAnimator = GameUI.GetComponent<Animator>();
            GameUI.transform.Find("Buttons").transform.Find("ShootButton").GetComponent<Button>().onClick.AddListener(ShootProjectile);
            

            
            userInfoPanelAnimator.SetBool(IsPanelShown, true);
            SolarSystem.GetComponent<SolarSystemManager>().ShowLabels = GameUI.transform.Find("Buttons").transform.Find("Toggle labels").GetComponent<Toggle>().isOn;
            SolarSystem.GetComponent<SolarSystemManager>().ShowPaths = GameUI.transform.Find("Buttons").transform.Find("Toggle paths").GetComponent<Toggle>().isOn;
            
            StartGame();

        }

        private String getNewPlanetTargetName()
        {
            int randomInt = new Random().Next(planets.Count);
            return planets[randomInt];
        }

        private void ShootProjectile()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "planet")
                 {
                     var projObj = Instantiate(Projectile,ProjectileStartPosition.transform.position,new Quaternion());
                     var scale = getSmartRocketScaling(projObj);
                     projObj.transform.localScale = new Vector3(scale, scale, scale);
                     projObj.GetComponent<FlyTowards>().Target = hit.collider.gameObject;
                     projObj.SetActive(true);
                     GunParticleSystem.Play();

                     var destroyablePlanet = hit.collider.gameObject.GetComponent<DestroyablePlanet>();
                     var hitsLeft = (destroyablePlanet.PlanetHP - 10) / 10;
                     if (hit.collider.gameObject.name == currentPlanetTargetName)
                     {
                         if (hitsLeft == 0)
                         {
                             planets.RemoveAt(planets.IndexOf(currentPlanetTargetName));

                             if (planets.Count == 0)
                             {
                                 userInfoText.text = $"The solar system was completely destroyed. You sure can fire those missiles, Commander! ;)";
                             }
                             else
                             {
                                 var newTarget = getNewPlanetTargetName();
                                 userInfoText.text = $"Nice one Commander! {currentPlanetTargetName} successfully destroyed! Your new target is {newTarget}. Give them hell!";
                                 currentPlanetTargetName = newTarget;
                             }

                         }
                         else
                         {
                             userInfoText.text = $"Nice one Commander! You attacked the correct planet! {hitsLeft} rockets will do.";
                         }

                     }
                     else
                     {
                         if (hitsLeft == 0)
                         {
                             planets.RemoveAt(planets.IndexOf(hit.collider.gameObject.name));
                             if (planets.Count > 0)
                             {
                                 userInfoText.text = $"God dang it Commander! You have completely destroyed {hit.collider.gameObject.name}! You were supposed to attack {currentPlanetTargetName}!!";
                             }
                             else
                             {
                                 userInfoText.text = $"Great, now the whole solar system is destroyed, and humanity has no chance of survival. You really need to learn your planet names, Commander!!";
                             }

                         }
                         else
                         {
                             userInfoText.text = $"Commander! You are attacking the wrong planet! Are you trying to destroy the Solar System?! You were supposed to attack {currentPlanetTargetName}, not {hit.collider.gameObject.name}!!";
                         }


                     }

                 }
            }

        }

        private float sunAvgSize = 0f;
        private float projectileAvgSize = 0f;
        private static readonly int IsPanelShown = Animator.StringToHash("isPanelShown");

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

        public void ResetGame()
        {
            planets = allPlanets.ToList();
            foreach (var planet in allDestroyablePlanets)
            {
                planet.resetPlanet();
            }
        }

        public void StartGame()
        {
            currentPlanetTargetName = getNewPlanetTargetName();
            userInfoText.text = $"{currentPlanetTargetName}'s energy field malfunctioned! This is your chance to strike, Commander!";
        }

        public void HideGame()
        {
            if (isActiveAndEnabled)
            {
                GameUI.SetActive(false);
                ShootingModule.SetActive(false);
            }
        }

        public void RevealGame()
        {
            GameUI.SetActive(true);
            ShootingModule.SetActive(true);
        }


        private void OnDisable()
        {
            ResetGame();
            GameUI.SetActive(false);
            ShootingModule.SetActive(false);
            GameUI.transform.Find("Buttons").Find("ShootButton").GetComponent<Button>().onClick.RemoveAllListeners();

        }
    }
}