using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeConstructor
{
    public static TerrainData GenerateMoistureLevels(TerrainData data, TerrainSettings settings)
    {
        bool allTilesEvaluated = false;
        // set all water tiles (rivers, coasts, oceans) to moisture value 5
        foreach (var tile in data.tiles)
        {
            if (tile.type.IsWaterTile())
            {
                tile.moistureValue = settings.maxMoistureLevel;
            }
        }
        // evaluate all tiles for moisture value
        while (!allTilesEvaluated)
        {
            Dictionary<TileData, int> tileMoistureValue = new Dictionary<TileData, int>(); // keeps a record of all tiles changed this iteration. Sets values of each tile at end
            foreach (var tile in data.tiles)
            {
                if (tile.moistureValue == 0)
                {
                    List<TileData> adjacentTiles = data.GetAllEdgeAdjacentTiles(tile.xCoordinate, tile.zCoordinate);
                    int highestAdjacentMoistureValue = 0;
                    bool moistureBlockedByMountain = false;
                    for (int i = 0; i < adjacentTiles.Count; i++)
                    {
                        if (adjacentTiles[i].moistureValue > highestAdjacentMoistureValue)
                        {
                            highestAdjacentMoistureValue = adjacentTiles[i].moistureValue;
                        }
                        // if next to a mountain and moisture travel is blocked 
                        if (adjacentTiles[i].type == TileType.Mountain)
                        {
                            Vector3 direction = (tile.Position - adjacentTiles[i].Position).normalized;
                            if (direction == settings.windDirection)
                            {
                                moistureBlockedByMountain = true;
                            }
                        }
                    }
                    if (highestAdjacentMoistureValue > 0)
                    {
                        if (moistureBlockedByMountain)
                        {
                            tileMoistureValue.Add(tile, Mathf.Clamp(highestAdjacentMoistureValue - 3, 1, settings.maxMoistureLevel));
                        }
                        else
                        {
                            tileMoistureValue.Add(tile, Mathf.Clamp(highestAdjacentMoistureValue - 1, 1, settings.maxMoistureLevel));
                        }
                    }
                }
            }
            // set all of the changed tiles to their respective moisture values
            foreach (var tileMoisture in tileMoistureValue)
            {
                tileMoisture.Key.moistureValue = tileMoisture.Value;
            }
            allTilesEvaluated = true;
            foreach (var tile in data.tiles)
            {
                if (tile.moistureValue == 0 && !tile.type.IsWaterTile())
                {
                    allTilesEvaluated = false;
                    break;
                }
            }
        }
        return data;
    }

    public static TerrainData GenerateTemperatureLevels(TerrainData data, TerrainSettings settings)
    {
        TileData equator = null;
        float closestDistance = float.MaxValue;
        foreach (var tile in data.tiles)
        {
            if (Vector3.Distance(tile.Position, settings.equatorPosition) < closestDistance)
            {
                equator = tile;
                closestDistance = Vector3.Distance(tile.Position, settings.equatorPosition);
            }
        }
        if (equator != null)
        {
            equator.temperatureValue = 1f;
            bool allTilesEvaluated = false;
            while (!allTilesEvaluated)
            {
                foreach (var tile in data.tiles)
                {
                    if (tile.temperatureValue == 0)
                    {
                        List<TileData> adjacentTiles = data.GetAllEdgeAdjacentTiles(tile.xCoordinate, tile.zCoordinate);
                        float highestTempValue = 0;
                        for (int i = 0; i < adjacentTiles.Count; i++)
                        {
                            if (adjacentTiles[i].temperatureValue > highestTempValue)
                            {
                                highestTempValue = adjacentTiles[i].temperatureValue;
                            }
                        }
                        if (highestTempValue != 0)
                        {
                            tile.temperatureValue = Mathf.Clamp(highestTempValue - settings.temperatureChangeRate, .001f, 1);
                        }
                    }
                }
                allTilesEvaluated = true;
                foreach (var tile in data.tiles)
                {
                    if (tile.temperatureValue == 0)
                    {
                        allTilesEvaluated = false;
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Equator = null");
        }
        return data;
    }
}
