using UnityEngine;

namespace SphereGenerator
{
    public class CubicSphere : Sphere
    {
        private const int countOfSides = 6;
        private static readonly Vector3[] cubeDirections =
        {
            Vector3.up, Vector3.down, Vector3.left,
            Vector3.right, Vector3.forward, Vector3.back
        };

        public CubicSphere() : base(countOfSides) { }

        protected override TerrainFace[] GenerateTerrainFaces(MeshFilter[] meshFilters, int resolution)
        {
            TerrainFace[] terrainFaces = new TerrainFace[meshFilters.Length];

            for (int i = 0; i < meshFilters.Length; i++)
            {
                terrainFaces[i] = new SquareTerrainFace(meshFilters[i].sharedMesh, resolution, cubeDirections[i]);
            }

            return terrainFaces;
        }
    }
}

