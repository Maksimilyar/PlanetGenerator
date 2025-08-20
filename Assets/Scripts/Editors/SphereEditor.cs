using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace SphereGenerator
{
    [CustomEditor(typeof(Sphere), true)]
    public class SphereEditor : Editor
    {
        private Sphere sphere;
        private SerializedProperty resolutionProp;

        public override void OnInspectorGUI()
        {
            sphere = (Sphere)target;
            resolutionProp = serializedObject.FindProperty("_resolution");

            using var check = new EditorGUI.ChangeCheckScope();
            EditorGUILayout.PropertyField(resolutionProp);
            if (check.changed)
            {
                sphere.GenerateSphere();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            sphere = (Sphere)target;
            if (sphere.GetMeshFilters() == null)
                sphere.GenerateSphere();
        }

        private void OnDisable()
        {
            if (sphere == null)
            {
                sphere.DestroyMesh();
            }
        }
    }
}