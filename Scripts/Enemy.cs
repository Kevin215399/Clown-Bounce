using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SO_Enemy enemyTemplate;
    [SerializeField] private float health = 0;
    [SerializeField] private SpriteRenderer hitFlash;
    [SerializeField] private Vector2 dmgBox;
    [SerializeField] private AudioSource dmgSound;
    private bool dead = false;
    private float invincible = 0;
    private float spawnTimer = 1;
    private float dmgTimer = 0;
    void OnEnable()
    {
        spawnTimer = 1;
        dead = false;
        health = enemyTemplate.health;

    }

    public void Damage(float dmg)
    {
        if (dead)
            return;
        if (invincible > 0)
            return;
        invincible = .1f;
        health -= dmg;
        dmgSound.pitch = Random.Range(0.6f, 1.4f);
        dmgSound.volume = Random.Range(0.6f, 0.8f);
        dmgSound.Play();
        if (health <= 0)
        {
            dead = true;
            ObjectManager.instance.DespawnEnemy(gameObject, enemyTemplate.objectID);
        }
        else
        {
            hitFlash.enabled = true;
        }
    }
    public void Update()
    {
        if (spawnTimer < 0)
        {
            gameObject.GetComponent<MoveTowardPlayer>().enabled = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            gameObject.GetComponent<MoveTowardPlayer>().enabled = false;
            transform.localScale = new Vector3(1, 1 - spawnTimer, 1);
            spawnTimer -= Time.deltaTime * 2;
        }
        invincible -= Time.deltaTime;
        if (invincible <= 0)
        {
            hitFlash.enabled = false;
        }

        if (dmgTimer > 0)
        {
            dmgTimer -= Time.deltaTime;
        }
        else
        {
            Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, dmgBox, 0);
            for (int i = 0; i < collisions.Length; i++)
            {
                Debug.Log(collisions[i].name);
                if (collisions[i].transform.parent == null)
                    continue;
                if (collisions[i].transform.parent.GetComponent<MovementScript>() != null)
                {
                    collisions[i].transform.parent.GetComponent<MovementScript>().Damage(enemyTemplate.damage);
                    dmgTimer = .3f;
                    break;
                }
            }
        }
    }
}
