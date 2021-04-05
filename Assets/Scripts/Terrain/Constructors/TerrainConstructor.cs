using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public Material snowMaterial;
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
        foreach (var tile in terrainData.tiles)
        {
            if (tile.type == TileType.Mountain)
            {
                tile.mountains = MountainConstructor.Generate(tile, settings, mountainSet.mountains);
            }
            tile.boulders = MountainConstructor.GenerateScatteredBoulders(tile, settings, mountainSet.boulders);
        }
        terrainData = BiomeConstructor.GenerateMoistureLevels(terrainData, settings);
        terrainData = BiomeConstructor.GenerateTemperatureLevels(terrainData, settings);
        terrainData = BiomeConstructor.GenerateBiomes(terrainData, settings);
    }

    public void GenerateFlora()
    {
        terrainData = FloraGenerator.Generate(terrainData, settings, treeSet, grassSet);
    }


    public void ConstructTerrain()
    {
        for (int i = 0; i < terrainData.tiles.Count; i++)
        {
            TileData tile = terrainData.tiles[i];
            GameObject[] variants;
            switch (tile.type)
            {
                case TileType.CoastInnerCorner: variants = tileSet.coastalInnerCorner; break;
                case TileType.CoastOuterCorner: variants = tileSet.coastalOuterCorner; break;
                case TileType.CoastStraight: variants = tileSet.coastalStraight; break;
                case TileType.Forest:
                case TileType.Plains: variants = tileSet.plainsTiles; break;
                case TileType.Mountain: variants = tileSet.plainsTiles; break;
                case TileType.Taiga:
                case TileType.Tundra: variants = tileSet.snowTiles; break;
                case TileType.Desert: variants = tileSet.desertTiles; break;
                case TileType.RainForest: variants = tileSet.rainForestTiles; break;
                case TileType.OceanFloor: variants = tileSet.oceanFloorTiles; break;
                case TileType.RiverMouth: variants = tileSet.riverMouth; break;
                case TileType.RiverStraight: variants = tileSet.riverStraight; break;
                case TileType.RiverBendRight: variants = tileSet.riverCornerRight; break;
                case TileType.RiverBendLeft: variants = tileSet.riverCornerLeft; break;
                case TileType.RiverEnd: variants = tileSet.riverEnd; break;
                default: variants = tileSet.plainsTiles; break;
            }
            GameObject tileGameObject = Instantiate(variants[Random.Range(0, variants.Length)], tile.Position, Quaternion.Euler(tile.Rotation), transform);
            for (int j = 0; j < tile.mountains.Count; j++)
            {
                TerrainObjectData mountain = tile.mountains[j];
                Instantiate(mountainSet.mountains[mountain.typeIndex], mountain.Position, Quaternion.Euler(mountain.Rotation), tileGameObject.transform);
            }
            for (int j = 0; j < tile.boulders.Count; j++)
            {
                TerrainObjectData boulder = tile.boulders[j];
                Instantiate(mountainSet.boulders[boulder.typeIndex], boulder.Position, Quaternion.Euler(boulder.Rotation), tileGameObject.transform);
            }
            // set to be covered in snow if below freezing
            if (tile.temperatureValue < settings.freezingTemperature)
            {
                MeshRenderer[] meshRends = tileGameObject.GetComponentsInChildren<MeshRenderer>();
                for (int j = 0; j < meshRends.Length; j++)
                {
                    meshRends[j].sharedMaterial = snowMaterial;
                }
            }
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

    //private void OnDrawGizmos()
    //{
    //    foreach (var tile in terrainData.tiles)
    //    {
    //        Gizmos.color = new Color(tile.temperatureValue, 0, 1 - tile.temperatureValue);
    //        Gizmos.DrawSphere(tile.Position + new Vector3(50, 0, 50), 10f);
    //    }
    //}
}
