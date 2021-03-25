﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainConstructor : MonoBehaviour
{
    public MountainSet mountains;
    [Range(0f, 1f)]
    public float mountainLevel = .5f;
    public float mountainSpacing = 10f;
    public float sizeVariationLower = .8f;
    public float sizeVariationUpper = 1.2f;
    public int maxMountainsPerTile = 5;

    private TerrainConstructor terrain;
    private TerrainData TerrainData => terrain.terrainData;
    private List<GameObject> placedMountains = new List<GameObject>();

    public void PlaceMountains()
    {
        terrain = GetComponent<TerrainConstructor>();
        for (int x = 0; x < TerrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < TerrainData.Tiles.GetLength(1); z++)
            {
                TileData tile = TerrainData.Tiles[x, z];
                if (tile.noiseValue > mountainLevel)
                {
                    for (int i = 0; i < maxMountainsPerTile; i++)
                    {
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize), transform.position.y, Random.Range(0, terrain.tileSize));
                        int randIndex = Random.Range(0, mountains.largeMountains.Length);
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        GameObject newMountain = Instantiate(mountains.largeMountains[randIndex], randomPosition + tile.Transform.position, randomRotation, transform);
                        newMountain.transform.localScale = newMountain.transform.localScale * Random.Range(sizeVariationLower, sizeVariationUpper);
                        placedMountains.Add(newMountain);
                    }
                }
            }
        }
    }

    public void ClearMountains()
    {
        while (placedMountains.Count > 0)
        {
            for (int i = 0; i < placedMountains.Count; i++)
            {
                DestroyImmediate(placedMountains[i]);
                placedMountains.RemoveAt(i);
            }
        }
    }

    private void OnValidate()
    {
        if (sizeVariationLower >= sizeVariationUpper)
        {
            sizeVariationLower = sizeVariationUpper - .01f;
        }
    }
}