using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastConstructor : MonoBehaviour
{
    public TerrainTileSet tileSet;

    private float tileSize;
    private GameObject[,] modifiedTiles;

    public GameObject[,] ReplaceCoastalTiles(GameObject[,] generatedTiles, float tileSize)
    {
        this.tileSize = tileSize;
        modifiedTiles = generatedTiles;
        // first get rid of islands/peninsulas
        for (int x = 0; x < modifiedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < modifiedTiles.GetLength(1); z++)
            {
                List<GameObject> edgeAdjacentOcean = GetEdgeAdjacentOceanTiles(x, z);
                if (edgeAdjacentOcean.Count == 2)
                {
                    Vector3 directionOne = edgeAdjacentOcean[0].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                    directionOne = directionOne.normalized;
                    Vector3 directionTwo = edgeAdjacentOcean[1].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                    directionTwo = directionTwo.normalized;
                    if (Vector3.Angle(directionOne, directionTwo) > 90)
                    {
                        TurnIntoOceanTile(x, z);
                    }
                }
                else if (edgeAdjacentOcean.Count > 2)
                {
                    TurnIntoOceanTile(x, z);
                }

            }
        }
        for (int x = 0; x < modifiedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < modifiedTiles.GetLength(1); z++)
            {
                List<GameObject> edgeAdjacentOcean = GetEdgeAdjacentOceanTiles(x, z);
                if (!IsOceanTile(x, z) && edgeAdjacentOcean.Count > 0)
                {
                    if (edgeAdjacentOcean.Count == 1) // coastal straight
                    {
                        Vector3 direction = edgeAdjacentOcean[0].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                        direction = direction.normalized;
                        DestroyImmediate(modifiedTiles[x, z]);
                        int randomIndex = Random.Range(0, tileSet.coastalStraight.Length);
                        modifiedTiles[x, z] = Instantiate(tileSet.coastalStraight[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                        if (direction == -transform.forward)
                        {
                            modifiedTiles[x, z].transform.Rotate(0, 180, 0);
                            modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                        }
                        else if (direction == transform.right)
                        {
                            modifiedTiles[x, z].transform.Rotate(0, 90, 0);
                            modifiedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                        }
                        else if (direction == -transform.right)
                        {
                            modifiedTiles[x, z].transform.Rotate(0, 270, 0);
                            modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                        }
                    }
                    else if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) <= 90) // coastal outer corner
                        {
                            DestroyImmediate(modifiedTiles[x, z]);
                            int randomIndex = Random.Range(0, tileSet.coastalOuterCorner.Length);
                            modifiedTiles[x, z] = Instantiate(tileSet.coastalOuterCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                            if (directionOne == transform.forward && directionTwo == -transform.right || directionTwo == transform.forward && directionOne == -transform.right)
                            {
                                modifiedTiles[x, z].transform.Rotate(0, -90, 0);
                                modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                            }
                            else if (directionOne == transform.right && directionTwo == -transform.forward || directionTwo == transform.right && directionOne == -transform.forward)
                            {
                                modifiedTiles[x, z].transform.Rotate(0, 90, 0);
                                modifiedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                            }
                            else if (directionOne == -transform.forward && directionTwo == -transform.right || directionTwo == -transform.forward && directionOne == -transform.right)
                            {
                                modifiedTiles[x, z].transform.Rotate(0, 180, 0);
                                modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                            }
                        }
                    }
                }
                List<GameObject> cornerAdjacentOcean = GetCornerAdjacentOceanTiles(x, z);
                if (cornerAdjacentOcean.Count == 1 && edgeAdjacentOcean.Count == 0) // coastal inner corner
                {
                    Vector3 direction = cornerAdjacentOcean[0].transform.localPosition - modifiedTiles[x, z].transform.localPosition;
                    direction = direction.normalized;
                    DestroyImmediate(modifiedTiles[x, z]);
                    int randomIndex = Random.Range(0, tileSet.coastalInnerCorner.Length);
                    modifiedTiles[x, z] = Instantiate(tileSet.coastalInnerCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
                    if (angle > 90 && angle <= 180)
                    {
                        modifiedTiles[x, z].transform.Rotate(0, -90, 0);
                        modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                    }
                    else if (angle < -90 && angle >= -180)
                    {
                        modifiedTiles[x, z].transform.Rotate(0, 180, 0);
                        modifiedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                    }
                    else if (angle < 0 && angle >= -90)
                    {
                        modifiedTiles[x, z].transform.Rotate(0, 90, 0);
                        modifiedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                    }
                }
            }
        }
        return modifiedTiles;
    }

    private void TurnIntoOceanTile(int x, int z)
    {
        DestroyImmediate(modifiedTiles[x, z]);
        int randomTileIndex = Random.Range(0, tileSet.oceanTiles.Length);
        GameObject newTile = Instantiate(tileSet.oceanTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        modifiedTiles[x, z] = newTile;
    }

    public List<GameObject> GetEdgeAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            oceanTiles.Add(modifiedTiles[x - 1, z]);
        }
        if (x + 1 < modifiedTiles.GetLength(0) && IsOceanTile(x + 1, z))
        {
            oceanTiles.Add(modifiedTiles[x + 1, z]);
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            oceanTiles.Add(modifiedTiles[x, z - 1]);
        }
        if (z + 1 < modifiedTiles.GetLength(1) && IsOceanTile(x, z + 1))
        {
            oceanTiles.Add(modifiedTiles[x, z + 1]);
        }
        return oceanTiles;
    }

    public List<GameObject> GetCornerAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && z - 1 >= 0 && IsOceanTile(x - 1, z - 1))
        {
            oceanTiles.Add(modifiedTiles[x - 1, z - 1]);
        }
        if (x + 1 < modifiedTiles.GetLength(0) && z + 1 < modifiedTiles.GetLength(1) && IsOceanTile(x + 1, z + 1))
        {
            oceanTiles.Add(modifiedTiles[x + 1, z + 1]);
        }
        if (x - 1 >= 0 && z + 1 < modifiedTiles.GetLength(1) && IsOceanTile(x - 1, z + 1))
        {
            oceanTiles.Add(modifiedTiles[x - 1, z + 1]);
        }
        if (x + 1 < modifiedTiles.GetLength(1) && z - 1 >= 0 && IsOceanTile(x + 1, z - 1))
        {
            oceanTiles.Add(modifiedTiles[x + 1, z - 1]);
        }
        return oceanTiles;
    }

    public bool IsOceanTile(int x, int z)
    {
        return modifiedTiles[x, z].CompareTag("Terrain/OceanFloor");
    }
}
