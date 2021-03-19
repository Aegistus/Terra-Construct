using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainTileSet", menuName = "Terrain Tile Set", order = 1)]
public class TerrainTileSet : ScriptableObject
{
    public GameObject[] flatTiles;
    public GameObject[] mountainTiles;
}
