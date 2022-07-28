using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BookAR.Scripts._3D.SkullAndBrain
{
    public class SelectionShaderActivator : MonoBehaviour
    {
        private static readonly int IsSelected = Shader.PropertyToID("is_selected");

        [SerializeField] private GameObject selectableObject;
        private List<Material> materials;

        private void OnEnable()
        {
            materials = selectableObject.GetComponentsInChildren<MeshRenderer>().Select(
                (r)=>r.material
                ).ToList();
            foreach (var material in materials)
            {
                if (material.shader.name == "EasyGameStudio/SelectEffect")
                {
                    material.SetFloat(IsSelected,1);

                }
                else
                {
                    Debug.LogWarning("found a material in current prefab which does not use the SelectEffect shader. Is this desired?");
                }
            }
        }

        private void OnDisable()
        {
            foreach (var material in materials)
            {
                if (material.shader.name == "EasyGameStudio/SelectEffect")
                {
                    material.SetFloat(IsSelected,0);

                }
            }
        }
    }
}