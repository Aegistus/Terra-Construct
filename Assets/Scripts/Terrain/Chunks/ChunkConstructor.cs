using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkConstructor : MonoBehaviour
{
    public int chunkSize = 3;

    public TerrainChunk[,] chunks;

    private TerrainConstructor terrain;
    private TerrainData Data => terrain.terrainData;

    public void ArrangeChunks()
    {
        terrain = GetComponent<TerrainConstructor>();
        chunks = new TerrainChunk[terrain.XTileTotal / chunkSize + 1, terrain.ZTileTotal / chunkSize + 1];
        for (int x = 0; x < chunks.GetLength(0); x++)
        {
            for (int z = 0; z < chunks.GetLength(1); z++)
            {
                GameObject chunkObject = new GameObject("Chunk");
                chunks[x,z] = new TerrainChunk(chunkObject);
            }
        }
        for (int x = 0; x < Data.xSize; x++)
        {
            for (int z = 0; z < Data.zSize; z++)
            {
                chunks[x / terrain.XTileTotal, z / terrain.ZTileTotal].baseTiles.Add(Data.GetTileAtCoordinates(x, z).Transform.gameObject);
            }
        }
    }

    public void CombineTerrainMeshes()
    {
        foreach (var chunk in chunks)
        {
            chunk.CombineTileMeshes();
        }
    }

    public void GenerateChunks()
    {
        foreach (var chunk in chunks)
        {
            chunk.SetupNewLODs();
        }
    }
}
