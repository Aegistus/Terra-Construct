using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainConstructor : MonoBehaviour
{
    public MountainSet mountains;
    [Range(0f, 1f)]
    public float mountainLevel = .5f;
    [Range(0f, 1f)]
    public float foothillLevel = .4f;
    [Range(0f, 1f)]
    public float mountainClumping = .5f;
    //public float mountainSpacing = 10f;
    public float sizeVariationLower = .8f;
    public float sizeVariationUpper = 1.2f;
    public int maxMountainsPerTile = 5;
    public int maxFoothillsPerTile = 2;
    public float seaMountainLevel = -3f;

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
                        if (Random.value > mountainClumping)
                        {
                            break;
                        }
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize / 2), transform.position.y, Random.Range(0, terrain.tileSize / 2));
                        int randIndex = Random.Range(0, mountains.mountains.Length);
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        GameObject newMountain = Instantiate(mountains.mountains[randIndex], randomPosition + tile.Transform.position, randomRotation, tile.Transform);
                        newMountain.transform.localScale = newMountain.transform.localScale * Random.Range(sizeVariationLower, sizeVariationUpper);
                        placedMountains.Add(newMountain);
                    }
                }
            }
        }
    }

    public void PlaceFootHills()
    {
        terrain = GetComponent<TerrainConstructor>();
        for (int x = 0; x < TerrainData.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < TerrainData.Tiles.GetLength(1); z++)
            {
                TileData tile = TerrainData.Tiles[x, z];
                if (tile.noiseValue > foothillLevel && tile.noiseValue < mountainLevel)
                {
                    for (int i = 0; i < maxFoothillsPerTile; i++)
                    {
                        if (Random.value > mountainClumping)
                        {
                            break;
                        }
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize / 2), transform.position.y, Random.Range(0, terrain.tileSize / 2));
                        if (TerrainData.IsOceanTile(x, z) || TerrainData.IsCoastalTile(x, z))
                        {
                            randomPosition.y = seaMountainLevel;
                        }
                        int randIndex = Random.Range(0, mountains.hills.Length);
                        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        GameObject newMountain = Instantiate(mountains.hills[randIndex], randomPosition + tile.Transform.position, randomRotation, tile.Transform);
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
        if (foothillLevel >= mountainLevel)
        {
            foothillLevel = mountainLevel - .01f;
        }
    }
}
