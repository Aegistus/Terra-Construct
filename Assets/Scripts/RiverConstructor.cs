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
        for (int x = 0; x < TerrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < TerrainData.Tiles.GetLength(1); z++)
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
            PlaceRiverMouthTile(potentialTiles[i].x, potentialTiles[i].z, oceanTiles[0].Transform.position);
            TileData currentTile = TerrainData.Tiles[potentialTiles[i].x, potentialTiles[i].z];
            for (int j = 0; j < maxRiverLength; j++)
            {
                if (currentTile != null)
                {
                    currentTile = ContinueRiverPath(TerrainData.GetTileCoordinates(currentTile));
                }
            }
        }
    }

    private TileData ContinueRiverPath(Coordinates currentTileCoords)
    {
        // get random direction for next tile
        TileData currentTile = TerrainData.Tiles[currentTileCoords.x, currentTileCoords.z];
        List<TileData> adjacentLandTiles = TerrainData.GetEdgeAdjacentLandTiles(currentTileCoords.x, currentTileCoords.z);
        if (adjacentLandTiles.Count == 0)
        {
            return null;
        }
        GameObject newTile = null;
        TileType newType = TileType.FlatLand;
        TileData nextTile = null;
        int randIndex;
        if (currentTile.type != TileType.RiverMouth)
        {
            for (int i = 0; i < adjacentLandTiles.Count; i++)
            {
                Vector3 direction = adjacentLandTiles[i].Transform.localPosition - currentTile.Transform.localPosition;
                direction = direction.normalized;
                if (direction == currentTile.Transform.forward)
                {
                    randIndex = Random.Range(0, tileSet.riverStraight.Length);
                    newTile = Instantiate(tileSet.riverStraight[randIndex], currentTile.Transform.position, Quaternion.identity, transform);
                    newType = TileType.RiverStraight;
                    nextTile = adjacentLandTiles[i];
                    break;
                }
                //else if (direction == currentTile.Transform.right) // if right, make right tile
                //{
                //    randIndex = Random.Range(0, tileSet.riverCornerRight.Length);
                //    newTile = Instantiate(tileSet.riverCornerRight[randIndex], currentTile.Transform.position, Quaternion.identity, transform);
                //    newType = TileType.RiverBendRight;
                //    nextTile = adjacentLandTiles[i];
                //    break;
                //}
                //else if (direction == -currentTile.Transform.right) // if left, make left tile
                //{
                //    randIndex = Random.Range(0, tileSet.riverCornerLeft.Length);
                //    newTile = Instantiate(tileSet.riverCornerLeft[randIndex], currentTile.Transform.position, Quaternion.identity, transform);
                //    newType = TileType.RiverBendLeft;
                //    nextTile = adjacentLandTiles[i];
                //    break;
                //}
            }
        }
        else
        {
            nextTile = adjacentLandTiles[0];
        }
        // replace tile in TerrainData with new tile
        if (newTile != null)
        {
            TerrainData.Tiles[currentTileCoords.x, currentTileCoords.z].ReplaceTile(newTile, newType);
        }
        return nextTile;
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
        TerrainData.Tiles[x, z].ReplaceTile(newTile, TileType.RiverMouth);
        riverMouths.Add(newTile);
    }

}
