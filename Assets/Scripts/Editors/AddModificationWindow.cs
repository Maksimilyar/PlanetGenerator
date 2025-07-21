using System;
using UnityEditor;
using UnityEngine;

public class AddModificationWindow : EditorWindow
{
    private CelestialBodyTerrain terrain;
    private Modification selectedModification;

    public static void Open(CelestialBodyTerrain terrain)
    {
        AddModificationWindow window = GetWindow<AddModificationWindow>("Add Modification");
        window.terrain = terrain;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Modification", EditorStyles.boldLabel);

        selectedModification = (Modification)EditorGUILayout.ObjectField(
            "Modification",
            selectedModification,
            typeof(Modification),
            false
        );

        if (selectedModification != null && GUILayout.Button("Add"))
        {
            AddModification();
            Close();
        }
    }

    private void AddModification()
    {
        Undo.RecordObject(terrain, "Add Modification");
        
        Modification[] currentMods;
        if (terrain.Modifications != null)
        {
            currentMods = terrain.Modifications;
        }
        else
        {
            currentMods = new Modification[0];
        }
        Modification[] newMods = new Modification[currentMods.Length + 1];

        Array.Copy(currentMods, newMods, currentMods.Length);
        newMods[newMods.Length - 1] = selectedModification;

        terrain.Modifications = newMods;

        EditorUtility.SetDirty(terrain);
    }
}
