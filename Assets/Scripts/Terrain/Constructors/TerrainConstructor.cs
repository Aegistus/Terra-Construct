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

    public void Construct()
    {
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        terrainData.CreateTiles(settings.xSize / tileSize, settings.zSize / tileSize);
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
        for (int x = 0; x < terrainData.xSize; x++)
        {
            for (int z = 0; z < terrainData.zSize; z++)
            {
                PlaceTile(x, z);
                yield return null;
            }
        }
    }

    public void PlaceTileGridEditor()
    {
        for (int x = 0; x < terrainData.xSize; x++)
        {
            for (int z = 0; z < terrainData.zSize; z++)
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
            Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
            terrainData.GetTileAtCoordinates(x,z).ReplaceTile(TileType.OceanFloor, position, Vector3.zero);
            terrainData.GetTileAtCoordinates(x, z).noiseValue = noiseValue;
        }
        else
        {
            Vector3 position = new Vector3(x * tileSize, transform.position.y, z * tileSize);
            terrainData.GetTileAtCoordinates(x, z).ReplaceTile(TileType.FlatLand, position, Vector3.zero);
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

    public void LoadTerrain(TerrainData data)
    {
        ClearTerrain();
        terrainData = data;
        for (int x = 0; x < terrainData.xSize; x++)
        {

        }
    }
}
