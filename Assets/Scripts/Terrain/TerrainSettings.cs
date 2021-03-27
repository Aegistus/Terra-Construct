using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainSettings", menuName = "Terrain Settings", order = 1)]
public class TerrainSettings : ScriptableObject
{
    public int xSize = 1000;
    public int zSize = 1000;
}
