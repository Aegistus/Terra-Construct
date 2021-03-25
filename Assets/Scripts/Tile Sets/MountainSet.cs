using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMountainSet", menuName = "Mountain Set", order = 3)]
public class MountainSet : ScriptableObject
{
    public GameObject[] smallMountains;
    public GameObject[] mediumMountains;
    public GameObject[] largeMountains;
}
