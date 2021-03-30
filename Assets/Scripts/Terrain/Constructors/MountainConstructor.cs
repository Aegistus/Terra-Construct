using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainConstructor
{
    public static TerrainData GenerateMountains(TerrainData data, TerrainSettings settings, MountainSet mountainSet)
    {
        List<TerrainObjectData> mountains = new List<TerrainObjectData>();
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                TileData tile = data.GetTileAtCoordinates(x, z);
                if (tile.noiseValue > settings.mountainLevel && tile.type == TileType.FlatLand)
                {
                    for (int i = 0; i < settings.maxMountainsPerTile; i++)
                    {
                        if (Random.value > settings.mountainClumping)
                        {
                            break;
                        }
                        float halfExtent = settings.tileSize / 2;
                        Vector3 randomPosition = new Vector3(Random.Range(-halfExtent, halfExtent), 0, Random.Range(-halfExtent, halfExtent));
                        if (data.IsOceanTile(x, z) || data.IsCoastalTile(x, z))
                        {
                            randomPosition.y = settings.seaMountainLevel;
                        }
                        int randIndex = Random.Range(0, mountainSet.mountains.Length);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        TerrainObjectData newMountain = new TerrainObjectData(randIndex, randomPosition + tile.Position, randomRotation, Vector3.one * Random.Range(settings.sizeVariationLower, settings.sizeVariationUpper));
                        
                        mountains.Add(newMountain);
                    }
                }
            }
        }
        data.mountains = mountains;
        return data;
    }

    public static TerrainData GenerateFoothills(TerrainData data, TerrainSettings settings, MountainSet mountainSet)
    {
        List<TerrainObjectData> foothills = new List<TerrainObjectData>();
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                TileData tile = data.GetTileAtCoordinates(x, z);
                if (tile.noiseValue > settings.foothillLevel)
                {
                    for (int i = 0; i < settings.maxFoothillsPerTile; i++)
                    {
                        if (Random.value > settings.mountainClumping)
                        {
                            break;
                        }
                        float halfExtent = settings.tileSize / 2;
                        Vector3 randomPosition = new Vector3(Random.Range(-halfExtent, halfExtent), 0, Random.Range(-halfExtent, halfExtent));
                        if (data.IsOceanTile(x, z) || data.IsCoastalTile(x, z))
                        {
                            randomPosition.y = settings.seaMountainLevel;
                        }
                        int randIndex = Random.Range(0, mountainSet.hills.Length);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        TerrainObjectData newFoothill = new TerrainObjectData(randIndex, randomPosition + tile.Position, randomRotation, Vector3.one * Random.Range(settings.sizeVariationLower, settings.sizeVariationUpper));

                        foothills.Add(newFoothill);
                    }
                }
            }
        }
        data.foothills = foothills;
        return data;
    }
}
