using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class RespawnTimer
{
    float timer;
    byte objectID;
}
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }
    [SerializeField] private List<RespawnTimer> respawnTimers = new List<RespawnTimer>();
    [SerializeField] private List<ObjectPool<GameObject>> bulletPools = new List<ObjectPool<GameObject>>();
    [SerializeField] private List<ObjectPool<GameObject>> enemyPools = new List<ObjectPool<GameObject>>();
    [SerializeField] private ObjectPool<GameObject> tombstonePool = null;
    [SerializeField] private List<SO_Enemy> enemySOs = new List<SO_Enemy>();
    [SerializeField] private List<SO_Wave> levels = new List<SO_Wave>();
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private GameObject tombstone;
    [SerializeField] private BounceBackText[] bounceTexts = new BounceBackText[2];
    [SerializeField] private GameObject winScreen;
    [SerializeField] private AudioSource bounceAudio;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject boomerangPrefab;
    [SerializeField] private AudioSource normalAudio;
    [SerializeField] private AudioSource bossAudio;
    [SerializeField] private TMP_Text levelText;
    public float waveTimer = 3;
    public byte levelIndex = 0;
    public byte waveIndex = 0;
    public bool bounceBack = false;
    public UnityEvent bounceBackEvent;
    public bool canMoveOn = false;

    private bool dropBoomerang = false;

    private void MkEnemyPool(GameObject prefab, int defaultCap, int max)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: defaultCap, // Initial internal storage size
            maxSize: max         // Max inactive objects kept in memory
        );
        enemyPools.Add(pool);
    }
    private void MkBulletPool(GameObject prefab, int defaultCap, int max)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: defaultCap, // Initial internal storage size
            maxSize: max         // Max inactive objects kept in memory
        );
        bulletPools.Add(pool);
    }

    private void Start()
    {
        instance = this;
        for (int i = 0; i < enemySOs.Count; i++)
        {
            MkEnemyPool(enemySOs[i].prefab, enemySOs[i].defaultCap, enemySOs[i].maxCap);
        }
        for (int i = 0; i < bulletPrefabs.Length; i++)
        {
            Debug.Log("Made bullet pool");
            MkBulletPool(bulletPrefabs[i], 15, 40);
        }
        tombstonePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(tombstone),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: 50, // Initial internal storage size
            maxSize: 150         // Max inactive objects kept in memory
        );
    }
    public void SpawnEnemy(byte enemyIndex)
    {
        GameObject enemy = enemyPools[enemyIndex].Get();
        Vector3 spawnPos;
        Collider2D[] isTaken;
        do
        {
            spawnPos = MovementScript.instance.transform.position + new Vector3(Random.Range(5, 12) * (Random.Range(0, 2) * 2 - 1), Random.Range(5, 12) * (Random.Range(0, 2) * 2 - 1), 0);
            isTaken = Physics2D.OverlapBoxAll(spawnPos, new Vector2(2.5f, 2.5f), 0);
        } while (isTaken.Length > 0);

        spawnPos = new Vector3(Mathf.Clamp(spawnPos.x, -28.7f, 18.3f), Mathf.Clamp(spawnPos.y, -16.6f, 10.8f), 0);
        enemy.transform.position = spawnPos;
    }
    public void SpawnEnemy(byte enemyIndex, Vector2 position)
    {
        GameObject enemy = enemyPools[enemyIndex].Get();
        enemy.transform.position = position;
    }
    public void MakeBullet(byte bulletIndex, Vector2 position, float angle)
    {
        Debug.Log("Make bullet " + bulletIndex.ToString());

        GameObject obj = bulletPools[bulletIndex].Get();
        obj.transform.position = position;
        obj.transform.eulerAngles = new Vector3(0, 0, angle);
        if (obj.GetComponent<Bullet>() != null)
            obj.GetComponent<Bullet>().SetVelocity();
        if (obj.GetComponent<Bomb>() != null)
            obj.GetComponent<Bomb>().SetVelocity();
    }
    public void DespawnEnemy(GameObject enemy, byte enemyIndex)
    {
        GameObject tombstone = tombstonePool.Get();
        tombstone.GetComponent<Tombstone>().enemyID = enemyIndex;
        tombstone.transform.position = enemy.transform.position;

        if (Random.Range(0, 20) == 0)
        {
            Instantiate(healthPrefab, enemy.transform.position, Quaternion.identity);
        }

        if (dropBoomerang)
        {
            dropBoomerang = false;
            Instantiate(boomerangPrefab, enemy.transform.position, Quaternion.identity);
        }

        enemyPools[enemyIndex].Release(enemy);
    }
    public void DespawnTomb(GameObject tomb)
    {
        tombstonePool.Release(tomb);
    }
    public void DespawnBullet(GameObject bullet, byte bulletIndex)
    {
        bulletPools[bulletIndex].Release(bullet);
    }
    private void Update()
    {
        levelText.text = "Level #" + (levelIndex + 1).ToString();
        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            if (levelIndex >= levels.Count)
            {
                winScreen.SetActive(true);
                return;
            }
            if (waveIndex >= levels[levelIndex].waves.Count)
            {
                Debug.Log("Reached end of wave");
                bool noneLeft = true;
                for (int i = 0; i < enemyPools.Count; i++)
                {
                    if (enemyPools[i].CountActive > 0)
                    {
                        Debug.Log(enemyPools[i].CountActive.ToString() + "Enemies left in pool " + i.ToString());
                        noneLeft = false;
                        break;
                    }
                }
                if (noneLeft)
                {
                    Debug.Log("None left");
                }
                if (!noneLeft && bounceBack)
                {
                    canMoveOn = true;
                }
                if (noneLeft && !bounceBack)
                {
                    Debug.Log("Bounce back");
                    bounceAudio.Play();
                    bounceBack = true;
                    bounceTexts[0].Animate();
                    bounceTexts[1].Animate();
                    bounceBackEvent?.Invoke();
                    return;
                }
                if (noneLeft && bounceBack && canMoveOn)
                {
                    Debug.Log("Bounce back done");
                    bounceBack = false;
                    levelIndex++;
                    if (levelIndex == 3)
                    {
                        normalAudio.Stop();
                        bossAudio.Play();
                    }
                    waveIndex = 0;
                    waveTimer = 2;
                    if (levelIndex == 1)
                    {
                        dropBoomerang = true;
                    }
                }
                return;
            }
            if (levelIndex >= levels.Count)
            {
                return;
            }
            for (int i = 0; i < levels[levelIndex].waves[waveIndex].amount; i++)
            {
                SpawnEnemy(levels[levelIndex].waves[waveIndex].enemyIndex);
            }
            waveTimer = levels[levelIndex].waves[waveIndex].delay;
            waveIndex++;
        }
    }
}
