using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_Enemy : ScriptableObject
{
    public float health = 100;
    public float spawnTimer = 1;
    public byte objectID = 0;
    public float damage = 0;
    public GameObject prefab = null;

    public int defaultCap = 20;
    public int maxCap = 100;
}
