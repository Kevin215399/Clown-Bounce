using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    [SerializeField] private byte bombIndex = 0;
    [SerializeField] private float cooldown = .4f;
    [SerializeField] private SimpleAnimate throwBomb;
    private float timer = 0;
    private void Update()
    {
        if (Vector2.Distance(MovementScript.instance.transform.position, transform.position) < 8)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = cooldown;
                ObjectManager.instance.MakeBullet(bombIndex, transform.position, Random.Range(-8, 8));
                throwBomb.PlayOnce();
            }
        }
    }
}
