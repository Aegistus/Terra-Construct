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
                        Vector3 direction = edgeAdjacentOcean[0].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
                        direction = direction.normalized;
                        int randomIndex = Random.Range(0, tileSet.coastalStraight.Length);
                        GameObject newTile = Instantiate(tileSet.coastalStraight[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                        TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.CoastStraight);
                        if (direction == -transform.forward)
                        {
                            newTile.transform.Rotate(0, 180, 0);
                            newTile.transform.localPosition += new Vector3(tileSize, 0, tileSize);
                        }
                        else if (direction == transform.right)
                        {
                            newTile.transform.Rotate(0, 90, 0);
                            newTile.transform.localPosition += new Vector3(0, 0, tileSize);
                        }
                        else if (direction == -transform.right)
                        {
                            newTile.transform.Rotate(0, 270, 0);
                            newTile.transform.localPosition += new Vector3(tileSize, 0, 0);
                        }
                    }
                    else if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) <= 90) // coastal outer corner
                        {
                            int randomIndex = Random.Range(0, tileSet.coastalOuterCorner.Length);
                            GameObject newTile = Instantiate(tileSet.coastalOuterCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                            TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.CoastOuterCorner);
                            if (directionOne == transform.forward && directionTwo == -transform.right || directionTwo == transform.forward && directionOne == -transform.right)
                            {
                                newTile.transform.Rotate(0, -90, 0);
                                newTile.transform.localPosition += new Vector3(tileSize, 0, 0);
                            }
                            else if (directionOne == transform.right && directionTwo == -transform.forward || directionTwo == transform.right && directionOne == -transform.forward)
                            {
                                newTile.transform.Rotate(0, 90, 0);
                                newTile.transform.localPosition += new Vector3(0, 0, tileSize);
                            }
                            else if (directionOne == -transform.forward && directionTwo == -transform.right || directionTwo == -transform.forward && directionOne == -transform.right)
                            {
                                newTile.transform.Rotate(0, 180, 0);
                                newTile.transform.localPosition += new Vector3(tileSize, 0, tileSize);
                            }
                        }
                    }
                }
                List<TileData> cornerAdjacentOcean = TerrainData.GetCornerAdjacentOceanTiles(x, z);
                if (cornerAdjacentOcean.Count == 1 && edgeAdjacentOcean.Count == 0) // coastal inner corner
                {
                    Vector3 direction = cornerAdjacentOcean[0].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
                    direction = direction.normalized;
                    int randomIndex = Random.Range(0, tileSet.coastalInnerCorner.Length);
                    GameObject newTile = Instantiate(tileSet.coastalInnerCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.CoastInnerCorner);
                    float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
                    if (angle > 90 && angle <= 180)
                    {
                        newTile.transform.Rotate(0, -90, 0);
                        newTile.transform.localPosition += new Vector3(tileSize, 0, 0);
                    }
                    else if (angle < -90 && angle >= -180)
                    {
                        newTile.transform.Rotate(0, 180, 0);
                        newTile.transform.localPosition += new Vector3(tileSize, 0, tileSize);
                    }
                    else if (angle < 0 && angle >= -90)
                    {
                        newTile.transform.Rotate(0, 90, 0);
                        newTile.transform.localPosition += new Vector3(0, 0, tileSize);
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
                    int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
                    GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.FlatLand);
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
                        Vector3 directionOne = edgeAdjacentOcean[0].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].Transform.localPosition - TerrainData.GetTileAtCoordinates(x, z).Transform.localPosition;
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
        int randomTileIndex = Random.Range(0, tileSet.oceanFloorTiles.Length);
        GameObject newTile = Instantiate(tileSet.oceanFloorTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.OceanFloor);
    }
}
