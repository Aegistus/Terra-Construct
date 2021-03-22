using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;
    public int numOfRivers = 3;
    public int maxRiverLength = 4;

    List<GameObject> riverMouths;
    float tileSize;

    public TerrainData ConstructRivers(TerrainData terrainData, float tileSize)
    {
        this.tileSize = tileSize;
        riverMouths = new List<GameObject>();

        // find all potential river mouths
        List<Vector2> potentialTiles = new List<Vector2>();
        for (int x = 0; x < terrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < terrainData.Tiles.GetLength(1); z++)
            {
                if (!terrainData.IsOceanTile(x,z) && terrainData.AdjacentOceanTilesCount(x,z) == 1)
                {
                    potentialTiles.Add(new Vector2(x, z));
                }
            }
        }
        if (numOfRivers < 0)
        {
            numOfRivers = 0;
        }
        // create a list of random candidates. length == numOfRivers
        while (potentialTiles.Count != numOfRivers)
        {
            potentialTiles.RemoveAt(Random.Range(0, potentialTiles.Count));
        }
        // create all rivers
        for (int i = 0; i < potentialTiles.Count; i++)
        {
            List<GameObject> oceanTiles = terrainData.GetEdgeAdjacentOceanTiles((int)potentialTiles[i].x, (int)potentialTiles[i].y);
            PlaceRiverMouthTile((int)potentialTiles[i].x, (int)potentialTiles[i].y, oceanTiles[0].transform.position, terrainData);
            BuildRiver((int)potentialTiles[i].x, (int)potentialTiles[i].y, terrainData);
        }

        return terrainData;
    }

    public void PlaceRiverMouthTile(int x, int z, Vector3 oceanTilePosition, TerrainData terrainData)
    {
        DestroyImmediate(terrainData.Tiles[x, z]);
        int randomTileIndex = Random.Range(0, tileSet.riverMouth.Length);
        GameObject newTile = Instantiate(tileSet.riverMouth[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        Vector3 direction = oceanTilePosition - newTile.transform.localPosition;
        direction = direction.normalized;
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
        terrainData.Tiles[x, z] = newTile;
        riverMouths.Add(newTile);
    }

    private void BuildRiver(int xStart, int zStart, TerrainData terrainData)
    {
        TerrainData.Coordinates currentTileCoords = new TerrainData.Coordinates(xStart, zStart);
        int totalRiverTiles = 0;
        // build river while river is still under max length
        while (totalRiverTiles < maxRiverLength)
        {
            List<GameObject> adjacentLand = terrainData.GetEdgeAdjacentLandTiles(currentTileCoords.x, currentTileCoords.z);
            if (adjacentLand.Count == 0)
            {
                break;
            }
            int randIndex = Random.Range(0, adjacentLand.Count);
            GameObject nextTile = adjacentLand[randIndex];
            currentTileCoords = terrainData.GetTileCoordinates(nextTile);
            terrainData.Tiles[currentTileCoords.x, currentTileCoords.z] = ReplaceWithRiverTile(nextTile);
            totalRiverTiles++;
        }
    }

    private GameObject ReplaceWithRiverTile(GameObject tile)
    {
        int randIndex = Random.Range(0, tileSet.riverStraight.Length);
        GameObject riverTile = Instantiate(tileSet.riverStraight[randIndex], tile.transform.position, Quaternion.identity, transform);
        DestroyImmediate(tile);
        return riverTile;
    }
}
