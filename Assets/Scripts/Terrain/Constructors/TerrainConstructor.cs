using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;
    public TreeSet treeSet;
    public NoiseMap elevationNoiseMap;

    [HideInInspector] public TerrainData terrainData;

    public void GenerateTerrain()
    {
        terrainData = new TerrainData();
        terrainData.CreateTiles(settings.xSize / settings.tileSize, settings.zSize / settings.tileSize);
        terrainData = LandmassConstructor.GenerateLandmasses(terrainData, settings, elevationNoiseMap);
        terrainData = CoastConstructor.GenerateCoasts(terrainData, settings);
        terrainData = MountainConstructor.GenerateMountains(terrainData, settings, mountainSet);
        terrainData = MountainConstructor.GenerateFoothills(terrainData, settings, mountainSet);
        terrainData = ForestGenerator.Generate(terrainData, settings, treeSet);
    }


    public void ConstructTerrain()
    {
        for (int x = 0; x < terrainData.xSize; x++)
        {
            for (int z = 0; z < terrainData.zSize; z++)
            {
                TileData tile = terrainData.GetTileAtCoordinates(x, z);
                GameObject tileGameObject = null;
                switch(tile.type)
                {
                    case TileType.CoastInnerCorner: tileGameObject = tileSet.coastalInnerCorner[0];break;
                    case TileType.CoastOuterCorner: tileGameObject = tileSet.coastalOuterCorner[0];break;
                    case TileType.CoastStraight: tileGameObject = tileSet.coastalStraight[0]; break;
                    case TileType.FlatLand: tileGameObject = tileSet.landTiles[0]; break;
                    case TileType.OceanFloor: tileGameObject = tileSet.oceanFloorTiles[0]; break;
                }
                Instantiate(tileGameObject, tile.position, Quaternion.Euler(tile.rotation), transform);
            }
        }
        for (int i = 0; i < terrainData.mountains.Count; i++)
        {
            TerrainObjectData mountain = terrainData.mountains[i];
            Instantiate(mountainSet.mountains[mountain.typeIndex], mountain.position, Quaternion.Euler(mountain.rotation), transform);
        }
        for (int i = 0; i < terrainData.foothills.Count; i++)
        {
            TerrainObjectData foothill = terrainData.foothills[i];
            Instantiate(mountainSet.hills[foothill.typeIndex], foothill.position, Quaternion.Euler(foothill.rotation), transform);
        }
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
    }
}
