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

    private ForestPlacer forestPlacer;
    private TreeSet treePrefabs;
    private List<Queue<GameObject>> treePool;

    private void Start()
    {
        mainCam = Camera.main;
        forestPlacer = GetComponent<ForestPlacer>();
        treePrefabs = forestPlacer.treeSet;
        if (treePool == null)
        {
            CreateTreePool();
        }
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
        treePrefabs = GetComponent<ForestPlacer>().treeSet;
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
        CheckTrees();
    }

    private void CheckTrees()
    {
        if (mainCam == null && Application.isPlaying)
        {
            mainCam = Camera.main;
        }
        foreach (var tree in forestPlacer.placedTrees)
        {
            if (!tree.Active)
            {
                if (Vector3.Distance(mainCam.transform.position, tree.position) <= foregroundRenderDistance)
                {
                    GameObject treeGameObject = treePool[tree.typeIndex].Dequeue();
                    treeGameObject.transform.position = tree.position;
                    treeGameObject.transform.eulerAngles = tree.rotation;
                    treeGameObject.transform.localScale = tree.scale;
                    treeGameObject.SetActive(true);
                    tree.Activate(treeGameObject);
                    treePool[tree.typeIndex].Enqueue(treeGameObject);
                }
            }
            else
            {
                if (Vector3.Distance(mainCam.transform.position, tree.position) > foregroundRenderDistance)
                {
                    tree.Deactivate();
                }
            }
        }
    }



}
