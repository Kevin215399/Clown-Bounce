using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private Rigidbody2D rb2d;
    void Update()
    {
        Vector2 moveTowards = MovementScript.instance.gameObject.transform.position - transform.position;
        moveTowards.Normalize();
        moveTowards *= speed;
        rb2d.velocity = moveTowards;
    }
}
