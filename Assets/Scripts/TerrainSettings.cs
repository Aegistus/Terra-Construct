using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTerrainSettings", menuName = "Terrain Settings", order = 1)]
public class TerrainSettings : ScriptableObject
{
    public float xSize = 1000f;
    public float zSize = 1000f;
}
