using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using System;

namespace SphereGenerator
{
    [DisallowMultipleComponent]
    public abstract class Sphere : MonoBehaviour
    {
        [Range(2, 255), SerializeField] 
        protected int _resolution = 2;

        [SerializeField, HideInInspector]
        protected MeshFilter[] _meshFilters;

        [SerializeField, HideInInspector]
        protected TerrainFace[] _terrainFaces;

        [SerializeField, HideInInspector]
        protected int _countOfSides;

        public Sphere(int countOfSides)
        {
            _countOfSides = countOfSides;
        }

        public virtual void Initialize()
        {
            if (_meshFilters == null || _meshFilters.Length != _countOfSides)
            {
                CreateMesh();
            }

            _terrainFaces ??= GenerateTerrainFaces(_meshFilters, _resolution);

            GenerateMeshAsynk();
        }

        public void GenerateSphere()
        {
            Initialize();
        }

        public void GenerateSphere(int resolution)
        {
            if (resolution < 2 || resolution > 255 || _resolution == resolution) return;

            _resolution = resolution;
            GenerateSphere();
        }

        public void GenerateSphere(int resolution, bool IsMeshGenerated)
        {
            if (IsMeshGenerated == true)
            {
                GenerateSphere(resolution);
            }
        }

        

        public void UppdateMesh()
        {
            foreach (MeshFilter meshFilter in _meshFilters)
            {
                meshFilter.sharedMesh.RecalculateNormals();
            }
        }

        public void DestroyMesh()
        {
            if (_meshFilters == null) return;

            foreach (var filter in _meshFilters)
            {
                if (filter != null)
                {
                    DestroyImmediate(filter.gameObject);
                }
            }
        }

        public MeshFilter[] GetMeshFilters()
        {
            if (_meshFilters == null) return null;
            return _meshFilters.ToArray();
        }

        public int GetResolution()
        {
            return _resolution;
        }

        public int GetCountOfSides()
        {
            return _countOfSides;
        }

        private void GenerateMesh()
        {
            foreach (var face in _terrainFaces)
            {
                face.SetResolution(_resolution);
                face.GenerateMeshData();
                face.ConstructMesh();
            }
        }

        private void GenerateMeshAsynk()
        {
            Parallel.ForEach(_terrainFaces, face =>
            {
                face.SetResolution(_resolution);
                face.GenerateMeshData();
            });

            foreach (var face in _terrainFaces)
            {
                face.ConstructMesh();
            }
        }

        protected abstract TerrainFace[] GenerateTerrainFaces(MeshFilter[] meshFilters, int resolution);

        private void CreateMesh()
        {
            _meshFilters = new MeshFilter[_countOfSides];

            for (int i = 0; i < _countOfSides; i++)
            {
                if (_meshFilters[i] == null)
                {
                    GameObject meshObject = new("_mesh");
                    meshObject.transform.parent = transform;
                    meshObject.transform.localPosition = Vector3.zero;

                    meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }
            }

        }
    }
}
