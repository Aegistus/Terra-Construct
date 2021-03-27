using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    private GameObject gameObject;
    public Transform Transform => gameObject.transform;
    public TileType type;
    public readonly float noiseValue = 0;

    public TileData(GameObject gameObject, TileType type, float noiseValue)
    {
        this.gameObject = gameObject;
        this.type = type;
        this.noiseValue = noiseValue;
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
