using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings settings;
    private Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = settings.GetBaseRoughness();
        float amplitude = 1;

        for (int i = 0; i < settings.GetNumberLayers(); i++)
        {
            float value = noise.Evaluate(point * frequency + settings.GetCentre());
            noiseValue += (value + 1) * .5f * amplitude;
            frequency *= settings.GetRoughness();
            amplitude *= settings.GetPersistence();
        }

        noiseValue = Mathf.Max(0, noiseValue - settings.GetMinValue());
        //noiseValue = noiseValue - settings.GetMinValue();
        return noiseValue * settings.GetStrength();
    }
}
