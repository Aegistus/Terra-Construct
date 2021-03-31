using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int xCoordinate;
    public int zCoordinate;

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

    public TileType type;
    public float noiseValue = 0;

    public TileData(int xCoordinate, int zCoordinate)
    {
        this.xCoordinate = xCoordinate;
        this.zCoordinate = zCoordinate;
        type = TileType.OceanFloor;
        SetScale(Vector3.one);
        SetPosition(Vector3.zero);
        SetRotation(Vector3.zero);
    }

    public void SetPosition(Vector3 position)
    {
        xPos = position.x;
        yPos = position.y;
        zPos = position.z;
    }

    public void AddPosition(Vector3 position)
    {
        xPos += position.x;
        yPos += position.y;
        zPos += position.z;
    }

    public void AddPosition(float x, float y, float z)
    {
        xPos += x;
        yPos += y;
        zPos += z;
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

    public void ReplaceTile(TileType type, Vector3 position, Vector3 rotation)
    {
        this.type = type;
        SetPosition(position);
        SetRotation(rotation);
    }

    public void Rotate(float x, float y, float z)
    {
        xRot += x;
        yRot += y;
        zRot += z;
    }

    public bool Equals(TileData other)
    {
        if (other.type == type && other.xCoordinate == xCoordinate && other.zCoordinate == zCoordinate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
