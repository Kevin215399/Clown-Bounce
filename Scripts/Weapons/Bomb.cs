using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private float speed;
    [SerializeField] private SimpleAnimate fuseAnimate;
    [SerializeField] private ParticleSystem explodePS;
    [SerializeField] private byte objectIndex;
    private float startHeight = 0;
    private float bounceHeight = 0;
    private int bounces = 0;
    private float deleteTime = 0;
    private bool doDamage = true;
    public void SetVelocity()
    {
        startHeight = transform.position.y;
        bounces = Random.Range(1, 3);
        bounceHeight = startHeight - Random.Range(.5f, 1f);
        rb2D.velocity = transform.up * speed;
        fuseAnimate.gameObject.SetActive(true);
        doDamage = true;
    }
    private void Update()
    {
        if (deleteTime > 0)
        {
            deleteTime -= Time.deltaTime;
            if (deleteTime < 3 - .3f)
            {
                fuseAnimate.gameObject.SetActive(false);
                if (doDamage)
                {
                    doDamage = false;
                    Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 2);
                    for (int i = 0; i < collisions.Length; i++)
                    {
                        if (collisions[i].transform.parent == null)
                            continue;
                        if (collisions[i].transform.parent.GetComponent<MovementScript>() != null)
                        {
                            collisions[i].transform.parent.GetComponent<MovementScript>().Damage(15);
                            break;
                        }
                    }
                }
            }
            if (deleteTime <= 0)
            {
                ObjectManager.instance.DespawnBullet(gameObject, objectIndex);
            }
        }
        if (transform.position.y < bounceHeight && rb2D.velocity.y < 0)
        {
            if (bounces == 0)
            {
                rb2D.velocity = new Vector2(0, 0);
                rb2D.isKinematic = true;
                deleteTime = 3;
                explodePS.Play();
                fuseAnimate.PlayOnce();
            }
            else
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, Mathf.Abs(rb2D.velocity.y) * .7f);
                bounces--;
            }

        }
    }
}
