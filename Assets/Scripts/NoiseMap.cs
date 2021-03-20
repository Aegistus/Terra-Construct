using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseMap
{
    public float scale = 1;
    public int octaves = 3;
    [Tooltip("Controls increase in frequency of octaves")]
    public float lacunarity = 1;
    [Tooltip("Controls decrease in amplitude of octaves")]
    public float persistance = 1;

    public float XRandomOffset { get; set; }
    public float ZRandomOffset { get; set; }

    // Reference for clamping the return between 0 and 1
    private float maxValue = 0;
    private float minValue = 0;

    public float GetLayeredPerlinValueAtPosition(float x, float z)
    {
        float noiseAtPosition = 0;
        float frequency = 1;
        float amplitude = 1;
        for (int o = 0; o < octaves; o++)
        {
            noiseAtPosition += Mathf.PerlinNoise(x + XRandomOffset / scale * frequency, z + ZRandomOffset / scale * frequency) * amplitude;
            frequency *= lacunarity;
            amplitude *= persistance;
        }
        noiseAtPosition = Mathf.InverseLerp(minValue, maxValue, noiseAtPosition);
        return noiseAtPosition;
    }

    public void ResetNoiseRange()
    {
        maxValue = 0;
        minValue = 0;
        for (int o = 0; o < octaves; o++)
        {
            float amplitude = Mathf.Pow(persistance, o);
            maxValue += 1 * amplitude;
        }
        Debug.Log("Max Value: " + maxValue);
        for (int o = 0; o < octaves; o++)
        {
            float amplitude = Mathf.Pow(persistance, o);
            minValue += .01f * amplitude;
        }
        Debug.Log("Min Value: " + minValue);
    }
}
