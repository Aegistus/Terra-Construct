using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData
{
    private GameObject gameObject;
    public Transform Transform => gameObject.transform;
    public TileType type;

    public TileData(GameObject gameObject, TileType type)
    {
        this.gameObject = gameObject;
        this.type = type;
    }

    public void ReplaceTile(GameObject newTile, TileType type)
    {
        Object.DestroyImmediate(gameObject);
        gameObject = newTile;
        this.type = type;
    }

}
