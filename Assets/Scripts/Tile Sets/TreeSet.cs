using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTreeSet", menuName = "Tree Set", order = 4)]
public class TreeSet : ScriptableObject
{
    public List<GameObject> commonTrees;
    public List<GameObject> rareTrees;
    public List<GameObject> backgroundTrees;
}
