using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewTerrainSettings", menuName = "Terrain Settings", order = 1)]
public class TerrainSettings : ScriptableObject
{
    public int xSize = 1000;
    public int zSize = 1000;

    [Header("Tile Settings")]
    public int tileSize = 100;
    public int waterTileSize = 500;

    [Header("Ocean Settings")]
    [Range(0f, 1f)]
    public float oceanPercent = .4f;
    public float seaLevel = -1;

    [Header("River Settings")]
    public int numberOfRivers = 1;
    public int riverMaxLength = 5;

    [Header("Mountain Settings")]
    [Range(0f, 1f)]
    public float mountainLevel = .5f;
    [Range(0f, 1f)]
    public float foothillLevel = .4f;
    [Range(0f, 1f)]
    public float mountainClumping = .5f;
    public float sizeVariationLower = .8f;
    public float sizeVariationUpper = 1.2f;
    public int maxMountainsPerTile = 5;
    public int maxFoothillsPerTile = 2;
    public float seaMountainLevel = -3f;

    [Header("Flora Settings")]
    public bool placeAboveWaterLevel = true;
    public bool placeBelowMountainLevel = true;
    [Range(0f, 1f)]
    public float elevationMin = 0f;
    [Range(0f, 1f)]
    public float elevationMax = 0f;
    public int maxTreesPerForestTile = 10;
    public int maxGrassPerTile = 20;
    [Range(0f, 1f)]
    public float treeClumping = .5f;
}
