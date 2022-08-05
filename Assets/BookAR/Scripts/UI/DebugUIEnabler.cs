using System;
using UnityEngine;

namespace BookAR.Scripts.UI
{
    public class DebugUIEnabler : MonoBehaviour
    {
        [SerializeField] private bool enableDebugUI = true;
        [SerializeField] private GameObject debugCanvas1;
        [SerializeField] private GameObject debugCanvas2;
        [SerializeField] private GameObject debugCanvas3;


        private void OnEnable()
        {
            if (enableDebugUI)
            {
                if (debugCanvas1 != null)
                {
                    debugCanvas1.SetActive(true);
                }
                if (debugCanvas2 != null)
                {
                    debugCanvas2.SetActive(true);
                }
                if (debugCanvas3 != null)
                {
                    debugCanvas3.SetActive(true);
                }

            }
            else
            {
                if (debugCanvas1 != null)
                {
                    debugCanvas1.SetActive(false);
                }
                if (debugCanvas2 != null)
                {
                    debugCanvas2.SetActive(false);
                }
                if (debugCanvas3 != null)
                {
                    debugCanvas3.SetActive(false);
                }
            }
        }
    }
}