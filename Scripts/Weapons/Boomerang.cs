using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour, IWeapon
{
    [SerializeField] private float cooldown = 0.1f;
    [SerializeField] private Rigidbody2D rb2d = null;
    [SerializeField] private float radius = 0.6f;
    [SerializeField] private float dmg = 5f;
    private float timer = 0;
    private bool shooting = false;
    private bool seperate = false;
    private Vector2 targetPos;
    private Vector2 startPos;
    private float throwTime = 0;
    private void OnEnable()
    {
        timer = cooldown;
    }
    public void Shoot()
    {
        shooting = true;
    }
    public void StopShoot()
    {
        shooting = false;
    }

    private void Update()
    {


        if (seperate)
        {
            transform.eulerAngles += new Vector3(0, 0, 720 * Time.deltaTime);
            throwTime += Time.deltaTime;
            transform.position = Vector2.Lerp(throwTime < 0.5f ? startPos : MovementScript.instance.transform.position, targetPos, (1 - Mathf.Cos(throwTime * 360 * 3.1415f / 180)) / 2);
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i].transform.parent == null)
                    continue;
                if (collisions[i].transform.parent.GetComponent<Enemy>() != null)
                {
                    collisions[i].transform.parent.GetComponent<Enemy>().Damage(dmg);
                }
            }

            if (throwTime > 1)
            {
                seperate = false;
                gameObject.GetComponent<Pickup>().TempReattach();
            }
        }
        else
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 triangle = target - transform.position;
            float angle = Mathf.Atan2(triangle.y, triangle.x);
            transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

            timer -= Time.deltaTime;
            if (shooting)
            {
                if (timer <= 0)
                {
                    seperate = true;
                    timer = cooldown;
                    gameObject.GetComponent<Pickup>().TempSeperate();
                    targetPos = target;
                    startPos = transform.position;
                    throwTime = 0;
                }
            }
        }
    }
}
