using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int xPos;
    public int zPos;

    private GameObject gameObject;
    public Transform Transform => gameObject.transform;
    public TileType type;
    public float noiseValue = 0;

    public TileData(int xPos, int zPos)
    {
        this.xPos = xPos;
        this.zPos = zPos;
        type = TileType.OceanFloor;
    }

    public void ReplaceTile(GameObject newTile, TileType type)
    {
        Object.DestroyImmediate(gameObject);
        gameObject = newTile;
        this.type = type;
    }

    public bool Equals(TileData other)
    {
        if (other.gameObject == gameObject && other.type == type)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
