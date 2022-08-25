using System.Collections.Generic;
using UnityEngine;

public class CameraObstacle : MonoBehaviour
{
    [SerializeField] private Material _disabledMeshMaterial;

    public static Material DisabledMeshMaterial;

    private List<MeshRendererWithMaterials> _meshRenderers = new();

    private class MeshRendererWithMaterials
    {
        public MeshRenderer MeshRenderer { get; private set; }
        
        private Material[] _baseMaterials;
        private Material[] _disabledMaterials;

        public MeshRendererWithMaterials(MeshRenderer meshRenderer)
        {
            MeshRenderer = meshRenderer;

            _baseMaterials = meshRenderer.materials;
            
            _disabledMaterials = new Material[_baseMaterials.Length];

            for (int i = 0; i < _disabledMaterials.Length; i++)
            {
                var newMaterial = new Material(DisabledMeshMaterial);

                newMaterial.color = new Color(0, 0, 0, 0.2f);

                _disabledMaterials[i] = newMaterial;
            }
        }

        public void SetVisibleState(bool isVisible)
        {
            MeshRenderer.materials = (isVisible ? _baseMaterials : _disabledMaterials);
        }
    }

    private void Awake()
    {
        DisabledMeshMaterial = _disabledMeshMaterial;

        var meshRenderers = GetComponentsInChildren<MeshRenderer>();
        
        foreach (var meshRenderer in meshRenderers)
            _meshRenderers.Add(new MeshRendererWithMaterials(meshRenderer));
    }

    public void SetVisibleState(bool isVisible)
    {
        foreach (var meshRenderer in _meshRenderers)
            meshRenderer.SetVisibleState(isVisible);
    }
}