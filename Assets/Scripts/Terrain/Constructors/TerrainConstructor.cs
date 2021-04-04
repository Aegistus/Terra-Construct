using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;
    public TreeSet treeSet;
    public GrassSet grassSet;
    public NoiseMap elevationNoiseMap;
    public string terrainFileName = "save.txt";

    [HideInInspector] public TerrainData terrainData;
    private List<GameObject> oceanTiles = new List<GameObject>();

    public void GenerateTerrain()
    {
        terrainData = new TerrainData();
        terrainData.CreateTiles(settings.xSize / settings.tileSize, settings.zSize / settings.tileSize);
        terrainData = LandmassConstructor.GenerateLandmasses(terrainData, settings, elevationNoiseMap);
        terrainData = CoastConstructor.GenerateCoasts(terrainData, settings);
        terrainData = RiverConstructor.GenerateRiver(terrainData, settings);
        terrainData.mountains = MountainConstructor.Generate(terrainData, settings, mountainSet.mountains);
        terrainData.foothills = MountainConstructor.Generate(terrainData, settings, mountainSet.hills);
        terrainData.boulders = MountainConstructor.GenerateScatteredBoulders(terrainData, settings, mountainSet.boulders);
        terrainData = BiomeConstructor.GenerateMoistureLevels(terrainData, settings);
        terrainData = BiomeConstructor.GenerateTemperatureLevels(terrainData, settings);
    }

    public void GenerateFlora()
    {
        terrainData = FloraGenerator.Generate(terrainData, settings, treeSet, grassSet);
    }


    public void ConstructTerrain()
    {
        for (int x = 0; x < terrainData.xSize; x++)
        {
            for (int z = 0; z < terrainData.zSize; z++)
            {
                TileData tile = terrainData.GetTileAtCoordinates(x, z);
                GameObject[] variants;
                switch(tile.type)
                {
                    case TileType.CoastInnerCorner: variants = tileSet.coastalInnerCorner;break;
                    case TileType.CoastOuterCorner: variants = tileSet.coastalOuterCorner;break;
                    case TileType.CoastStraight: variants = tileSet.coastalStraight; break;
                    case TileType.FlatLand: variants = tileSet.landTiles; break;
                    case TileType.Mountain: variants = tileSet.landTiles; break;
                    case TileType.Swamp: variants = tileSet.swampTiles; break;
                    case TileType.OceanFloor: variants = tileSet.oceanFloorTiles; break;
                    case TileType.RiverMouth: variants = tileSet.riverMouth; break;
                    case TileType.RiverStraight: variants = tileSet.riverStraight; break;
                    case TileType.RiverBendRight: variants = tileSet.riverCornerRight; break;
                    case TileType.RiverBendLeft: variants = tileSet.riverCornerLeft; break;
                    case TileType.RiverEnd: variants = tileSet.riverEnd; break;
                    default: variants = tileSet.landTiles; break;
                }
                Instantiate(variants[Random.Range(0, variants.Length)], tile.Position, Quaternion.Euler(tile.Rotation), transform);
            }
        }
        for (int i = 0; i < terrainData.mountains.Count; i++)
        {
            TerrainObjectData mountain = terrainData.mountains[i];
            Instantiate(mountainSet.mountains[mountain.typeIndex], mountain.Position, Quaternion.Euler(mountain.Rotation), transform);
        }
        for (int i = 0; i < terrainData.foothills.Count; i++)
        {
            TerrainObjectData foothill = terrainData.foothills[i];
            Instantiate(mountainSet.hills[foothill.typeIndex], foothill.Position, Quaternion.Euler(foothill.Rotation), transform);
        }
        for (int i = 0; i < terrainData.boulders.Count; i++)
        {
            TerrainObjectData boulder = terrainData.boulders[i];
            Instantiate(mountainSet.boulders[boulder.typeIndex], boulder.Position, Quaternion.Euler(boulder.Rotation), transform);
        }
        GenerateOcean();
    }

    public void GenerateOcean()
    {
        if (oceanTiles != null)
        {
            ClearOcean();
        }
        int xTileTotal = settings.xSize / settings.waterTileSize;
        int zTileTotal = settings.zSize / settings.waterTileSize;
        oceanTiles = new List<GameObject>();
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                int randIndex = Random.Range(0, tileSet.waterTiles.Length);
                oceanTiles.Add(Instantiate(tileSet.waterTiles[randIndex], new Vector3(x * settings.waterTileSize, settings.seaLevel, z * settings.waterTileSize), Quaternion.identity, transform));
            }
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

    public void ClearOcean()
    {
        if (oceanTiles == null)
        {
            return;
        }
        for (int i = 0; i < oceanTiles.Count; i++)
        {
            DestroyImmediate(oceanTiles[i]);
            oceanTiles[i] = null;
        }
        oceanTiles = null;
    }

    private void OnDrawGizmos()
    {
        foreach (var tile in terrainData.tiles)
        {
            Gizmos.color = new Color(tile.temperatureValue, 0, 1 - tile.temperatureValue);
            Gizmos.DrawSphere(tile.Position + new Vector3(50, 0, 50), 10f);
        }
    }
}
