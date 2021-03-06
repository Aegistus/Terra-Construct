using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainConstructor
{
    public static List<TerrainObjectData> Generate(TileData tile, TerrainSettings settings, GameObject[] variants)
    {
        List<TerrainObjectData> mountains = new List<TerrainObjectData>();
        for (int i = 0; i < settings.maxMountainsPerTile; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0, settings.tileSize), 0, Random.Range(0, settings.tileSize));
            if (tile.type.IsWaterTile())
            {
                randomPosition.y = settings.seaMountainLevel;
            }
            int randIndex = Random.Range(0, variants.Length);
            Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
            TerrainObjectData newMountain = new TerrainObjectData(randIndex, randomPosition + tile.Position, randomRotation, Vector3.one * Random.Range(settings.sizeVariationLower, settings.sizeVariationUpper));

            mountains.Add(newMountain);
        }
        return mountains;
    }

    public static List<TerrainObjectData> GenerateScatteredBoulders(TileData tile, TerrainSettings settings, GameObject[] variants)
    {
        List<TerrainObjectData> boulders = new List<TerrainObjectData>();
        if (!tile.type.IsRiverTile() && !tile.type.IsOceanTile())
        {
            for (int i = 0; i < settings.maxMountainsPerTile; i++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(0, settings.tileSize), 0, Random.Range(0, settings.tileSize));
                if (tile.type.IsCoastalTile())
                {
                    randomPosition.y = settings.seaMountainLevel;
                }
                int randIndex = Random.Range(0, variants.Length);
                Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                TerrainObjectData newMountain = new TerrainObjectData(randIndex, randomPosition + tile.Position, randomRotation, Vector3.one * Random.Range(settings.sizeVariationLower, settings.sizeVariationUpper));

                boulders.Add(newMountain);
            }
        }
        return boulders;
    }
}
