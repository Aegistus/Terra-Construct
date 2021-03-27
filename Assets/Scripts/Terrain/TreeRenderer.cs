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
        terrain = FindObjectOfType<TerrainConstructor>();
        treePrefabs = FindObjectOfType<ForestPlacer>().trees;
        if (treePool == null)
        {
            CreateTreePool();
        }
        StartCoroutine(CheckPlayerPosition());
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
            for (int j = 0; j < treePoolSize / treePrefabs.commonTrees.Count; j++)
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
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
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
        CheckTrees();
    }

    public void UpdateTreesAroundEditorCamera()
    {
        if (mainCam == null && !Application.isPlaying)
        {
            mainCam = SceneView.GetAllSceneCameras()[0];
        }
        terrain = FindObjectOfType<TerrainConstructor>();
        CheckTrees();
    }

    private void CheckTrees()
    {
        foreach (var tree in Data.Trees)
        {
            if (!tree.Active)
            {
                if ((tree.position - mainCam.transform.position).sqrMagnitude <= Mathf.Pow(foregroundRenderDistance, 2))
                {
                    if (treePool[tree.typeIndex].Count > 0)
                    {
                        GameObject treeGameObject = treePool[tree.typeIndex].Dequeue();
                        treeGameObject.transform.position = tree.position;
                        treeGameObject.transform.eulerAngles = tree.rotation;
                        treeGameObject.transform.localScale = tree.scale;
                        treeGameObject.SetActive(true);
                        tree.Activate();
                        treePool[tree.typeIndex].Enqueue(treeGameObject);
                    }
                }
            }
            else
            {
                if ((tree.position - mainCam.transform.position).sqrMagnitude > Mathf.Pow(foregroundRenderDistance, 2))
                {
                    tree.Deactivate();
                }
            }
        }
    }
}
