﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeData
{
    public bool Active { get; private set; }
    public int typeIndex;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    private GameObject currentGameObject;

    public TreeData(int typeIndex, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.typeIndex = typeIndex;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }

    public void Activate(GameObject treeGameObj)
    {
        currentGameObject = treeGameObj;
        currentGameObject.transform.position = position;
        currentGameObject.transform.eulerAngles = rotation;
        currentGameObject.transform.localScale = scale;
        currentGameObject.SetActive(true);
        Active = true;
    }

    public GameObject Deactivate()
    {
        Active = false;
        currentGameObject.SetActive(false);
        return currentGameObject;
    }
}
