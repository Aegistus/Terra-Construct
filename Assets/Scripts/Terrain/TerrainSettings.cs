﻿using System;
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

    [Header("Biome Settings")]
    [Range(0f, 1f)]
    public float swampTileChance = .1f;

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
    public int maxMountainsPerTile = 2;
    public float seaMountainLevel = -3f;

    [Header("Flora Settings")]
    public bool placeAboveWaterLevel = true;
    public bool placeBelowMountainLevel = true;
    [Range(0f, 1f)]
    public float elevationMin = 0f;
    [Range(0f, 1f)]
    public float elevationMax = 0f;
    public Vector2 treePlacementRadius = new Vector2(4, 8);
    public Vector2 grassPlacementRadius = new Vector2(3, 6);
    [Range(0f, 1f)]
    public float rareTreePercent = .1f;
}
