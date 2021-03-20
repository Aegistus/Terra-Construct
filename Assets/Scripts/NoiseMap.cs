using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseMap
{
    public float noiseScale = 1;
    public int octaves = 3;
    [Tooltip("Controls increase in frequency of octaves")]
    [Range(0f, 1f)]
    public float lacunarity = 1;
    [Tooltip("Controls decrease in amplitude of octaves")]
    [Range(0f, 1f)]
    public float persistance = 1;

    public float XRandomOffset { get; set; }
    public float ZRandomOffset { get; set; }

    public float GetPerlinValueAtPosition(float x, float z)
    {
        float noiseAtPosition = 0;
        for (int o = 0; o < octaves; o++)
        {
            float frequency = Mathf.Pow(lacunarity, o);
            float amplitude = Mathf.Pow(persistance, o);
            noiseAtPosition += Mathf.PerlinNoise(x + XRandomOffset / noiseScale * frequency, z + ZRandomOffset / noiseScale * frequency) * amplitude;
        }
        return noiseAtPosition;
    }
}
