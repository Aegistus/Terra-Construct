using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;
    public int numOfRivers = 3;
    public int maxRiverLength = 4;

    TerrainConstructor constructor;
    TerrainData TerrainData => constructor.terrainData;
    List<GameObject> riverMouths;
    float tileSize;

    public void ConstructRivers(float tileSize)
    {
        constructor = GetComponent<TerrainConstructor>();
        this.tileSize = tileSize;
        riverMouths = new List<GameObject>();

        // find all potential river mouths
        List<Coordinates> potentialTiles = new List<Coordinates>();
        for (int x = 0; x < TerrainData.xSize; x++)
        {
            for (int z = 0; z < TerrainData.zSize; z++)
            {
                if (!TerrainData.IsOceanTile(x,z) && TerrainData.AdjacentOceanTilesCount(x,z) == 1)
                {
                    potentialTiles.Add(new Coordinates(x, z));
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
            List<TileData> oceanTiles = TerrainData.GetEdgeAdjacentOceanTiles(potentialTiles[i].x, potentialTiles[i].z);
            PlaceRiverMouthTile(potentialTiles[i].x, potentialTiles[i].z, oceanTiles[0].position);
            List<TileData> riverPath = new List<TileData>();
            riverPath.Add(TerrainData.GetTileAtCoordinates(potentialTiles[i])); // add the river mouth to the start of the path
            for (int j = 0; j < maxRiverLength; j++)
            {
                riverPath.Add(FindNextTileInRiverPath(riverPath));
            }
            ConstructRiverFromPath(riverPath);
        }
    }

    private TileData FindNextTileInRiverPath(List<TileData> riverPath)
    {
        Coordinates currentCoords = TerrainData.GetTileCoordinates(riverPath[riverPath.Count - 1]);
        List<TileData> adjacentLandTiles = TerrainData.GetEdgeAdjacentLandTiles(currentCoords.x, currentCoords.z);
        adjacentLandTiles.Remove(riverPath[riverPath.Count - 1]); // removes the current tile to avoid backtracking
        int randIndex = Random.Range(0, adjacentLandTiles.Count - 1);
        return adjacentLandTiles[randIndex];
    }

    private void ConstructRiverFromPath(List<TileData> riverPath)
    {
        for (int i = 1; i < riverPath.Count; i++)
        {
            if (riverPath[i] != null && i + 1 < riverPath.Count)
            {
                Vector3 directionFrom = riverPath[i - 1].position - riverPath[i].position;
                directionFrom = directionFrom.normalized;
                Vector3 directionTo = riverPath[i + 1].position - riverPath[i].position;
                directionTo = directionTo.normalized;
                if (Vector3.Angle(directionFrom, directionTo) == 180) // straight
                {
                    int randIndex = Random.Range(0, tileSet.riverStraight.Length);
                    GameObject newTile = Instantiate(tileSet.riverStraight[randIndex], riverPath[i].position, Quaternion.identity, transform);
                    riverPath[i].ReplaceTile(TileType.RiverStraight, Vector3.zero, Vector3.zero); // temporary
                    if (directionFrom == -transform.right)
                    {
                        newTile.transform.Rotate(0, -90, 0);
                        newTile.transform.localPosition += new Vector3(tileSize, 0, 0);
                    }
                    else if (directionFrom == transform.right)
                    {
                        newTile.transform.Rotate(0, 90, 0);
                        newTile.transform.localPosition += new Vector3(0, 0, tileSize);
                    }
                }
                else if (Vector3.SignedAngle(directionFrom, directionTo, Vector3.up) == 90) // right bend
                {
                    int randIndex = Random.Range(0, tileSet.riverCornerRight.Length);
                    GameObject newTile = Instantiate(tileSet.riverCornerRight[randIndex], riverPath[i].position, Quaternion.identity, transform);
                    riverPath[i].ReplaceTile(TileType.RiverBendRight, Vector3.zero, Vector3.zero); // temporary
                    if (directionFrom == transform.forward && directionTo == -transform.right || directionTo == transform.forward && directionFrom == -transform.right)
                    {
                        newTile.transform.Rotate(0, -90, 0);
                        newTile.transform.localPosition += new Vector3(tileSize, 0, 0);
                    }
                    else if (directionFrom == transform.right && directionTo == -transform.forward || directionTo == transform.right && directionFrom == -transform.forward)
                    {
                        newTile.transform.Rotate(0, 90, 0);
                        newTile.transform.localPosition += new Vector3(0, 0, tileSize);
                    }
                    else if (directionFrom == -transform.forward && directionTo == -transform.right || directionTo == -transform.forward && directionFrom == -transform.right)
                    {
                        newTile.transform.Rotate(0, 180, 0);
                        newTile.transform.localPosition += new Vector3(tileSize, 0, tileSize);
                    }
                }
            }
            else
            {
                // place river end tile
            }
        }
    }

    public void PlaceRiverMouthTile(int x, int z, Vector3 oceanTilePosition)
    {
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
        //TerrainData.GetTileAtCoordinates(x, z).ReplaceTile(TileType.RiverMouth);
        riverMouths.Add(newTile);
    }

}
