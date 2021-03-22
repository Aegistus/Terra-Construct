﻿using System.Collections;
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
    public float oceanPercent = .4f;

    public int XTileTotal { get; private set; }
    public int ZTileTotal { get; private set; }
    public GameObject[,] GeneratedTiles { get; private set; }

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
        GeneratedTiles = new GameObject[XTileTotal, ZTileTotal];
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
        GeneratedTiles = new GameObject[XTileTotal, ZTileTotal];
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
        if (elevationNoiseMap.GetLayeredPerlinValueAtPosition(x, z) < oceanPercent)
        {
            int randomTileIndex = Random.Range(0, tileSet.oceanFloorTiles.Length);
            GameObject newTile = Instantiate(tileSet.oceanFloorTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            GeneratedTiles[x, z] = newTile;
        }
        else
        {
            int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
            GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            GeneratedTiles[x, z] = newTile;
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
        GeneratedTiles = null;
    }

}
