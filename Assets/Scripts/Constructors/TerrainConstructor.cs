using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public NoiseMap elevationNoiseMap;

    [Header("Tile Settings")]
    public int tileSize = 100;

    [Header("Ocean Settings")]
    [Range(0f, 1f)]
    public float oceanPercent = .4f;

    [Header("Chunk Settings")]
    public int chunkSize = 3;
    public int XTileTotal { get; private set; }
    public int ZTileTotal { get; private set; }
    public TerrainData terrainData;

    private TerrainChunk[,] chunks;

    private void Start()
    {
        //StartCoroutine(PlaceMountains());
    }

    public void ConstructTerrain()
    {
        XTileTotal = settings.xSize / tileSize;
        ZTileTotal = settings.zSize / tileSize;
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        terrainData = new TerrainData(XTileTotal, ZTileTotal);
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
        terrainData.Tiles = new TileData[XTileTotal, ZTileTotal];
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
        terrainData.Tiles = new TileData[XTileTotal, ZTileTotal];
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
            terrainData.Tiles[x, z] = new TileData(newTile, TileType.OceanFloor, noiseValue);
        }
        else
        {
            int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
            GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            terrainData.Tiles[x, z] = new TileData(newTile, TileType.FlatLand, noiseValue);
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
        terrainData = null;
    }

   public void ChunkTerrain()
   {
        chunks = new TerrainChunk[XTileTotal / chunkSize + 1, ZTileTotal / chunkSize + 1];
        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
            {
                GameObject chunkObject = new GameObject("Chunk");
                chunkObject.transform.parent = transform;
                chunks[x,z] = new TerrainChunk(chunkObject);
            }
        }
        for (int x = 0; x < terrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < terrainData.Tiles.GetLength(1); z++)
            {
                chunks[x / chunkSize, z / chunkSize].tiles.Add(terrainData.Tiles[x, z]);
                terrainData.Tiles[x, z].Transform.parent = chunks[x / chunkSize, z / chunkSize].gameObject.transform;
            }
        }
        foreach (var chunk in chunks)
        {
            chunk.CombineMeshes();
        }
   }

}
