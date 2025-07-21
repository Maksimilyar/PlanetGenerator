using UnityEngine;

namespace SphereGenerator
{
    public class SquareTerrainFace : TerrainFace
    {
        private Vector3 _axisA;
        private Vector3 _axisB;
        private Vector3 _localUp;

        public SquareTerrainFace(Mesh mesh, int resolution, Vector3 localUp) : base(mesh, resolution)
        {
            _localUp = localUp;
            CalculateAxes();
        }

        private void CalculateAxes()
        {
            _axisA = (Mathf.Abs(_localUp.y) > 0.9f) ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);
            _axisB = Vector3.Cross(_localUp, _axisA).normalized;
            _axisA = Vector3.Cross(_axisB, _localUp).normalized;
        }

        protected override Vector3[] GenerateVertices(int resolution)
        {
            Vector3[] vertices = new Vector3[resolution * resolution];

            float stepSize = 2f / (resolution - 1);
            for (int x = 0; x < resolution; x++)
            {
                for (int y = 0; y < resolution; y++)
                {
                    int i = x + y * resolution;
                    Vector2 percent = new(x * stepSize - 1, y * stepSize - 1);
                    Vector3 pointOnUnitCube = _localUp + percent.x * _axisA + percent.y * _axisB;
                    vertices[i] = pointOnUnitCube.normalized / 2; 
                }
            }

            return vertices;
        }

        protected override int[] GenerateTriangles(int resolution)
        {
            int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
            int tryIndex = 0;

            for (int x = 0; x < resolution - 1; x++)
            {
                for (int y = 0; y < resolution - 1; y++)
                {
                    int i = x + y * resolution;

                    triangles[tryIndex] = i;
                    triangles[tryIndex + 1] = i + 1 + resolution;
                    triangles[tryIndex + 2] = i + resolution;

                    triangles[tryIndex + 3] = i;
                    triangles[tryIndex + 4] = i + 1;
                    triangles[tryIndex + 5] = i + 1 + resolution;

                    tryIndex += 6;
                }
            }

            return triangles;
        }



    }
}


