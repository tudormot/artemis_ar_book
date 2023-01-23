using System.Collections.Generic;
using UnityEngine;

namespace BookAR.Scripts.AssetControl.Common.AssetQuitting
{
    public class ARExperienceHider
    {
        private List<GameObject> gameObjectsToDisable;


        private List<bool> savedStatesGameObjectIsActive;

        public ARExperienceHider(List<GameObject> o)
        {
            gameObjectsToDisable = o;
        }
        

        public void enableARExperience()
        {
            for (var i = 0; i < gameObjectsToDisable.Count; i++)
            {
                gameObjectsToDisable[i].SetActive(savedStatesGameObjectIsActive[i]);
            }
        }
        public void disableARExperience()
        {
            for (var i = 0; i < gameObjectsToDisable.Count; i++)
            {
                savedStatesGameObjectIsActive[i] = gameObjectsToDisable[i].activeSelf;
                gameObjectsToDisable[i].SetActive(false);
            }
        }
    }
}