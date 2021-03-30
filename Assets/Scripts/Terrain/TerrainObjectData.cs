using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainObjectData
{
    public bool Active { get; private set; }
    public int typeIndex;

    public Vector3 Position => new Vector3(xPos, yPos, zPos);
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    [SerializeField] private float zPos;

    public Vector3 Rotation => new Vector3(xRot, yRot, zRot);
    [SerializeField] private float xRot;
    [SerializeField] private float yRot;
    [SerializeField] private float zRot;

    public Vector3 Scale => new Vector3(xScale, yScale, zScale);
    [SerializeField] private float xScale;
    [SerializeField] private float yScale;
    [SerializeField] private float zScale;

    private GameObject currentObject;

    public TerrainObjectData(int typeIndex, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.typeIndex = typeIndex;
        SetPosition(position);
        SetRotation(rotation);
        SetScale(scale);
    }

    public void Activate(GameObject terrainObject)
    {
        Active = true;
        currentObject = terrainObject;
    }

    public void Deactivate()
    {
        Active = false;
        if (currentObject)
        {
            currentObject.SetActive(false);
            currentObject = null;
        }
    }

    public void SetPosition(Vector3 position)
    {
        xPos = position.x;
        yPos = position.y;
        zPos = position.z;
    }

    public void SetRotation(Vector3 rotation)
    {
        xRot = rotation.x;
        yRot = rotation.y;
        zRot = rotation.z;
    }

    public void SetScale(Vector3 scale)
    {
        xScale = scale.x;
        yScale = scale.y;
        zScale = scale.z;
    }
}
