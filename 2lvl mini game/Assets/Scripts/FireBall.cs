using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float fireBallForce = 20f;
    public int fireBallDamage = 60;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * fireBallForce;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<AIpatrol>().EnemyTakeDamage(fireBallDamage);
        }
        Destroy(gameObject);
    }
}
