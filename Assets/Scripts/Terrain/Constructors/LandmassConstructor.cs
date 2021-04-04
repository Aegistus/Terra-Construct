using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LandmassConstructor
{
    public static TerrainData GenerateLandmasses(TerrainData data, TerrainSettings settings, NoiseMap noiseMap)
    {
        noiseMap.XRandomOffset = Random.Range(0, 10000);
        noiseMap.ZRandomOffset = Random.Range(0, 10000);
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                float noiseValue = noiseMap.GetLayeredPerlinValueAtPosition(x, z);
                if (noiseValue < settings.oceanPercent)
                {
                    data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.OceanFloor, position, Vector3.zero);
                    data.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
                }
                else if (Random.value < settings.swampTileChance)
                {
                    data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.Swamp, position, Vector3.zero);
                    data.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
                }
                else
                {
                    data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.FlatLand, position, Vector3.zero);
                    data.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
                }
            }
        }
        return data;
    }

    //public void ClearTerrain()
    //{
    //    while (transform.childCount > 0)
    //    {
    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            DestroyImmediate(transform.GetChild(i).gameObject);
    //        }
    //    }
    //    elevationNoiseMap.ResetNoiseRange();
    //}
}
