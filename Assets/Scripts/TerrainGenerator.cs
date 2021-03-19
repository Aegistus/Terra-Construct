using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public TerrainTileSet tileSet;
    public float tileSize = 100f;
    public float terrainXSize = 1000f;
    public float terrainZSize = 1000f;
    public float noiseScale = 10f;

    private int xTileTotal;
    private int zTileTotal;
    private GameObject[,] generatedTiles;
    private float xNoiseRandomness;
    private float zNoiseRandomness;

    private void Start()
    {
        xTileTotal = (int)(terrainXSize / tileSize);
        zTileTotal = (int)(terrainZSize / tileSize);
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        xNoiseRandomness = Random.Range(0, 10000);
        zNoiseRandomness = Random.Range(0, 10000);
        StartCoroutine(GenerateTerrain());
    }

    public IEnumerator GenerateTerrain()
    {
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                float noise = Mathf.PerlinNoise((x + xNoiseRandomness) * noiseScale, (z + zNoiseRandomness) * noiseScale);
                if (noise < .5)
                {
                    int randomTileIndex = Random.Range(0, tileSet.flatTiles.Length);
                    GameObject newTile = Instantiate(tileSet.flatTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    generatedTiles[x, z] = newTile;
                }
                else
                {
                    int randomTileIndex = Random.Range(0, tileSet.mountainTiles.Length);
                    GameObject newTile = Instantiate(tileSet.mountainTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    generatedTiles[x, z] = newTile;
                }
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
