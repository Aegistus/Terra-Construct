using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;
    public NoiseMap elevationNoiseMap;

    [Header("Tile Settings")]
    public int tileSize = 100;

    [Header("Ocean Settings")]
    [Range(0f, 1f)]
    public float seaLevel = .4f;

    private int xTileTotal;
    private int zTileTotal;
    private GameObject[,] generatedTiles;

    private void Start()
    {

        //StartCoroutine(PlaceMountains());
    }

    public void ConstructTerrain()
    {
        xTileTotal = settings.xSize / tileSize;
        zTileTotal = settings.zSize / tileSize;
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        StartCoroutine(PlaceTileGrid());
    }


    public IEnumerator PlaceTileGrid()
    {
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                PlaceTile(x, z);
                yield return null;
            }
        }
    }

    public void ConstructTerrainEditor()
    {
        xTileTotal = settings.xSize / tileSize;
        zTileTotal = settings.zSize / tileSize;
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        PlaceTileGridEditor();
    }

    public void PlaceTileGridEditor()
    {
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                PlaceTile(x, z);
            }
        }
    }

    public void PlaceTile(int x, int z)
    {
        if (elevationNoiseMap.GetLayeredPerlinValueAtPosition(x, z) < seaLevel)
        {
            int randomTileIndex = Random.Range(0, tileSet.oceanTiles.Length);
            GameObject newTile = Instantiate(tileSet.oceanTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            generatedTiles[x, z] = newTile;
        }
        else
        {
            int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
            GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            generatedTiles[x, z] = newTile;
        }
        print(elevationNoiseMap.GetLayeredPerlinValueAtPosition(x * tileSize, z * tileSize));
    }

    //public IEnumerator PlaceMountains()
    //{
    //    for (int x = 0; x < settings.xSize; x += (int)mountainSpacing)
    //    {
    //        for (int z = 0; z < settings.zSize; z += (int)mountainSpacing)
    //        {
    //            float noise = Mathf.PerlinNoise((x + xElevationNoise) * elevationNoiseScale, (z + zElevationNoise) * elevationNoiseScale);
    //            Vector3 positionWithOffset = new Vector3(Random.Range(-mountainSpacing / 2, mountainSpacing / 2), 0, Random.Range(-mountainSpacing / 2, mountainSpacing / 2));
    //            positionWithOffset += new Vector3(x, transform.position.y + yPosition, z);
    //            positionWithOffset = new Vector3(Mathf.Clamp(positionWithOffset.x, 0, settings.xSize), positionWithOffset.y, Mathf.Clamp(positionWithOffset.z, 0, settings.zSize));
    //            Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
    //            GameObject mountainVariant = null;
    //            if (noise >= smallStart && noise < mediumStart)
    //            {
    //                mountainVariant = mountainSet.smallMountains[Random.Range(0, mountainSet.smallMountains.Length)];
    //            }
    //            else if (noise >= mediumStart && noise < largeStart)
    //            {
    //                mountainVariant = mountainSet.mediumMountains[Random.Range(0, mountainSet.mediumMountains.Length)];
    //            }
    //            else if (noise >= largeStart)
    //            {
    //                mountainVariant = mountainSet.largeMountains[Random.Range(0, mountainSet.largeMountains.Length)];
    //            }
    //            if (mountainVariant != null)
    //            {
    //                Instantiate(mountainVariant, positionWithOffset,
    //                    randomRotation, transform);
    //            }
    //            yield return null;
    //        }
    //    }
    //}

    //bool[,] potentialStarts;
    //public IEnumerator PlaceRivers()
    //{
    //    potentialStarts = new bool[xTileTotal, zTileTotal];
    //    for (int x = 0; x < xTileTotal; x++)
    //    {
    //        for (int z = 0; z < zTileTotal; z++)
    //        {
    //            float noise = Mathf.PerlinNoise((x * tileSize + xElevationNoise) * elevationNoiseScale, (z * tileSize + zElevationNoise) * elevationNoiseScale);
    //            if (noise < riverMax)
    //            {
    //                potentialStarts[x, z] = true;
    //                generatedTiles[x, z].SetActive(false);
    //                generatedTiles[x, z] = null;
    //            }
    //        }
    //    }
    //    yield return null;
    //}

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
        generatedTiles = null;
    }

}
