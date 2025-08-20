using UnityEditor;
using UnityEngine;
using System;
using SphereGenerator;
using System.Collections.Generic;

[CustomEditor(typeof(CelestialBodyTerrain))]
public class CelestialBodyTerrainEditor : Editor
{
    private CelestialBodyTerrain _terrain;
    private Editor[] _modificationEditors;
    private bool[] _modificationFoldouts;

    private bool _delayedGeneration = true;

    private void OnEnable()
    {
        _terrain = (CelestialBodyTerrain)target;
    }

    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();

        Sphere currentSphere = _terrain.GetComponent<Sphere>();

        if (currentSphere != null && _terrain.Sphere == null) 
        { 
            _terrain.Sphere = currentSphere;
        }

        Type selectedType = (currentSphere != null) ? currentSphere.GetType() : null;
        Type newType = SphereTypeUtility.DrawSphereTypeSelection(selectedType);

        if (newType != selectedType)
        {
            if (currentSphere != null)
            {
                DestroyImmediate(currentSphere);
            }

            if (newType != null)
            {
                _terrain.Sphere = (Sphere)_terrain.gameObject.AddComponent(newType);
            }
            EditorUtility.SetDirty(_terrain);
        }

        //EditorGUI.BeginChangeCheck();

        _terrain.AutoUpdate = EditorGUILayout.Toggle("Auto Update", _terrain.AutoUpdate);
        if (_terrain.AutoUpdate == false)
        {
            _delayedGeneration = true;

            if (GUILayout.Button("Generate"))
            {
                //EditorGUI.EndChangeCheck();
                _terrain.AutoUpdate = true;
                _terrain.UpdateSphere();
                _terrain.AutoUpdate = false;
            }  
        }
        else if (_terrain.AutoUpdate == true)
        {
            //if (EditorGUI.EndChangeCheck())
            //{
            //    if (_delayedGeneration == true)
            //    {
            //        _delayedGeneration = false;
            //    }
            //    Debug.Log("+++++++");
            //    _terrain.UpdateSphere();
            //}

            if (_delayedGeneration == true)
            {
                _delayedGeneration = false;
            }
            Debug.Log("+++++++");
            _terrain.UpdateSphere();
        }

        //if (_terrain.CompareMeshFilters() == false)
        //{
        //    Debug.Log("----------");
        //    _terrain.UpdateSphere();
        //}

        EditorGUILayout.Space();
        if (GUILayout.Button("Add Modification"))
        {
            AddModificationWindow.Open(_terrain);
        }

        EditorGUILayout.LabelField("Modifications", EditorStyles.boldLabel);

        int CountOfModifications = 0;
        if (_terrain.Modifications != null)
        {
            CountOfModifications = _terrain.Modifications.Length;
        }

        if (_modificationFoldouts == null || _modificationFoldouts.Length != CountOfModifications)
        {
            _modificationFoldouts = new bool[CountOfModifications];
        }

        if (_modificationEditors == null || _modificationEditors.Length != CountOfModifications)
        {
            _modificationEditors = new Editor[CountOfModifications];
        }

        for (int i = 0; i < CountOfModifications; i++)
        {
            if (_terrain.Modifications[i] != null)
            {
                DrawSettingsEditor(_terrain.Modifications[i], _terrain.UpdateSphere, ref _modificationFoldouts[i], ref _modificationEditors[i]);

                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    RemoveModification(i);
                    i--;
                    CountOfModifications--;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void RemoveModification(int index)
    {
        Undo.RecordObject(_terrain, "Remove Modification");

        var modifications = new List<Modification>(_terrain.Modifications);
        modifications.RemoveAt(index);
        _terrain.Modifications = modifications.ToArray();

        _terrain.UpdateSphere();
        EditorUtility.SetDirty(_terrain);
    }

    private void DrawSettingsEditor(UnityEngine.Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

        if (foldout)
        {
            if (editor == null) CreateCachedEditor(settings, null, ref editor);

            var serialized = editor.serializedObject;
            serialized.Update();

            EditorGUI.BeginChangeCheck();
            editor.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                serialized.ApplyModifiedProperties(); 
                onSettingsUpdated();
            }
            else
            {
                serialized.ApplyModifiedProperties(); 
            }
        }
    }

    private void OnDisable()
    {
        if (_terrain == null)
        {
            _terrain.DestroyMesh();
        }
    }
}


