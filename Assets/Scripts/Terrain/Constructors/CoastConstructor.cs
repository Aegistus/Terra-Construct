using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TerrainConstructor))]
public class CoastConstructor : MonoBehaviour, IConstructor
{
    public TerrainTileSet tileSet;

    private float tileSize;
    private TerrainConstructor constructor;
    private TerrainData TerrainData => constructor.terrainData;

    public void Construct()
    {
        constructor = GetComponent<TerrainConstructor>();
        tileSize = constructor.tileSize;
        // first get rid of islands/peninsulas
        RemoveIsolatedOceanTiles();
        RemoveIslandsAndPeninsulas();
        for (int x = 0; x < TerrainData.xSize; x++)
        {
            for (int z = 0; z < TerrainData.zSize; z++)
            {
                List<TileData> edgeAdjacentOcean = TerrainData.GetEdgeAdjacentOceanTiles(x, z);
                if (!TerrainData.IsOceanTile(x, z) && edgeAdjacentOcean.Count > 0)
                {
                    if (edgeAdjacentOcean.Count == 1) // coastal straight
                    {
                        Vector3 direction = edgeAdjacentOcean[0].position - TerrainData.GetTileAtCoordinates(x, z).position;
                        direction = direction.normalized;
                        Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
                        TileData tile = TerrainData.GetTileAtCoordinates(x, z);
                        tile.ReplaceTile(TileType.CoastStraight, position, Vector3.zero);
                        if (direction == -transform.forward)
                        {
                            tile.Rotate(0, 180, 0);
                            tile.position += new Vector3(tileSize, 0, tileSize);
                        }
                        else if (direction == transform.right)
                        {
                            tile.Rotate(0, 90, 0);
                            tile.position += new Vector3(0, 0, tileSize);
                        }
                        else if (direction == -transform.right)
                        {
                            tile.Rotate(0, 270, 0);
                            tile.position += new Vector3(tileSize, 0, 0);
                        }
                    }
                    else if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].position - TerrainData.GetTileAtCoordinates(x, z).position;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].position - TerrainData.GetTileAtCoordinates(x, z).position;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) <= 90) // coastal outer corner
                        {
                            Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
                            TileData tile = TerrainData.GetTileAtCoordinates(x, z);
                            tile.ReplaceTile(TileType.CoastOuterCorner, position, Vector3.zero);
                            if (directionOne == transform.forward && directionTwo == -transform.right || directionTwo == transform.forward && directionOne == -transform.right)
                            {
                                tile.Rotate(0, -90, 0);
                                tile.position += new Vector3(tileSize, 0, 0);
                            }
                            else if (directionOne == transform.right && directionTwo == -transform.forward || directionTwo == transform.right && directionOne == -transform.forward)
                            {
                                tile.Rotate(0, 90, 0);
                                tile.position += new Vector3(0, 0, tileSize);
                            }
                            else if (directionOne == -transform.forward && directionTwo == -transform.right || directionTwo == -transform.forward && directionOne == -transform.right)
                            {
                                tile.Rotate(0, 180, 0);
                                tile.position += new Vector3(tileSize, 0, tileSize);
                            }
                        }
                    }
                }
                List<TileData> cornerAdjacentOcean = TerrainData.GetCornerAdjacentOceanTiles(x, z);
                if (cornerAdjacentOcean.Count == 1 && edgeAdjacentOcean.Count == 0) // coastal inner corner
                {
                    Vector3 direction = cornerAdjacentOcean[0].position - TerrainData.GetTileAtCoordinates(x, z).position;
                    direction = direction.normalized;
                    Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
                    TileData tile = TerrainData.GetTileAtCoordinates(x, z);
                    tile.ReplaceTile(TileType.CoastInnerCorner, position, Vector3.zero);
                    float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
                    if (angle > 90 && angle <= 180)
                    {
                        tile.Rotate(0, -90, 0);
                        tile.position += new Vector3(tileSize, 0, 0);
                    }
                    else if (angle < -90 && angle >= -180)
                    {
                        tile.Rotate(0, 180, 0);
                        tile.position += new Vector3(tileSize, 0, tileSize);
                    }
                    else if (angle < 0 && angle >= -90)
                    {
                        tile.Rotate(0, 90, 0);
                        tile.position += new Vector3(0, 0, tileSize);
                    }
                }
            }
        }
        // do a final check for islands and peninsulas
        RemoveIslandsAndPeninsulas();
    }

    private void RemoveIsolatedOceanTiles()
    {
        for (int x = 0; x < TerrainData.xSize; x++)
        {
            for (int z = 0; z < TerrainData.zSize; z++)
            {
                if (!TerrainData.IsOceanTile(x,z) && TerrainData.AdjacentOceanTilesCount(x,z) == 4)
                {
                    Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
                    TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(TileType.FlatLand, position, Vector3.zero);
                }
            }
        }
    }

    private void RemoveIslandsAndPeninsulas()
    {
        for (int x = 0; x < TerrainData.xSize; x++)
        {
            for (int z = 0; z < TerrainData.zSize; z++)
            {
                if (!TerrainData.IsOceanTile(x,z))
                {
                    List<TileData> edgeAdjacentOcean = TerrainData.GetEdgeAdjacentOceanTiles(x, z);
                    if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].position - TerrainData.GetTileAtCoordinates(x, z).position;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].position - TerrainData.GetTileAtCoordinates(x, z).position;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) > 90)
                        {
                            TurnIntoOceanTile(x, z);
                        }
                    }
                    else if (edgeAdjacentOcean.Count > 2)
                    {
                        TurnIntoOceanTile(x, z);
                    }
                }
            }
        }
    }

    private void TurnIntoOceanTile(int x, int z)
    {
        Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
        TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(TileType.OceanFloor, position, Vector3.zero);
    }
}
