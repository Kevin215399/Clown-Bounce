using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public byte enemyIndex = 0;
    public int amount = 10;
    public float delay = 1;
}
[CreateAssetMenu()]
public class SO_Wave : ScriptableObject
{
    public List<Wave> waves= new List<Wave>();
}
