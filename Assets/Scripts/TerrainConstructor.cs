﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;

    [Header("Tile Settings")]
    public float tileSize = 100f;

    [Header("Elevation Settings")]
    public float elevationNoiseScale = .003f;
    public int octaves = 3;
    [Tooltip("Controls increase in frequency of octaves")]
    public float elevationLacunarity = 1f;
    [Tooltip("Controls decrease in amplitude of octaves")]
    public float elevationPersistance = 1f;


    [Header("Mountain Settings")]
    public float mountainSpacing = 30f;
    [Range(0f, 1f)]
    public float smallStart = .4f;
    [Range(0f, 1f)]
    public float mediumStart = .6f;
    [Range(0f, 1f)]
    public float largeStart = .8f;
    public float yPosition = -3f;

    [Header("River Settings")]
    [Range(0f, 1f)]
    public float riverMax = .1f;

    private int xTileTotal;
    private int zTileTotal;
    private GameObject[,] generatedTiles;
    private float xElevationNoise;
    private float zElevationNoise;
    private float[,] elevationNoiseMap;

    private void Start()
    {
        xTileTotal = (int)(settings.xSize / tileSize);
        zTileTotal = (int)(settings.zSize / tileSize);
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        xElevationNoise = Random.Range(0, 10000);
        zElevationNoise = Random.Range(0, 10000);
        elevationNoiseMap = Noise.GenerateNoiseMap((int)settings.xSize, (int)settings.zSize, elevationNoiseScale, octaves, elevationLacunarity, elevationPersistance);
        StartCoroutine(PlaceTileGrid());
        //StartCoroutine(PlaceMountains());
    }


    public IEnumerator PlaceTileGrid()
    {
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                int randomTileIndex = Random.Range(0, tileSet.tiles.Length);
                GameObject newTile = Instantiate(tileSet.tiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                generatedTiles[x, z] = newTile;
                yield return new WaitForSeconds(.05f);
            }
        }
        StartCoroutine(PlaceRivers());
    }

    public IEnumerator PlaceMountains()
    {
        for (int x = 0; x < settings.xSize; x += (int)mountainSpacing)
        {
            for (int z = 0; z < settings.zSize; z += (int)mountainSpacing)
            {
                float noise = Mathf.PerlinNoise((x + xElevationNoise) * elevationNoiseScale, (z + zElevationNoise) * elevationNoiseScale);
                Vector3 positionWithOffset = new Vector3(Random.Range(-mountainSpacing / 2, mountainSpacing / 2), 0, Random.Range(-mountainSpacing / 2, mountainSpacing / 2));
                positionWithOffset += new Vector3(x, transform.position.y + yPosition, z);
                positionWithOffset = new Vector3(Mathf.Clamp(positionWithOffset.x, 0, settings.xSize), positionWithOffset.y, Mathf.Clamp(positionWithOffset.z, 0, settings.zSize));
                Quaternion randomRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360f), 0));
                GameObject mountainVariant = null;
                if (noise >= smallStart && noise < mediumStart)
                {
                    mountainVariant = mountainSet.smallMountains[Random.Range(0, mountainSet.smallMountains.Length)];
                }
                else if (noise >= mediumStart && noise < largeStart)
                {
                    mountainVariant = mountainSet.mediumMountains[Random.Range(0, mountainSet.mediumMountains.Length)];
                }
                else if (noise >= largeStart)
                {
                    mountainVariant = mountainSet.largeMountains[Random.Range(0, mountainSet.largeMountains.Length)];
                }
                if (mountainVariant != null)
                {
                    Instantiate(mountainVariant, positionWithOffset,
                        randomRotation, transform);
                }
                yield return null;
            }
        }
    }

    bool[,] potentialStarts;
    public IEnumerator PlaceRivers()
    {
        potentialStarts = new bool[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                float noise = Mathf.PerlinNoise((x * tileSize + xElevationNoise) * elevationNoiseScale, (z * tileSize + zElevationNoise) * elevationNoiseScale);
                if (noise < riverMax)
                {
                    potentialStarts[x, z] = true;
                    generatedTiles[x, z].SetActive(false);
                    generatedTiles[x, z] = null;
                }
            }
        }
        yield return null;
    }

}
