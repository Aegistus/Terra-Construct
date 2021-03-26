using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestPlacer : MonoBehaviour
{
    public TreeSet trees;
    [Range(0f, 1f)]
    public float rareTreePercent = .1f;
    public bool placeAboveWaterLevel = true;
    public bool placeBelowMountainLevel = true;
    [Range(0f, 1f)]
    public float elevationMin = 0f;
    [Range(0f, 1f)]
    public float elevationMax = 0f;
    public int maxTreesPerForestTile = 10;
    [Range(0f, 1f)]
    public float treeClumping = .5f;

    private TerrainConstructor terrain;
    private TerrainData Data => terrain.terrainData;

    public void PlaceForests()
    {
        terrain = GetComponent<TerrainConstructor>();
        for (int x = 0; x < Data.Tiles.GetLength(0); x++)
        {
            for (int z = 0; z < Data.Tiles.GetLength(1); z++)
            {
                
            }
        }
    }

    private void OnValidate()
    {
        if (placeAboveWaterLevel)
        {
            elevationMin = 0f;
        }
        else if (elevationMin >= elevationMax)
        {
            elevationMin = elevationMax - .01f;
        }
        if (placeBelowMountainLevel)
        {
            elevationMax = 1f;
        }
        else if (elevationMax <= elevationMin)
        {
            elevationMax = elevationMin + .01f;
        }
    }
}
