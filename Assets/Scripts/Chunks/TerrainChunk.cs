using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TerrainChunk
{
    public List<GameObject> objectsInsideChunk = new List<GameObject>();
    public List<GameObject> baseTiles = new List<GameObject>();
    public readonly GameObject gameObject;
    public readonly Transform transform;

    private Mesh[] mergedMeshes;
    private LODGroup mergedLodGroup;

    private GameObject[] meshChildren;

    public TerrainChunk(GameObject gameObject)
    {
        this.gameObject = gameObject;
        transform = gameObject.transform;
    }

    public void CombineTileMeshes()
    {
        foreach (var tile in baseTiles)
        {
            tile.transform.parent = transform;
        }
        List<LODGroup> lodGroups = gameObject.GetComponentsInChildren<LODGroup>(true).ToList();
        
        // Get all of the lod renderers for each lod level
        List<Renderer> lod0Renderers = new List<Renderer>();
        foreach (var lodGroup in lodGroups)
        {
            LOD[] lods = lodGroup.GetLODs();
            lod0Renderers.AddRange(lods[0].renderers);
        }
        List<Renderer> lod1Renderers = new List<Renderer>();
        foreach (var lodGroup in lodGroups)
        {
            LOD[] lods = lodGroup.GetLODs();
            lod0Renderers.AddRange(lods[1].renderers);
        }
        List<Renderer> lod2Renderers = new List<Renderer>();
        foreach (var lodGroup in lodGroups)
        {
            LOD[] lods = lodGroup.GetLODs();
            lod0Renderers.AddRange(lods[2].renderers);
        }

        // Merge LOD 0
        foreach (var renderer in lod1Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        foreach (var renderer in lod2Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        mergedMeshes[0] = MeshCombiner.CombineMeshesOfChildren(gameObject);

        // Merge LOD 1
        foreach (var renderer in lod0Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        foreach (var renderer in lod1Renderers)
        {
            renderer.gameObject.SetActive(true);
        }
        foreach (var renderer in lod2Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        mergedMeshes[1] = MeshCombiner.CombineMeshesOfChildren(gameObject);

        // Merge LOD 2
        foreach (var renderer in lod0Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        foreach (var renderer in lod1Renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        foreach (var renderer in lod2Renderers)
        {
            renderer.gameObject.SetActive(true);
        }
        mergedMeshes[2] = MeshCombiner.CombineMeshesOfChildren(gameObject);
    }

    public void SetupNewLODs()
    {
        if (mergedLodGroup == null)
        {
            mergedLodGroup = gameObject.AddComponent<LODGroup>();
        }
        meshChildren = new GameObject[3];
        LOD[] newLods = new LOD[3];
        for (int i = 0; i < meshChildren.Length; i++)
        {
            meshChildren[i] = new GameObject("LOD " + i);
            meshChildren[i].transform.parent = transform;
            meshChildren[i].AddComponent<MeshFilter>().mesh = mergedMeshes[i];
            newLods[i] = new LOD()
            {
                renderers = new Renderer[] { meshChildren[i].GetComponent<Renderer>() }
            };
        }
        mergedLodGroup.SetLODs(newLods);
    }

}
