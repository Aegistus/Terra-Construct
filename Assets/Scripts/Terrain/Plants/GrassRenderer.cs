using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GrassRenderer : MonoBehaviour
{
    public GrassSet grassSet;
    public float foregroundRenderDistance = 40f;
    //public float backgroundRenderDistance = 100f;
    public int grassPoolSize = 500;
    private Camera mainCam;

    private TerrainConstructor terrain;
    private List<Queue<GameObject>> grassPool;

    private void Start()
    {
        mainCam = Camera.main;
        if (grassPool == null)
        {
            CreateGrassPool();
        }
        terrain = FindObjectOfType<TerrainConstructor>();
        CheckGrass();
        StartCoroutine(CheckPlayerPosition());
    }

    public IEnumerator CheckPlayerPosition()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            CheckGrass();
        }
    }

    public void CreateGrassPool()
    {
        ClearGrassPool();
        grassPool = new List<Queue<GameObject>>();
        for (int i = 0; i < grassSet.common.Length; i++)
        {
            Queue<GameObject> treeQueue = new Queue<GameObject>();
            for (int j = 0; j < grassPoolSize / grassSet.common.Length; j++)
            {
                GameObject tree = Instantiate(grassSet.common[i], transform);
                tree.SetActive(false);
                treeQueue.Enqueue(tree);
            }
            grassPool.Add(treeQueue);
        }
    }

    public void ClearGrassPool()
    {
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        grassPool = null;
    }

    public void UpdateGrassAroundEditorCamera()
    {
        if (mainCam == null && !Application.isPlaying)
        {
            mainCam = SceneView.GetAllSceneCameras()[0];
        }
        terrain = FindObjectOfType<TerrainConstructor>();
        CheckGrass();
    }

    private void CheckGrass()
    {
        if (mainCam == null && Application.isPlaying)
        {
            mainCam = Camera.main;
        }
        foreach (var grass in terrain.terrainData.grass)
        {
            if (!grass.Active)
            {
                if (Vector3.Distance(mainCam.transform.position, grass.Position) <= foregroundRenderDistance)
                {
                    GameObject grassGameObject = grassPool[grass.typeIndex].Dequeue();
                    grassGameObject.transform.position = grass.Position;
                    grassGameObject.transform.eulerAngles = grass.Rotation;
                    grassGameObject.transform.localScale = grass.Scale;
                    grassGameObject.SetActive(true);
                    grass.Activate(grassGameObject);
                    grassPool[grass.typeIndex].Enqueue(grassGameObject);
                }
            }
            else
            {
                if (Vector3.Distance(mainCam.transform.position, grass.Position) > foregroundRenderDistance)
                {
                    grass.Deactivate();
                }
            }
        }
    }
}
