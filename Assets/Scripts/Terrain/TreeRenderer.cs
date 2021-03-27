using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TreeRenderer : MonoBehaviour
{
    public float foregroundRenderDistance = 40f;
    public float backgroundRenderDistance = 100f;
    public int treePoolSize = 100;
    private Camera mainCam;
    private Camera editorCam;

    private TerrainConstructor terrain;
    private TerrainData Data => terrain.terrainData;
    private TreeSet treePrefabs;
    private List<Queue<GameObject>> treePool;

    private void Start()
    {
        if (mainCam == null)
        {
            mainCam = Camera.main;
        }
        terrain = GetComponent<TerrainConstructor>();
        treePrefabs = FindObjectOfType<ForestPlacer>().trees;
        StartCoroutine(CheckPlayerPosition());
        CreateTreePool();
    }

    public IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UpdateTreesAroundPlayer();
        }
    }

    public void CreateTreePool()
    {
        ClearTreePool();
        treePool = new List<Queue<GameObject>>();
        for (int i = 0; i < treePrefabs.commonTrees.Count; i++)
        {
            Queue<GameObject> treeQueue = new Queue<GameObject>();
            for (int j = 0; j < treePoolSize; j++)
            {
                GameObject tree = Instantiate(treePrefabs.commonTrees[i], transform);
                tree.SetActive(false);
                treeQueue.Enqueue(tree);
            }
            treePool.Add(treeQueue);
        }
    }

    public void ClearTreePool()
    {
        if (treePool == null)
        {
            return;
        }
        for (int i = 0; i < treePool.Count; i++)
        {
            while (treePool[i].Count > 0)
            {
                DestroyImmediate(treePool[i].Dequeue());
            }
        }
        treePool = null;
    }

    public void UpdateTreesAroundPlayer()
    {
        if (mainCam == null && Application.isPlaying)
        {
            mainCam = Camera.main;
        }
        foreach (var tree in Data.Trees)
        {
            if (!tree.Active)
            {
                if ((tree.position - mainCam.transform.position).sqrMagnitude <= Mathf.Pow(foregroundRenderDistance, 2))
                {
                    if (treePool[tree.typeIndex].Count > 0)
                    {
                        tree.Activate(treePool[tree.typeIndex].Dequeue());
                    }
                }
            }
            else
            {
                if ((tree.position - mainCam.transform.position).sqrMagnitude > Mathf.Pow(foregroundRenderDistance, 2))
                {
                    treePool[tree.typeIndex].Enqueue(tree.Deactivate());
                }
            }
        }
    }

    public void UpdateTreesAroundEditorCamera()
    {
        if (editorCam == null && !Application.isPlaying)
        {
            editorCam = SceneView.GetAllSceneCameras()[0];
        }
        terrain = GetComponent<TerrainConstructor>();
        foreach (var tree in Data.Trees)
        {
            if (!tree.Active)
            {
                if ((tree.position - editorCam.transform.position).sqrMagnitude <= Mathf.Pow(foregroundRenderDistance, 2))
                {
                    if (treePool[tree.typeIndex].Count > 0)
                    {
                        tree.Activate(treePool[tree.typeIndex].Dequeue());
                    }
                }
            }
            else
            {
                if ((tree.position - editorCam.transform.position).sqrMagnitude > Mathf.Pow(foregroundRenderDistance, 2))
                {
                    treePool[tree.typeIndex].Enqueue(tree.Deactivate());
                }
            }
        }
    }
}
