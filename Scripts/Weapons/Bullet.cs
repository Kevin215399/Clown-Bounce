using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private byte bulletIndex = 0;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private float radius = 0.1f;
    [SerializeField] private float dmg = 10;
    public void SetVelocity()
    {
        rb2d.velocity = transform.right * speed;
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, MovementScript.instance.transform.position) > 20)
        {
            ObjectManager.instance.DespawnBullet(gameObject, bulletIndex);
        }

        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius);
        for (int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].transform.parent == null)
                continue;
            if (collisions[i].transform.parent.GetComponent<Enemy>() != null)
            {
                collisions[i].transform.parent.GetComponent<Enemy>().Damage(dmg);
                ObjectManager.instance.DespawnBullet(gameObject, bulletIndex);
                break;
            }
        }
    }
}
