using SphereGenerator;
using Unity.VisualScripting;
using UnityEngine;

public class CelestialBodyTerrain : MonoBehaviour
{
    private Sphere _sphere;

    private MeshFilter[] _originalMeshFilters;

    private MeshFilter[] _meshFilters;

    private Modification[] _modifications;

    private bool _autoUpdate = true;

    public Sphere Sphere
    {
        get => _sphere;
        internal set
        {
            if (value != null)
            {
                _sphere = value;
                UpdateSphere();
            }
        }
    }

    public Modification[] Modifications
    {
        get => _modifications;
        internal set
        {
            if (value != null)
            {
                _modifications = value;
                UpdateSphere();
            }
        }
    }

    public bool AutoUpdate
    {
        get => _autoUpdate;
        internal set
        {
            _autoUpdate = value;
        }
    }

    internal void ApplyModifications()
    {
        if (_modifications == null) return;

        for (int i = 0; i < _originalMeshFilters.Length; i++)
        {
            Mesh sourceMesh = _originalMeshFilters[i].sharedMesh;
            Mesh targetMesh = _meshFilters[i].sharedMesh;

            if (targetMesh == null)
            {
                targetMesh = Instantiate(sourceMesh);
                _meshFilters[i].sharedMesh = targetMesh;
            }
            else
            {
                targetMesh.Clear();
                targetMesh.vertices = sourceMesh.vertices;
                targetMesh.normals = sourceMesh.normals;
                targetMesh.uv = sourceMesh.uv;
                targetMesh.triangles = sourceMesh.triangles;
            }
        }

        for (int i = 0; i < _modifications.Length; i++)
        {
           _modifications[i].ApplyModification(_meshFilters);
        }
    }

    internal void UpdateSphere()
    {
        if (_autoUpdate == true)
        {
            if (_sphere == null)
            {
                if (_originalMeshFilters != null && _originalMeshFilters[0] == null)
                {
                    _originalMeshFilters = null;
                    DestroyMesh();
                }
                Debug.Log("_sphere == null");
                return;
            }

            _originalMeshFilters = _sphere.GetMeshFilters();

            if (_originalMeshFilters == null ||  _originalMeshFilters.Length == 0) 
            {
                DestroyMesh();
                Debug.Log("_originalMeshFilters == null");
                return;
            }

            if (_meshFilters == null || _meshFilters.Length != _originalMeshFilters.Length)
                _meshFilters = new MeshFilter[_originalMeshFilters.Length];

            for (int i = 0; i < _meshFilters.Length; i++)
            {
                if (_meshFilters[i] == null)
                {
                    GameObject meshObject = new("_meshCopy");
                    meshObject.transform.parent = transform;
                    meshObject.transform.localPosition = Vector3.zero;

                    meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }

                _meshFilters[i].sharedMesh = Instantiate(_originalMeshFilters[i].sharedMesh);

                EditRendering(_originalMeshFilters, false);
            }
            Debug.Log("_meshCopy is created");


            ApplyModifications();
        }
    }

    internal void DestroyMesh()
    {
        if (_meshFilters == null) return;

        EditRendering(_originalMeshFilters, true);

        foreach (var filter in _meshFilters)
        {
            if (filter != null)
            {
                DestroyImmediate(filter.gameObject);
            }
        }

        _meshFilters = null;

        Debug.Log("Mesh is destroyed");
    }

    internal void EditRendering(MeshFilter[] meshFilters, bool value = true)
    {
        if (meshFilters == null) return;

        foreach (var filter in meshFilters)
        {
            if (filter != null)
            {
                var renderer = filter.GetComponent<MeshRenderer>();
                if (renderer != null)
                    renderer.enabled = value;
            }
        }       
    }

    internal bool CompareMeshFilters()
    {
        if (Sphere == null && _originalMeshFilters != null) return false;
        if (Sphere.GetMeshFilters() == null) return false;
        if (Sphere != null && _originalMeshFilters == null) return false;
        if (_originalMeshFilters == null) return true;
        return _originalMeshFilters.Length == Sphere.GetMeshFilters().Length;
    }
}
