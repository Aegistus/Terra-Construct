using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChunk
{
    public List<GameObject> objectsInsideChunk = new List<GameObject>();
    public readonly GameObject gameObject;
    public readonly Transform transform;

    public TerrainChunk(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
    }


}
