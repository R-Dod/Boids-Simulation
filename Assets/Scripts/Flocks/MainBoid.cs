using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MainBoid : MonoBehaviour
{
    Collider2D boidCollider;

    Vector2 velocity;
    [Range(0f, 50f)]

    public float horizontalRadius = 30f; // radius of the circle
    [Range(0f, 50f)]

    public float verticalRadius = 15f;
    [Range(0f, 10f)]

    public float speed = 1f; // speed of movement
    private float angle = 0f; // current angle

    private float RotateSpeed = 2f;
    // private float Radius = 5f;

    public Collider2D BoidCollider
    {
        get
        {
            return boidCollider;
        }
    }

    private void Start()
    {
    }



    void Update()
    {
        angle += RotateSpeed * Time.deltaTime; // increase angle over time
        float x = horizontalRadius * Mathf.Cos(angle); // calculate X coordinate
        float y = verticalRadius * Mathf.Sin(angle); // calculate Y coordinate
        Vector2 vec = new Vector2(x, y);

        Move(vec);
    }

    public void Move(Vector2 velocity)
    {
        transform.up = velocity;
        transform.position = (Vector2)transform.position + velocity * Time.deltaTime;
    }
}
