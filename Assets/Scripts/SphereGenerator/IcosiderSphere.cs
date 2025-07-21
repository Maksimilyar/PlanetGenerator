using UnityEngine;
using System;

namespace SphereGenerator
{
    public class IcosiderSphere : Sphere
    {
        private const int countOfSides = 20;

        private readonly int[,] faces =
        {
            { 0, 1, 8 }, { 0, 10, 1 }, { 9, 3, 2 }, { 3, 11, 2 },
            { 4, 5, 0 }, { 5, 4, 2 }, { 1, 7, 6 }, { 3, 6, 7 },
            { 8, 9, 4 }, { 9, 8, 6 }, { 5, 11, 10 }, { 7, 10, 11 },
            { 0, 8, 4 }, { 0, 5, 10 }, { 1, 6, 8 }, { 1, 10, 7 },
            { 2, 4, 9 }, { 2, 11, 5 }, { 9, 6, 3 }, { 3, 7, 11 }
        };

        private readonly Vector3[] vertices =
        {
            new Vector3(0, 1, Fi()),
            new Vector3(0, -1, Fi()),
            new Vector3(0, 1, -Fi()),
            new Vector3(0, -1, -Fi()),
            new Vector3(1, Fi(), 0),
            new Vector3(-1, Fi(), 0),
            new Vector3(1, -Fi(), 0),
            new Vector3(-1, -Fi(), 0),
            new Vector3(Fi(), 0, 1),
            new Vector3(Fi(), 0, -1),
            new Vector3(-Fi(), 0, 1),
            new Vector3(-Fi(), 0, -1),
        };

        public IcosiderSphere() : base(countOfSides) { }

        protected override TerrainFace[] GenerateTerrainFaces(MeshFilter[] meshFilters, int resolution)
        {
            
            TerrainFace[] terrainFaces = new TerrainFace[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                Vector3 pointA = vertices[faces[i, 0]];
                Vector3 pointB = vertices[faces[i, 1]];
                Vector3 pointC = vertices[faces[i, 2]];
                terrainFaces[i] = new TriangularTerrainFace(meshFilters[i].sharedMesh, resolution, pointA, pointB, pointC);
            }

            return terrainFaces;
        }

        private static float Fi()
        {
            return ((float)Math.Sqrt(5) + 1) / 2;
        }
    }
}
