using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    [SerializeField] private Transform visual;
    [SerializeField] private float target = 0;
    public byte enemyID = 0;
    private float spawnTimer = 0;
    private void OnEnable()
    {
        float xScale = Random.Range(0.8f, 1.3f);
        transform.localScale = new Vector3(xScale, xScale + Random.Range(-0.1f, 0.1f), 1);
        ObjectManager.instance.bounceBackEvent.AddListener(Bounceback);
    }
    private void OnDisable()
    {
        ObjectManager.instance.bounceBackEvent.RemoveListener(Bounceback);
    }
    private void Bounceback()
    {
        spawnTimer = Random.Range(0.1f, .5f) + 1.5f;
    }
    private void Update()
    {
        visual.position += new Vector3(0, (target - visual.localPosition.y) * Time.deltaTime, 0);
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                ObjectManager.instance.SpawnEnemy(enemyID, transform.position);
                ObjectManager.instance.DespawnTomb(gameObject);
            }
        }
    }
}
