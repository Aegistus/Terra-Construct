using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileList
{
    public List<TileData> tiles = new List<TileData>();
    public int Count => tiles.Count;

    public void Add(TileData data)
    {
        tiles.Add(data);
    }

    public TileData Get(int i)
    {
        return tiles[i];
    }
}
