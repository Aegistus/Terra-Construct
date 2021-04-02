using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject[] cloudPrefabs;
    public Bounds bounds;
    public Direction cloudMoveDirection = Direction.PositiveX;
    public float cloudMoveSpeed;
    public Vector3 startSide;
    public int maxActiveClouds = 50;
    public int cloudPoolSize = 100;

    private Vector3 moveVector;
    private Queue<GameObject> cloudPool = new Queue<GameObject>();
    private List<GameObject> currentClouds = new List<GameObject>();

    public enum Direction
    {
        PositiveX, NegativeX, PositiveZ, NegativeZ
    }

    private void Start()
    {
        for (int i = 0; i < cloudPoolSize; i++)
        {
            GameObject cloud = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Length)], transform);
            cloud.SetActive(false);
            cloudPool.Enqueue(cloud);
        }
        for (int i = 0; i < maxActiveClouds; i++)
        {
            GameObject startingCloud = cloudPool.Dequeue();
            Vector3 randomPos = new Vector3(Random.Range(-bounds.extents.x, bounds.extents.x), bounds.extents.y, Random.Range(-bounds.extents.z, bounds.extents.z));
            randomPos += bounds.center;
            startingCloud.transform.position = randomPos;
            startingCloud.SetActive(true);
            currentClouds.Add(startingCloud);
        }
    }

    private void Update()
    {
        switch (cloudMoveDirection)
        {
            case Direction.PositiveX:
                moveVector = transform.right;
                break;
            case Direction.NegativeX:
                moveVector = -transform.right;
                break;
            case Direction.PositiveZ:
                moveVector = transform.forward;
                break;
            case Direction.NegativeZ:
                moveVector = -transform.forward;
                break;
            default:
                break;
        }
        for (int i = 0; i < currentClouds.Count; i++)
        {
            if (bounds.Contains(currentClouds[i].transform.position))
            {
                currentClouds[i].transform.Translate(moveVector * cloudMoveSpeed * Time.deltaTime);
            }
            else
            {
                currentClouds[i].SetActive(false);
                cloudPool.Enqueue(currentClouds[i]);
                currentClouds.Remove(currentClouds[i]);
                if (currentClouds.Count < maxActiveClouds)
                {
                    SetNewCloudActive();
                }
            }
        }
    }

    private void SetNewCloudActive()
    {
        GameObject newCloud = cloudPool.Dequeue();
        Vector3 startingLocation;
        if (cloudMoveDirection == Direction.NegativeX || cloudMoveDirection == Direction.PositiveX)
        {
            startingLocation = new Vector3(bounds.extents.x * -moveVector.x, bounds.extents.y, Random.Range(-bounds.extents.z, bounds.extents.z));
        }
        else
        {
            startingLocation = new Vector3(Random.Range(-bounds.extents.x, bounds.extents.x), bounds.extents.y, bounds.extents.z * - moveVector.z);
        }
        startingLocation += bounds.center;
        newCloud.transform.position = startingLocation;
        newCloud.SetActive(true);
        currentClouds.Add(newCloud);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
