using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TerrainConstructor : MonoBehaviour, IConstructor
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public NoiseMap elevationNoiseMap;

    public TerrainData terrainData;

    [Header("Tile Settings")]
    public int tileSize = 100;

    [Header("Ocean Settings")]
    [Range(0f, 1f)]
    public float oceanPercent = .4f;

    [Header("Chunk Settings")]
    public int chunkSize = 3;
    public int XTileTotal { get; private set; }
    public int ZTileTotal { get; private set; }

    public void Construct()
    {
        XTileTotal = settings.xSize / tileSize;
        ZTileTotal = settings.zSize / tileSize;
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        terrainData.CreateTiles(XTileTotal, ZTileTotal);
        if (Application.isPlaying)
        {
            StartCoroutine(PlaceTileGrid());
        }
        else
        {
            PlaceTileGridEditor();
        }
    }

    public IEnumerator PlaceTileGrid()
    {
        for (int x = 0; x < XTileTotal; x++)
        {
            for (int z = 0; z < ZTileTotal; z++)
            {
                PlaceTile(x, z);
                yield return null;
            }
        }
    }

    public void PlaceTileGridEditor()
    {
        for (int x = 0; x < XTileTotal; x++)
        {
            for (int z = 0; z < ZTileTotal; z++)
            {
                PlaceTile(x, z);
            }
        }
    }

    public void PlaceTile(int x, int z)
    {
        float noiseValue = elevationNoiseMap.GetLayeredPerlinValueAtPosition(x, z);
        if (noiseValue < oceanPercent)
        {
            int randomTileIndex = Random.Range(0, tileSet.oceanFloorTiles.Length);
            GameObject newTile = Instantiate(tileSet.oceanFloorTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            terrainData.GetTileAtCoordinates(x,z).ReplaceTile(newTile, TileType.OceanFloor);
            terrainData.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
        }
        else
        {
            int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
            GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            terrainData.GetTileAtCoordinates(x, z).ReplaceTile(newTile, TileType.FlatLand);
            terrainData.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
        }
        //print(elevationNoiseMap.GetLayeredPerlinValueAtPosition(x * tileSize, z * tileSize));
    }

    public void ClearTerrain()
    {
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        elevationNoiseMap.ResetNoiseRange();
    }


}
