using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TreeRenderer : MonoBehaviour
{
    public TreeSet treePrefabs;
    public float foregroundRenderDistance = 40f;
    public float backgroundRenderDistance = 100f;
    public int treePoolSize = 500;
    private Camera mainCam;

    private TerrainConstructor terrain;
    private List<Queue<GameObject>> treePool;

    private void Start()
    {
        mainCam = Camera.main;
        if (treePool == null)
        {
            CreateTreePool();
        }
        terrain = FindObjectOfType<TerrainConstructor>();
        CheckTrees();
        StartCoroutine(CheckPlayerPosition());
    }

    public IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CheckTrees();
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
        if (mainCam == null && Application.isPlaying)
        {
            mainCam = Camera.main;
        }
        foreach (var tree in terrain.terrainData.trees)
        {
            if (!tree.Active)
            {
                if (Vector3.Distance(mainCam.transform.position, tree.Position) <= foregroundRenderDistance && treePool.Count > 0)
                {
                    GameObject treeGameObject = treePool[tree.typeIndex].Dequeue();
                    if (Vector3.Distance(mainCam.transform.position, treeGameObject.transform.position) > foregroundRenderDistance)
                    {
                        treeGameObject.transform.position = tree.Position;
                        treeGameObject.transform.eulerAngles = tree.Rotation;
                        treeGameObject.transform.localScale = tree.Scale;
                        treeGameObject.SetActive(true);
                        tree.Activate(treeGameObject);
                        treePool[tree.typeIndex].Enqueue(treeGameObject);
                    }
                    else
                    {
                        treePool[tree.typeIndex].Enqueue(treeGameObject);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(mainCam.transform.position, tree.Position) > foregroundRenderDistance)
                {
                    tree.Deactivate();
                }
            }
        }
    }



}
