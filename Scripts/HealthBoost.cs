using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    void Update()
    {
        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(1,2),0);
        for(int i = 0; i < collisions.Length; i++)
        {
            if(collisions[i].transform.parent == null)
            {
                continue;
            }
            if(collisions[i].transform.parent.GetComponent<MovementScript>() != null)
            {
                collisions[i].transform.parent.GetComponent<MovementScript>().Damage(-15);
                Destroy(gameObject);
            }
        }
    }
}
