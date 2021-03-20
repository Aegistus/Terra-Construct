using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;

    [Header("Tile Settings")]
    public float tileSize = 100f;

    [Header("Mountain Settings")]
    public float mountainSpacing = 30f;
    [Range(0f, 1f)]
    public float smallStart = .4f;
    [Range(0f, 1f)]
    public float mediumStart = .6f;
    [Range(0f, 1f)]
    public float largeStart = .8f;
    public float yPosition = -3f;
    public float mountainNoiseScale = 1f;

    private int xTileTotal;
    private int zTileTotal;
    private GameObject[,] generatedTiles;
    private float xMountainNoise;
    private float zMountainNoise;

    private void Start()
    {
        xTileTotal = (int)(settings.xSize / tileSize);
        zTileTotal = (int)(settings.zSize / tileSize);
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        xMountainNoise = Random.Range(0, 10000);
        zMountainNoise = Random.Range(0, 10000);
        StartCoroutine(PlaceTileGrid());
        StartCoroutine(PlaceMountains());
    }

    public IEnumerator PlaceTileGrid()
    {
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                int randomTileIndex = Random.Range(0, tileSet.flatTiles.Length);
                GameObject newTile = Instantiate(tileSet.flatTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                generatedTiles[x, z] = newTile;
                yield return new WaitForSeconds(.05f);
            }
        }
    }

    public IEnumerator PlaceMountains()
    {
        for (int x = 0; x < settings.xSize / mountainSpacing; x++)
        {
            for (int z = 0; z < settings.zSize / mountainSpacing; z++)
            {
                float noise = Mathf.PerlinNoise((x + xMountainNoise) * mountainNoiseScale, (z + zMountainNoise) * mountainNoiseScale);
                Vector3 randomOffset = new Vector3(Random.Range(-mountainSpacing / 2, mountainSpacing / 2), 0, Random.Range(-mountainSpacing / 2, mountainSpacing / 2));
                Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
                GameObject mountainVariant = null;
                if (noise >= smallStart && noise < mediumStart)
                {
                    mountainVariant = mountainSet.smallMountains[Random.Range(0, mountainSet.smallMountains.Length)];
                }
                else if (noise >= mediumStart && noise < largeStart)
                {
                    mountainVariant = mountainSet.mediumMountains[Random.Range(0, mountainSet.smallMountains.Length)];
                }
                else if (noise >= largeStart)
                {
                    mountainVariant = mountainSet.largeMountains[Random.Range(0, mountainSet.smallMountains.Length)];
                }
                if (mountainVariant != null)
                {
                    Instantiate(mountainVariant, new Vector3(x * mountainSpacing, transform.position.y + yPosition, z * mountainSpacing) + randomOffset,
                        randomRotation, transform);
                }
                yield return null;
            }
        }
    }

}
