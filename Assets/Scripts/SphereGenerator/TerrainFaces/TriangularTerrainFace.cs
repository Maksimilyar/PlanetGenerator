using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace SphereGenerator
{
    public class TriangularTerrainFace : TerrainFace
    {
        private readonly Vector3[] _meshVertices = new Vector3[3];
        private Vector3 _axisA;
        private Vector3 _axisB;
        private Vector3 _localUp;

        public TriangularTerrainFace(Mesh mesh, int resolution, Vector3 pointA, Vector3 pointB, Vector3 pointC) : base(mesh, resolution)
        {
            _meshVertices[0] = pointA;
            _meshVertices[1] = pointB;
            _meshVertices[2] = pointC;
            CalculateAxes();
        }

        private void CalculateAxes()
        {
            Vector3 AB = _meshVertices[1] - _meshVertices[0];
            Vector3 AC = _meshVertices[2] - _meshVertices[0];

            _localUp = Vector3.Cross(AB, AC).normalized;
            _axisA = (Mathf.Abs(_localUp.y) > 0.9f) ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);
            _axisB = Vector3.Cross(_localUp, _axisA).normalized;
            _axisA = Vector3.Cross(_axisB, _localUp).normalized;
        }

        protected override Vector3[] GenerateVertices(int resolution)
        {
            Vector3[] vertices = new Vector3[resolution * (resolution + 1) / 2];
            int vertexIndex = 0;

            for (int row = 0; row < resolution; row++)
            {
                float rowFactor = (float)row / (resolution - 1); 

                Vector3 leftEdge = Vector3.Lerp(_meshVertices[0], _meshVertices[1], rowFactor);
                Vector3 rightEdge = Vector3.Lerp(_meshVertices[0], _meshVertices[2], rowFactor);

                for (int col = 0; col <= row; col++)
                {
                    float colFactor = row == 0 ? 0 : (float)col / row; 

                    Vector3 pointOnUnitTriangle = Vector3.Lerp(leftEdge, rightEdge, colFactor);

                    Vector3 pointOnUnitSphere = pointOnUnitTriangle.normalized / 2;

                    vertices[vertexIndex++] = pointOnUnitSphere;
                }
            }

            return vertices;
        }

        protected override int[] GenerateTriangles(int resolution)
        {
            List<int> triangles = new List<int>();

            int index = 0;
            for (int row = 0; row < resolution - 1; row++)
            {
                for (int col = 0; col <= row; col++)
                {
                    int a = index;
                    int b = index + row + 1;
                    int c = index + row + 2;

                    triangles.Add(a);
                    triangles.Add(b);
                    triangles.Add(c);

                    if (col < row)
                    {
                        int d = index + 1;
                        triangles.Add(a);
                        triangles.Add(c);
                        triangles.Add(d);
                    }

                    index++;
                }
            }

            return triangles.ToArray();
        }
    }
}







