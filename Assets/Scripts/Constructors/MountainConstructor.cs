using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainConstructor : MonoBehaviour
{
    public MountainSet mountains;
    [Range(0f, 1f)]
    public float mountainLevel = .5f;
    public float mountainSpacing = 10f;
    public float mountainSizeModifier = 1f;
    public int maxMountainsPerTile = 5;

    private TerrainConstructor terrain;
    private TerrainData TerrainData => terrain.terrainData;

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
                        int randIndex = Random.Range(0, mountains.largeMountains.Length);
                        Vector3 randomPosition = new Vector3(Random.Range(0, terrain.tileSize), transform.position.y, Random.Range(0, terrain.tileSize));
                        Instantiate(mountains.largeMountains[randIndex], randomPosition + tile.Transform.position, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
