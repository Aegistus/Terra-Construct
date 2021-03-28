using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TreeData
{
    public bool Active { get; private set; }
    public int typeIndex;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    private GameObject currentTreeObject;

    public TreeData(int typeIndex, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.typeIndex = typeIndex;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public void Activate(GameObject treeObject)
    {
        Active = true;
        currentTreeObject = treeObject;
    }

    public void Deactivate()
    {
        Active = false;
        if (currentTreeObject)
        {
            currentTreeObject.SetActive(false);
            currentTreeObject = null;
        }
    }
}
