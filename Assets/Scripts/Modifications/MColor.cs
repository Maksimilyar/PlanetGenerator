using UnityEngine;

[CreateAssetMenu(menuName = "Planet Generator/Modifications/Color Green")]
public class MColor : Modification
{
    public override void ApplyModification(MeshFilter[] meshFilters)
    {
        foreach (var meshFilter in meshFilters)
        {
            var renderer = meshFilter.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial.color = Color.green;
            }
        }
    }
}
