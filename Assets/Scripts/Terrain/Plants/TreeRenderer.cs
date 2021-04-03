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
    private List<Queue<GameObject>> commonTreePool;
    private List<Queue<GameObject>> rareTreePool;

    private void Start()
    {
        mainCam = Camera.main;
        if (commonTreePool == null)
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
        commonTreePool = new List<Queue<GameObject>>();
        for (int i = 0; i < treePrefabs.commonTrees.Count; i++)
        {
            Queue<GameObject> treeQueue = new Queue<GameObject>();
            for (int j = 0; j < treePoolSize / treePrefabs.commonTrees.Count; j++)
            {
                GameObject tree = Instantiate(treePrefabs.commonTrees[i], transform);
                tree.SetActive(false);
                treeQueue.Enqueue(tree);
            }
            commonTreePool.Add(treeQueue);
        }
        rareTreePool = new List<Queue<GameObject>>();
        for (int i = 0; i < treePrefabs.rareTrees.Count; i++)
        {
            Queue<GameObject> rareTreeQueue = new Queue<GameObject>();
            for (int j = 0; j < treePoolSize / treePrefabs.rareTrees.Count; j++)
            {
                GameObject rareTree = Instantiate(treePrefabs.rareTrees[i], transform);
                rareTree.SetActive(false);
                rareTreeQueue.Enqueue(rareTree);
            }
            rareTreePool.Add(rareTreeQueue);
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
        commonTreePool = null;
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
                if (Vector3.Distance(mainCam.transform.position, tree.Position) <= foregroundRenderDistance && commonTreePool.Count > 0)
                {
                    GameObject treeGameObject = commonTreePool[tree.typeIndex].Dequeue();
                    if (Vector3.Distance(mainCam.transform.position, treeGameObject.transform.position) > foregroundRenderDistance)
                    {
                        treeGameObject.transform.position = tree.Position;
                        treeGameObject.transform.eulerAngles = tree.Rotation;
                        treeGameObject.transform.localScale = tree.Scale;
                        treeGameObject.SetActive(true);
                        tree.Activate(treeGameObject);
                        commonTreePool[tree.typeIndex].Enqueue(treeGameObject);
                    }
                    else
                    {
                        commonTreePool[tree.typeIndex].Enqueue(treeGameObject);
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
        foreach (var rareTree in terrain.terrainData.rareTrees)
        {
            if (!rareTree.Active)
            {
                if (Vector3.Distance(mainCam.transform.position, rareTree.Position) <= foregroundRenderDistance && rareTreePool.Count > 0)
                {
                    GameObject treeGameObject = rareTreePool[rareTree.typeIndex].Dequeue();
                    if (Vector3.Distance(mainCam.transform.position, treeGameObject.transform.position) > foregroundRenderDistance)
                    {
                        treeGameObject.transform.position = rareTree.Position;
                        treeGameObject.transform.eulerAngles = rareTree.Rotation;
                        treeGameObject.transform.localScale = rareTree.Scale;
                        treeGameObject.SetActive(true);
                        rareTree.Activate(treeGameObject);
                        rareTreePool[rareTree.typeIndex].Enqueue(treeGameObject);
                    }
                    else
                    {
                        rareTreePool[rareTree.typeIndex].Enqueue(treeGameObject);
                    }
                }
            }
            else
            {
                if (Vector3.Distance(mainCam.transform.position, rareTree.Position) > foregroundRenderDistance)
                {
                    rareTree.Deactivate();
                }
            }
        }
    }



}
