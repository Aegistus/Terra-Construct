using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int xSize, int zSize, float scale, int octaves, float lacunarity, float persistance)
    {
        float[,] elevationNoiseMap = new float[xSize, zSize];
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                float noisePoint = 0;
                for (int o = 0; o < octaves; o++)
                {
                    float frequency = Mathf.Pow(lacunarity, o);
                    float amplitude = Mathf.Pow(persistance, o);
                    noisePoint += Mathf.PerlinNoise((x / scale + amplitude) * frequency, (z / scale + amplitude) * frequency);
                }
                elevationNoiseMap[x, z] = noisePoint;
            }
        }
        return elevationNoiseMap;
    }
}
