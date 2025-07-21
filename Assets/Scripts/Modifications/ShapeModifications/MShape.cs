using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifications/Shape")]
public class MShape : Modification
{
    [SerializeField] private NoiseLayer[] _noiseLayers;

    private INoiseFilter[] _noiseFilters;

    public override void ApplyModification(MeshFilter[] meshFilters)
    {
        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = CalculatePointOnPlanet(vertices[i]);
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        _noiseFilters = new INoiseFilter[_noiseLayers.Length];
        for (int i = 0; i < _noiseFilters.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(_noiseLayers[i].GetNoiseSettings());
        }

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_noiseLayers[0].GetEnabled())
            {
                elevation = firstLayerValue;
            }
        }

        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (_noiseLayers[i].GetEnabled())
            {
                float mask = (_noiseLayers[i].GetUseFirstLayerAsMask()) ? firstLayerValue : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        return pointOnUnitSphere * (elevation + 1);
    }


}
