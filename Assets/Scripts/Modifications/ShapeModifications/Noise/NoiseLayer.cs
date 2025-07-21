using UnityEngine;

[System.Serializable]
public class NoiseLayer
{
    [SerializeField] private bool enabled = true;
    [SerializeField] private bool useFirstLayerAsMask;
    [SerializeField] private NoiseSettings noiseSettings;

    public NoiseSettings GetNoiseSettings() { return noiseSettings; }
    public bool GetEnabled() { return enabled; }
    public bool GetUseFirstLayerAsMask() { return useFirstLayerAsMask; }
}
