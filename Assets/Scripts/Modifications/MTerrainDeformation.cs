using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Modifications/TerrainDeformation")]
public class MTerrainDeformation : Modification
{
    [SerializeField, Min(1)]
    private float _radius = 1f;

    private float _curentRadius = 1f;

    public override void ApplyModification(MeshFilter[] meshFilters)
    {
        if (_curentRadius == _radius) return;

        if (meshFilters == null) return;

        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                if ((i % 2) == 0)
                    vertices[i] *= _radius;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }


}
