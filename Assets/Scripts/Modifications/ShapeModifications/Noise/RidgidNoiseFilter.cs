using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RidgidNoiseSettings settings;
    private Noise noise = new Noise();

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.GetBaseRoughness();
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.GetNumberLayers(); i++)
        {
            float value = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.GetCentre()));
            value *= value * weight;
            weight = Mathf.Clamp01(value * settings.GetWeightMultiplier());
            noiseValue += value * amplitude;
            frequency *= settings.GetRoughness();
            amplitude *= settings.GetPersistence();
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.GetMinValue());
        //noiseValue = noiseValue - settings.GetMinValue();
        return noiseValue * settings.GetStrength();
    }
}
