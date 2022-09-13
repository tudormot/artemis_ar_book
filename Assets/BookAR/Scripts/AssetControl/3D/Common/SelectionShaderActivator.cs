using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BookAR.Scripts.AssetControl._3D.Common
{
    public class SelectionShaderActivator : MonoBehaviour
    {
        private static readonly int IsSelected = Shader.PropertyToID("is_selected");

        [SerializeField] private GameObject selectableObject;
        private List<Material> materials;

        private void OnEnable()
        {
            Debug.Log("SelectionShaderActivator enabled!");
            materials = selectableObject.GetComponentsInChildren<MeshRenderer>().Select(
                (r)=>r.material
                ).Concat(
                selectableObject.GetComponentsInChildren<SkinnedMeshRenderer>().Select(
                    (r)=>r.material
                )).ToList();

            var possibleMaterial = selectableObject.GetComponent<SkinnedMeshRenderer>()?.material;
            if (possibleMaterial != null)
            {
                materials.Add(possibleMaterial);
            }

            possibleMaterial = selectableObject.GetComponent<MeshRenderer>()?.material;
            if (possibleMaterial != null)
            {
                materials.Add(possibleMaterial);
            }

            Debug.Log($"materials: {materials}");
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
            Debug.Log($"onDisable called!");

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