using UnityEngine;

public abstract class Modification : ScriptableObject
{
    public abstract void ApplyModification(MeshFilter[] meshFilters);
}
