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
        terrainData = MountainConstructor.GenerateMountains(terrainData, settings, mountainSet);
        terrainData = MountainConstructor.GenerateFoothills(terrainData, settings, mountainSet);
        terrainData = FloraGenerator.Generate(terrainData, settings, treeSet, grassSet);
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
                    case TileType.RiverMouth: tileGameObject = tileSet.riverMouth[0]; break;
                    case TileType.RiverStraight: tileGameObject = tileSet.riverStraight[0]; break;
                    case TileType.RiverBendRight: tileGameObject = tileSet.riverCornerRight[0]; break;
                    case TileType.RiverBendLeft: tileGameObject = tileSet.riverCornerLeft[0]; break;
                    case TileType.RiverEnd: tileGameObject = tileSet.riverEnd[0]; break;
                    default: tileGameObject = tileSet.landTiles[0]; break;
                }
                Instantiate(tileGameObject, tile.Position, Quaternion.Euler(tile.Rotation), transform);
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
}
