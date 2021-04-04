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

        return data;
    }
}
