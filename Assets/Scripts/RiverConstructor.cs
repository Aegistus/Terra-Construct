using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;
    public int numOfRivers = 3;

    GameObject[,] modifiedTiles;
    List<GameObject> riverMouths;
    float tileSize;

    public TerrainData ConstructRivers(TerrainData terrainData, float tileSize)
    {
        modifiedTiles = terrainData.Tiles;
        this.tileSize = tileSize;
        riverMouths = new List<GameObject>();

        // find all potential river mouths
        List<Vector2> potentialTiles = new List<Vector2>();
        for (int x = 0; x < modifiedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < modifiedTiles.GetLength(1); z++)
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
        // create the mouths of all rivers
        for (int i = 0; i < potentialTiles.Count; i++)
        {
            List<GameObject> oceanTiles = terrainData.GetEdgeAdjacentOceanTiles((int)potentialTiles[i].x, (int)potentialTiles[i].y);
            PlaceRiverMouthTile((int)potentialTiles[i].x, (int)potentialTiles[i].y, oceanTiles[0].transform.position);
        }

        return terrainData;
    }

    public void PlaceRiverMouthTile(int x, int z, Vector3 oceanTileLocalPosition)
    {
        DestroyImmediate(modifiedTiles[x, z]);
        int randomTileIndex = Random.Range(0, tileSet.riverMouth.Length);
        GameObject newTile = Instantiate(tileSet.riverMouth[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        Vector3 direction = oceanTileLocalPosition - newTile.transform.localPosition;
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
        modifiedTiles[x, z] = newTile;
        riverMouths.Add(newTile);
    }
}
