using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Modifications/SphereRadius")]
public class MSphereRadius : Modification
{
    [SerializeField, Min(1)]
    private float _radius = 1f;

    public override void ApplyModification(MeshFilter[] meshFilters)
    {
        if (meshFilters == null) return;
       
        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter == null || meshFilter.sharedMesh == null) continue;

            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= _radius; 
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals(); 
            mesh.RecalculateBounds();  
        }
    }
}
