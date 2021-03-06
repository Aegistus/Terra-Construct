using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoastConstructor
{

    public static TerrainData GenerateCoasts(TerrainData data, TerrainSettings settings)
    {
        // first get rid of islands/peninsulas
        data = RemoveIsolatedOceanTiles(data, settings);
        data = RemoveIslandsAndPeninsulas(data, settings);
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                List<TileData> edgeAdjacentOcean = data.GetEdgeAdjacentTilesOfType(x, z, TileType.OceanFloor);
                if (!data.IsOceanTile(x, z) && edgeAdjacentOcean.Count > 0)
                {
                    if (edgeAdjacentOcean.Count == 1) // coastal straight
                    {
                        Vector3 direction = edgeAdjacentOcean[0].Position - data.GetTileAtCoordinates(x, z).Position;
                        direction = direction.normalized;
                        Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                        TileData tile = data.GetTileAtCoordinates(x, z);
                        tile.ReplaceTile(TileType.CoastStraight, position, Vector3.zero);
                        if (direction == -Vector3.forward)
                        {
                            tile.Rotate(0, 180, 0);
                            tile.AddPosition(new Vector3(settings.tileSize, 0, settings.tileSize));
                        }
                        else if (direction == Vector3.right)
                        {
                            tile.Rotate(0, 90, 0);
                            tile.AddPosition(new Vector3(0, 0, settings.tileSize));
                        }
                        else if (direction == -Vector3.right)
                        {
                            tile.Rotate(0, 270, 0);
                            tile.AddPosition(new Vector3(settings.tileSize, 0, 0));
                        }
                    }
                    else if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].Position - data.GetTileAtCoordinates(x, z).Position;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].Position - data.GetTileAtCoordinates(x, z).Position;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) <= 90) // coastal outer corner
                        {
                            Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                            TileData tile = data.GetTileAtCoordinates(x, z);
                            tile.ReplaceTile(TileType.CoastOuterCorner, position, Vector3.zero);
                            if (directionOne == Vector3.forward && directionTwo == -Vector3.right || directionTwo == Vector3.forward && directionOne == -Vector3.right)
                            {
                                tile.Rotate(0, -90, 0);
                                tile.AddPosition(new Vector3(settings.tileSize, 0, 0));
                            }
                            else if (directionOne == Vector3.right && directionTwo == -Vector3.forward || directionTwo == Vector3.right && directionOne == -Vector3.forward)
                            {
                                tile.Rotate(0, 90, 0);
                                tile.AddPosition(new Vector3(0, 0, settings.tileSize));
                            }
                            else if (directionOne == -Vector3.forward && directionTwo == -Vector3.right || directionTwo == -Vector3.forward && directionOne == -Vector3.right)
                            {
                                tile.Rotate(0, 180, 0);
                                tile.AddPosition(new Vector3(settings.tileSize, 0, settings.tileSize));
                            }
                        }
                    }
                }
                List<TileData> cornerAdjacentOcean = data.GetCornerAdjacentTilesOfType(x, z, TileType.OceanFloor);
                if (cornerAdjacentOcean.Count == 1 && edgeAdjacentOcean.Count == 0) // coastal inner corner
                {
                    Vector3 direction = cornerAdjacentOcean[0].Position - data.GetTileAtCoordinates(x, z).Position;
                    direction = direction.normalized;
                    Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                    TileData tile = data.GetTileAtCoordinates(x, z);
                    tile.ReplaceTile(TileType.CoastInnerCorner, position, Vector3.zero);
                    float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);
                    if (angle > 90 && angle <= 180)
                    {
                        tile.Rotate(0, -90, 0);
                        tile.AddPosition(new Vector3(settings.tileSize, 0, 0));
                    }
                    else if (angle < -90 && angle >= -180)
                    {
                        tile.Rotate(0, 180, 0);
                        tile.AddPosition(new Vector3(settings.tileSize, 0, settings.tileSize));
                    }
                    else if (angle < 0 && angle >= -90)
                    {
                        tile.Rotate(0, 90, 0);
                        tile.AddPosition(new Vector3(0, 0, settings.tileSize));
                    }
                }
            }
        }
        // do a final check for islands and peninsulas
        data = RemoveIslandsAndPeninsulas(data, settings);
        return data;
    }

    private static TerrainData RemoveIsolatedOceanTiles(TerrainData data, TerrainSettings settings)
    {
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                if (!data.IsOceanTile(x,z) && data.AdjacentOceanTilesCount(x,z) == 4)
                {
                    Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                    data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.Plains, position, Vector3.zero);
                }
            }
        }
        return data;
    }

    private static TerrainData RemoveIslandsAndPeninsulas(TerrainData data, TerrainSettings settings)
    {
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                if (!data.IsOceanTile(x,z))
                {
                    List<TileData> edgeAdjacentOcean = data.GetEdgeAdjacentTilesOfType(x, z, TileType.OceanFloor);
                    if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].Position - data.GetTileAtCoordinates(x, z).Position;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].Position - data.GetTileAtCoordinates(x, z).Position;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) > 90)
                        {
                            Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                            data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.OceanFloor, position, Vector3.zero);
                        }
                    }
                    else if (edgeAdjacentOcean.Count > 2)
                    {
                        Vector3 position = new Vector3(x * settings.tileSize, 0, z * settings.tileSize);
                        data.GetTileAtCoordinates(x, z).ReplaceTile(TileType.OceanFloor, position, Vector3.zero);
                    }
                }
            }
        }
        return data;
    }
}
