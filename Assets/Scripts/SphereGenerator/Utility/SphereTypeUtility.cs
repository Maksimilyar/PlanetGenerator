using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;



namespace SphereGenerator
{
    public static class SphereTypeUtility
    {
        public static Type DrawSphereTypeSelection(Type currentType)
        {
            List<Type> sphereTypes = GetAllSphereTypes();
            string[] typeNames = sphereTypes.Select(t => t.Name).ToArray();
            int currentIndex = sphereTypes.IndexOf(currentType);
            int newIndex = EditorGUILayout.Popup("Sphere Type", currentIndex, typeNames);

            return (newIndex >= 0) ? sphereTypes[newIndex] : null;
        }

        public static List<Type> GetAllSphereTypes()
        {
            return Assembly.GetAssembly(typeof(Sphere))
                           .GetTypes()
                           .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Sphere)))
                           .ToList();
        }

        public static Type GetFirstSphereType()
        {
            List<Type> types = GetAllSphereTypes();
            return types.Count > 0 ? types[0] : null;
        }
    }
}

