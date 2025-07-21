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

            //if (TrySyncResolutionWithTerrain())
            //{
            //    GUILayout.Label("Resolution is managed by CelestialBodyTerrain", EditorStyles.boldLabel);
            //}
            //else
            //{
            //    using var check = new EditorGUI.ChangeCheckScope();
            //    EditorGUILayout.PropertyField(resolutionProp);
            //    if (check.changed)
            //    {
            //        sphere.GenerateSphere(sphere.GetResolution());
            //    }
            //}

            using var check = new EditorGUI.ChangeCheckScope();
            EditorGUILayout.PropertyField(resolutionProp);
            if (check.changed)
            {
                sphere.GenerateSphere();
            }

            serializedObject.ApplyModifiedProperties();
        }

        //private bool TrySyncResolutionWithTerrain()
        //{
        //    var terrain = sphere.GetComponent("CelestialBodyTerrain");
        //    if (terrain == null) return false;

        //    var resolutionField = terrain.GetType().GetField("_resolution", BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (resolutionField == null) return false;

        //    int sphereResolution = resolutionProp.intValue;
        //    int terrainResolution = (int)resolutionField.GetValue(terrain);

        //    if (terrainResolution != sphereResolution)
        //    {
        //        resolutionField.SetValue(terrain, sphereResolution);
        //    }

        //    return true;
        //}

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