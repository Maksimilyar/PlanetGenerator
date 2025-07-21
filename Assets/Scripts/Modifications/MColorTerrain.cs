using UnityEngine;

[CreateAssetMenu(menuName = "Modifications/ColorTerrain")]
public class MColorTerrain : Modification
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Material _vertexColorMaterial;

    public override void ApplyModification(MeshFilter[] meshFilters)
    {
        // ПЕРШИЙ ПРОХІД: знайти глобальні min і max
        float globalMinR = float.MaxValue;
        float globalMaxR = float.MinValue;

        foreach (var filter in meshFilters)
        {
            // Центр планети (в світових координатах)
            Vector3 center = filter.transform.position;



            Mesh mesh = filter.sharedMesh;
            if (mesh == null) continue;

            Vector3[] vertices = mesh.vertices;
            Transform tf = filter.transform;

            foreach (var vertex in vertices)
            {
                Vector3 worldVertex = tf.TransformPoint(vertex);
                float r = (worldVertex - center).magnitude;

                if (r < globalMinR) globalMinR = r;
                if (r > globalMaxR) globalMaxR = r;
            }
        }

        float radiusRange = globalMaxR - globalMinR;
        if (radiusRange == 0f) radiusRange = 1f; // захист від ділення на нуль

        // ДРУГИЙ ПРОХІД: фарбування вершин
        foreach (var filter in meshFilters)
        {
            Mesh mesh = filter.sharedMesh;
            if (mesh == null) continue;

            Vector3[] vertices = mesh.vertices;
            Color[] colors = new Color[vertices.Length];
            Transform tf = filter.transform;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 worldVertex = tf.TransformPoint(vertices[i]);
                float r = worldVertex.magnitude;
                float t = (r - globalMinR) / radiusRange;
                colors[i] = _gradient.Evaluate(t);
            }

            mesh.colors = colors;

            // Призначення матеріалу
            if (_vertexColorMaterial != null)
            {
                filter.GetComponent<Renderer>().sharedMaterial = _vertexColorMaterial;
            }
        }
    }
}
