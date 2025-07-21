using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple,
        Ridgid
    };
    [SerializeField] private FilterType filterType;

    [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHide("filterType", 1)]
    public RidgidNoiseSettings ridgidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        [SerializeField] protected float strength = 1;
        [Range(1, 8), SerializeField] protected int numberLayers = 1;
        [SerializeField] protected float baseRoughness = 1;
        [SerializeField] protected float roughness = 2;
        [SerializeField] protected float persistence = .5f;
        [SerializeField] protected Vector3 centre;
        [SerializeField] protected float minValue;
        public float GetStrength() { return strength; }
        public float GetRoughness() { return roughness; }
        public Vector3 GetCentre() { return centre; }
        public int GetNumberLayers() { return numberLayers; }
        public float GetPersistence() { return persistence; }
        public float GetBaseRoughness() { return baseRoughness; }
        public float GetMinValue() { return minValue; }
    }

    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        [SerializeField] private float weightMultiplier = .8f;

        public float GetWeightMultiplier() { return weightMultiplier; }
    }

    public FilterType GetFilterType() { return filterType; }  
}
