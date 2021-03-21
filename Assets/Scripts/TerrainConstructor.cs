using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainConstructor : MonoBehaviour
{
    public TerrainSettings settings;
    public TerrainTileSet tileSet;
    public MountainSet mountainSet;
    public NoiseMap elevationNoiseMap;

    [Header("Tile Settings")]
    public int tileSize = 100;

    [Header("Ocean Settings")]
    [Range(0f, 1f)]
    public float oceanPercent = .4f;

    private int xTileTotal;
    private int zTileTotal;
    private GameObject[,] generatedTiles;

    private void Start()
    {

        //StartCoroutine(PlaceMountains());
    }

    public void ConstructTerrain()
    {
        xTileTotal = settings.xSize / tileSize;
        zTileTotal = settings.zSize / tileSize;
        elevationNoiseMap.XRandomOffset = Random.Range(0, 10000);
        elevationNoiseMap.ZRandomOffset = Random.Range(0, 10000);
        ClearTerrain();
        if (Application.isPlaying)
        {
            StartCoroutine(PlaceTileGrid());
        }
        else
        {
            PlaceTileGridEditor();
        }
    }

    public IEnumerator PlaceTileGrid()
    {
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                PlaceTile(x, z);
                yield return null;
            }
        }
        ReplaceCoastalTiles();
    }

    public void PlaceTileGridEditor()
    {
        generatedTiles = new GameObject[xTileTotal, zTileTotal];
        for (int x = 0; x < xTileTotal; x++)
        {
            for (int z = 0; z < zTileTotal; z++)
            {
                PlaceTile(x, z);
            }
        }
        ReplaceCoastalTiles();
    }

    public void PlaceTile(int x, int z)
    {
        if (elevationNoiseMap.GetLayeredPerlinValueAtPosition(x, z) < oceanPercent)
        {
            int randomTileIndex = Random.Range(0, tileSet.oceanTiles.Length);
            GameObject newTile = Instantiate(tileSet.oceanTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            generatedTiles[x, z] = newTile;
        }
        else
        {
            int randomTileIndex = Random.Range(0, tileSet.landTiles.Length);
            GameObject newTile = Instantiate(tileSet.landTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
            generatedTiles[x, z] = newTile;
        }
        //print(elevationNoiseMap.GetLayeredPerlinValueAtPosition(x * tileSize, z * tileSize));
    }

    public void ReplaceCoastalTiles()
    {
        // first get rid of islands/peninsulas
        for (int x = 0; x < generatedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < generatedTiles.GetLength(1); z++)
            {
                List<GameObject> edgeAdjacentOcean = GetEdgeAdjacentOceanTiles(x, z);
                if (edgeAdjacentOcean.Count == 2)
                {
                    Vector3 directionOne = edgeAdjacentOcean[0].transform.localPosition - generatedTiles[x, z].transform.localPosition;
                    directionOne = directionOne.normalized;
                    Vector3 directionTwo = edgeAdjacentOcean[1].transform.localPosition - generatedTiles[x, z].transform.localPosition;
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
        for (int x = 0; x < generatedTiles.GetLength(0); x++)
        {
            for (int z = 0; z < generatedTiles.GetLength(1); z++)
            {
                List<GameObject> edgeAdjacentOcean = GetEdgeAdjacentOceanTiles(x, z);
                if (!IsOceanTile(x, z) && edgeAdjacentOcean.Count > 0)
                {
                    if (edgeAdjacentOcean.Count == 1) // coastal straight
                    {
                        Vector3 direction = edgeAdjacentOcean[0].transform.localPosition - generatedTiles[x, z].transform.localPosition;
                        direction = direction.normalized;
                        DestroyImmediate(generatedTiles[x, z]);
                        int randomIndex = Random.Range(0, tileSet.coastalStraight.Length);
                        generatedTiles[x, z] = Instantiate(tileSet.coastalStraight[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                        if (direction == -transform.forward)
                        {
                            generatedTiles[x, z].transform.Rotate(0, 180, 0);
                            generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                        }
                        else if (direction == transform.right)
                        {
                            generatedTiles[x, z].transform.Rotate(0, 90, 0);
                            generatedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                        }
                        else if (direction == -transform.right)
                        {
                            generatedTiles[x, z].transform.Rotate(0, 270, 0);
                            generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                        }
                    }
                    else if (edgeAdjacentOcean.Count == 2)
                    {
                        Vector3 directionOne = edgeAdjacentOcean[0].transform.localPosition - generatedTiles[x, z].transform.localPosition;
                        directionOne = directionOne.normalized;
                        Vector3 directionTwo = edgeAdjacentOcean[1].transform.localPosition - generatedTiles[x, z].transform.localPosition;
                        directionTwo = directionTwo.normalized;
                        if (Vector3.Angle(directionOne, directionTwo) <= 90) // coastal outer corner
                        {
                            DestroyImmediate(generatedTiles[x, z]);
                            int randomIndex = Random.Range(0, tileSet.coastalOuterCorner.Length);
                            generatedTiles[x, z] = Instantiate(tileSet.coastalOuterCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                            if (directionOne == transform.forward && directionTwo == -transform.right || directionTwo == transform.forward && directionOne == -transform.right)
                            {
                                generatedTiles[x, z].transform.Rotate(0, -90, 0);
                                generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                            }
                            else if (directionOne == transform.right && directionTwo == -transform.forward || directionTwo == transform.right && directionOne == -transform.forward)
                            {
                                generatedTiles[x, z].transform.Rotate(0, 90, 0);
                                generatedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                            }
                            else if (directionOne == -transform.forward && directionTwo == -transform.right || directionTwo == -transform.forward && directionOne == -transform.right)
                            {
                                generatedTiles[x, z].transform.Rotate(0, 180, 0);
                                generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                            }
                        }
                    }
                }
                List<GameObject> cornerAdjacentOcean = GetCornerAdjacentOceanTiles(x, z);
                if (cornerAdjacentOcean.Count == 1 && edgeAdjacentOcean.Count == 0) // coastal inner corner
                {
                    Vector3 direction = cornerAdjacentOcean[0].transform.localPosition - generatedTiles[x, z].transform.localPosition;
                    direction = direction.normalized;
                    DestroyImmediate(generatedTiles[x, z]);
                    int randomIndex = Random.Range(0, tileSet.coastalInnerCorner.Length);
                    generatedTiles[x, z] = Instantiate(tileSet.coastalInnerCorner[randomIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
                    float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
                    if (angle > 90 && angle <= 180)
                    {
                        generatedTiles[x, z].transform.Rotate(0, -90, 0);
                        generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, 0);
                    }
                    else if (angle < -90 && angle >= -180)
                    {
                        generatedTiles[x, z].transform.Rotate(0, 180, 0);
                        generatedTiles[x, z].transform.localPosition += new Vector3(tileSize, 0, tileSize);
                    }
                    else if (angle < 0 && angle >= -90)
                    {
                        generatedTiles[x, z].transform.Rotate(0, 90, 0);
                        generatedTiles[x, z].transform.localPosition += new Vector3(0, 0, tileSize);
                    }
                }
            }
        }
    }

    private void TurnIntoOceanTile(int x, int z)
    {
        DestroyImmediate(generatedTiles[x, z]);
        int randomTileIndex = Random.Range(0, tileSet.oceanTiles.Length);
        GameObject newTile = Instantiate(tileSet.oceanTiles[randomTileIndex], new Vector3(x * tileSize, transform.position.y, z * tileSize), Quaternion.identity, transform);
        generatedTiles[x, z] = newTile;
    }

    public List<GameObject> GetEdgeAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && IsOceanTile(x - 1, z))
        {
            oceanTiles.Add(generatedTiles[x - 1, z]);
        }
        if (x + 1 < generatedTiles.GetLength(0) && IsOceanTile(x + 1, z))
        {
            oceanTiles.Add(generatedTiles[x + 1, z]);
        }
        if (z - 1 >= 0 && IsOceanTile(x, z - 1))
        {
            oceanTiles.Add(generatedTiles[x, z - 1]);
        }
        if (z + 1 < generatedTiles.GetLength(1) && IsOceanTile(x, z + 1))
        {
            oceanTiles.Add(generatedTiles[x, z + 1]);
        }
        return oceanTiles;
    }

    public List<GameObject> GetCornerAdjacentOceanTiles(int x, int z)
    {
        List<GameObject> oceanTiles = new List<GameObject>();
        if (x - 1 >= 0 && z - 1 >= 0 && IsOceanTile(x - 1, z - 1))
        {
            oceanTiles.Add(generatedTiles[x - 1, z - 1]);
        }
        if (x + 1 < generatedTiles.GetLength(0) && z + 1 < generatedTiles.GetLength(1) && IsOceanTile(x + 1, z + 1))
        {
            oceanTiles.Add(generatedTiles[x + 1, z + 1]);
        }
        if (x - 1 >= 0 && z + 1 < generatedTiles.GetLength(1) && IsOceanTile(x - 1, z + 1))
        {
            oceanTiles.Add(generatedTiles[x - 1, z + 1]);
        }
        if (x + 1 < generatedTiles.GetLength(1) && z - 1 >= 0 && IsOceanTile(x + 1, z - 1))
        {
            oceanTiles.Add(generatedTiles[x + 1, z - 1]);
        }
        return oceanTiles;
    }

    public bool IsOceanTile(int x, int z)
    {
        return generatedTiles[x,z].CompareTag("Terrain/OceanFloor");
    }

    public void ClearTerrain()
    {
        while (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        elevationNoiseMap.ResetNoiseRange();
        generatedTiles = null;
    }

}
