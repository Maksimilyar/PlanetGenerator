using UnityEngine;

namespace SphereGenerator
{
    public abstract class TerrainFace
    {
        protected Mesh _mesh;
        protected int _resolution;

        private Vector3[] _vertices;
        private int[] _triangles;

        public TerrainFace(Mesh mesh, int resolution)
        {
            this._mesh = mesh;
            this._resolution = resolution;
        }


        public void ConstructMesh()
        {
            if (_vertices == null || _triangles == null)
            {
                GenerateMeshData();
            }

            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.RecalculateNormals();

            _vertices = null;
            _triangles = null;
        }

        public void GenerateMeshData()
        {
            _vertices = GenerateVertices(_resolution);
            _triangles = GenerateTriangles(_resolution);
        }

        public void SetResolution(int resolution)
        {
            _resolution = resolution;
        }


        protected abstract Vector3[] GenerateVertices(int resolution);
        protected abstract int[] GenerateTriangles(int resolution);

    }
}


