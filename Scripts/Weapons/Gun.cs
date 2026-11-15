using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IWeapon
{
    [SerializeField] private byte bulletIndex = 0;
    [SerializeField] private float cooldown = 0.1f;
    private float timer = 0;
    private bool shooting = false;
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
        if (!gameObject.GetComponent<Pickup>().pickedUp)
        {
            return;
        }
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 triangle = target - transform.position;
        float angle = Mathf.Atan2(triangle.y,triangle.x);
        transform.eulerAngles = new Vector3(0,0,angle * Mathf.Rad2Deg);

        timer -= Time.deltaTime;
        if (shooting)
        {
            if (timer <= 0)
            {
                timer = cooldown;
                ObjectManager.instance.MakeBullet(bulletIndex, transform.position, transform.eulerAngles.z);
            }
        }
    }

}
