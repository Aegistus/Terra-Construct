using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloraGenerator
{
    public static TerrainData Generate(TerrainData data, TerrainSettings settings, TreeSet treeSet, GrassSet grassSet)
    {
        List<TerrainObjectData> placedTrees = new List<TerrainObjectData>();
        List<TerrainObjectData> placedGrass = new List<TerrainObjectData>();
        float minValue;
        float maxValue;
        if (settings.placeAboveWaterLevel)
        {
            minValue = settings.oceanPercent + .01f;
        }
        else
        {
            minValue = settings.elevationMin;
        }
        if (settings.placeBelowMountainLevel)
        {
            maxValue = settings.mountainLevel - .01f;
        }
        else
        {
            maxValue = settings.elevationMax;
        }
        for (int x = 0; x < data.xSize; x++)
        {
            for (int z = 0; z < data.zSize; z++)
            {
                TileData tile = data.GetTileAtCoordinates(x, z);
                if (tile.noiseValue > minValue && tile.noiseValue < maxValue && tile.type == TileType.FlatLand)
                {
                    List<Vector3> treeSpawns = PoissonDiscSampling.GeneratePointsOfDifferentSize(settings.treePlacementRadius, Vector2.one * settings.tileSize);
                    for (int i = 0; i < treeSpawns.Count; i++)
                    {
                        int typeIndex = Random.Range(0, treeSet.commonTrees.Count);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        placedTrees.Add(new TerrainObjectData(typeIndex, tile.Position + new Vector3(treeSpawns[i].x, 0, treeSpawns[i].y), randomRotation, Vector3.one * 2));
                    }
                    // create grass
                    List<Vector3> grassSpawns = PoissonDiscSampling.GeneratePointsOfDifferentSize(settings.grassPlacementRadius, Vector2.one * settings.tileSize);
                    for (int i = 0; i < grassSpawns.Count; i++)
                    {
                        int typeIndex = Random.Range(0, grassSet.common.Length);
                        Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
                        placedGrass.Add(new TerrainObjectData(typeIndex, tile.Position + new Vector3(grassSpawns[i].x, 0, grassSpawns[i].y), randomRotation, Vector3.one));
                    }
                }
            }
        }
        data.trees = placedTrees;
        data.grass = placedGrass;
        return data;
    }

    //private void OnDrawGizmos()
    //{
    //    if (placedTrees != null)
    //    {
    //        Gizmos.color = Color.red;
    //        foreach (var tree in placedTrees)
    //        {
    //            Gizmos.DrawSphere(tree.position, 1f);
    //        }
    //    }
    //}
}
