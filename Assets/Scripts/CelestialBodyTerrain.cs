using SphereGenerator;
using UnityEngine;

public class CelestialBodyTerrain : MonoBehaviour
{
    [Range(2, 256)]
    private int _resolution = 2;

    private Sphere _sphere;

    private MeshFilter[] _meshFilters;

    private Modification[] _modifications;

    private bool _autoUpdate = true;
    

    public int Resolution
    {
        get => _resolution;
        internal set
        {
            if (_resolution != value)
            {
                _resolution = value;
                if (_sphere != null)
                {
                    _sphere.GenerateSphere(_resolution, _autoUpdate);
                    UpdateSphere();
                }
            }
        }
    }

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

        for (int i = 0; i < _modifications.Length; i++)
        {
           _modifications[i].ApplyModification(_meshFilters);;
        }
    }

    internal void UpdateSphere()
    {
        if (_autoUpdate == true)
        {
            if (_sphere == null) return;
            //_sphere.GenerateSphere();

            _meshFilters = _sphere.GetMeshFilters();
            ApplyModifications();
        }
    }

    internal void RegenerateSphere()
    {
        if (_sphere != null)
        {
            _sphere.GenerateSphere(_resolution);
        }
    }
}
