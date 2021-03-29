using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int xPos;
    public int zPos;

    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    public TileType type;
    public float noiseValue = 0;

    public TileData(int xPos, int zPos)
    {
        this.xPos = xPos;
        this.zPos = zPos;
        type = TileType.OceanFloor;
        scale = Vector3.one;
    }

    public void ReplaceTile(TileType type, Vector3 position, Vector3 rotation)
    {
        this.type = type;
        this.position = position;
        this.rotation = rotation;
    }

    public void Rotate(float x, float y, float z)
    {
        rotation += new Vector3(x, y, z);
    }

    public bool Equals(TileData other)
    {
        if (other.type == type && other.xPos == xPos && other.zPos == zPos)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
