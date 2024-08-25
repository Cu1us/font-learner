using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(SpriteRenderer))]
public class BackgroundObject : MonoBehaviour
{
    public Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    public float minimumSpeed;
    public float minimumTimeAlive;
    float spawnTime;
    public float MidAcceleration;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnTime = Time.time;
    }
    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < minimumSpeed)
        {
            rb.velocity = rb.velocity.normalized * minimumSpeed;
        }
        if (!spriteRenderer.isVisible)
        {
            rb.velocity += transform.position.magnitude * MidAcceleration * Time.fixedDeltaTime * Vector2.one;
        }
    }
    private void Update()
    {
        if (!spriteRenderer.isVisible)
        {
            if (Time.time - spawnTime > minimumTimeAlive)
            {
                Destroy(gameObject);
            }
        }
    }
}
