using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseMap
{
    public int xSize;
    public int zSize;
    public float noiseScale = 1;
    public int octaves = 3;
    public float lacunarity = 1;
    public float persistance = 1;

    public void SetSize(int xSize, int zSize)
    {
        this.xSize = xSize;
        this.zSize = zSize;
    }

    public float GetPerlinValueAtPosition(float x, float z)
    {
        float noiseAtPosition = 0;
        for (int o = 0; o < octaves; o++)
        {
            float frequency = Mathf.Pow(lacunarity, o);
            float amplitude = Mathf.Pow(persistance, o);
            noiseAtPosition += Mathf.PerlinNoise((x / noiseScale + amplitude) * frequency, (z / noiseScale + amplitude) * frequency);
        }
        return noiseAtPosition;
    }
}
