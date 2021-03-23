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
        List<Vector2> potentialTiles = new List<Vector2>();
        for (int x = 0; x < TerrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < TerrainData.Tiles.GetLength(1); z++)
            {
                if (!TerrainData.IsOceanTile(x,z) && TerrainData.AdjacentOceanTilesCount(x,z) == 1)
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
            List<TileData> oceanTiles = TerrainData.GetEdgeAdjacentOceanTiles((int)potentialTiles[i].x, (int)potentialTiles[i].y);
            PlaceRiverMouthTile((int)potentialTiles[i].x, (int)potentialTiles[i].y, oceanTiles[0].Transform.position);
            BuildRiver((int)potentialTiles[i].x, (int)potentialTiles[i].y);
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
        TerrainData.Tiles[x, z].ReplaceTile(newTile, TileType.RiverMouth);
        riverMouths.Add(newTile);
    }

    private void BuildRiver(int xStart, int zStart)
    {
        TerrainData.Coordinates currentTileCoords = new TerrainData.Coordinates(xStart, zStart);
        List<TileData> riverTiles = new List<TileData>();
        TileData currentTile = TerrainData.Tiles[xStart, zStart];
        // build river while river is still under max length
        while (riverTiles.Count < maxRiverLength)
        {
            List<TileData> adjacentLand = TerrainData.GetEdgeAdjacentLandTiles(currentTileCoords.x, currentTileCoords.z);
            if (adjacentLand.Count == 0)
            {
                break;
            }
            // pick a random direction to go in
            int randIndex = Random.Range(0, adjacentLand.Count);
            TileData nextTile = adjacentLand[randIndex];
            currentTileCoords = TerrainData.GetTileCoordinates(nextTile);
            TerrainData.Tiles[currentTileCoords.x, currentTileCoords.z].ReplaceTile(ReplaceWithRiverTile(nextTile, currentTile), TileType.RiverBendLeft);
            riverTiles.Add(TerrainData.Tiles[currentTileCoords.x, currentTileCoords.z]);
            currentTile = TerrainData.Tiles[currentTileCoords.x, currentTileCoords.z];
        }
        for (int i = 0; i < riverTiles.Count - 1; i++)
        {
            if (Vector3.Angle(riverTiles[i].Transform.forward, riverTiles[i + 1].Transform.forward) != 0)
            {
                TerrainData.Coordinates coords = TerrainData.GetTileCoordinates(riverTiles[i]);
                TerrainData.Tiles[coords.x, coords.z].ReplaceTile(ReplaceWithRiverBendTile(riverTiles[i], riverTiles[i + 1]), TileType.RiverBendLeft);
            }
        }
    }

    private GameObject ReplaceWithRiverBendTile(TileData tile, TileData target)
    {
        int randIndex = Random.Range(0, tileSet.riverCorner.Length);
        GameObject riverBend = Instantiate(tileSet.riverCorner[randIndex], tile.Transform.position, Quaternion.identity, transform);
        Vector3 direction = target.Transform.localPosition - riverBend.transform.localPosition;
        direction = direction.normalized;
        float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
        if (angle > 90 && angle <= 180)
        {
            riverBend.transform.Rotate(0, -90, 0);
            riverBend.transform.localPosition += new Vector3(tileSize, 0, 0);
        }
        //else if (angle < -90 && angle >= -180)
        //{
        //    riverBend.transform.Rotate(0, 180, 0);
        //    riverBend.transform.localPosition += new Vector3(tileSize, 0, tileSize);
        //}
        //else if (angle < 0 && angle >= -90)
        //{
        //    riverBend.transform.Rotate(0, 90, 0);
        //    riverBend.transform.localPosition += new Vector3(0, 0, tileSize);
        //}
        return riverBend;
    }

    private GameObject ReplaceWithRiverTile(TileData tile, TileData lastTile)
    {
        int randIndex = Random.Range(0, tileSet.riverStraight.Length);
        GameObject riverTile = Instantiate(tileSet.riverStraight[randIndex], tile.Transform.position, Quaternion.identity, transform);
        Vector3 direction = lastTile.Transform.localPosition - riverTile.transform.localPosition;
        direction = direction.normalized;
        float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
        if (angle > 90 && angle <= 180)
        {
            riverTile.transform.Rotate(0, -90, 0);
            riverTile.transform.localPosition += new Vector3(tileSize, 0, 0);
        }
        else if (angle < -90 && angle >= -180)
        {
            riverTile.transform.Rotate(0, 180, 0);
            riverTile.transform.localPosition += new Vector3(tileSize, 0, tileSize);
        }
        else if (angle < 0 && angle >= -90)
        {
            riverTile.transform.Rotate(0, 90, 0);
            riverTile.transform.localPosition += new Vector3(0, 0, tileSize);
        }
        return riverTile;
    }
}
