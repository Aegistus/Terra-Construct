using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainTileSet", menuName = "Terrain Tile Set", order = 2)]
public class TerrainTileSet : ScriptableObject
{
    public GameObject[] landTiles;
    public GameObject[] oceanTiles;
    public GameObject[] coastalStraight;
    public GameObject[] coastalInnerCorner;
    public GameObject[] coastalOuterCorner;
}
