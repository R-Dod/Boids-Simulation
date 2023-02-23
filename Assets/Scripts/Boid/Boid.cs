using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Boid : MonoBehaviour
{

    Collider2D boidCollider;

    // Vector2 position;
    public Vector2 velocity;
    public float radius = 15f;

    [Range(0f, 100f)]

    public float protected_range = 2.5f;

    public float turnFactor = 0.25f;

    public float speed = 5f;

    [Range(0f, 1f)]

    public float cohesionFactor = 0.005f;
    [Range(0f, 1f)]

    public float avoidanceFactor = 0.05f;
    [Range(0f, 1f)]

    public float alignmentFactor = 0.05f;

    public float horizontalRadius = 20f; // radius of the circle
    [Range(0f, 50f)]

    public float verticalRadius = 10f;
    [Range(0f, 10f)]

    private float angle = 0f; // current angle

    private float RotateSpeed = 0.5f;
    static Boid centerBoid;

    List<Boid> neighbors;

    List<Transform> obstacles;

    public Collider2D BoidCollider
    {
        get
        {
            return boidCollider;
        }
    }


    public List<Boid> Neighbors { get => neighbors; set => neighbors = value; }
    public List<Transform> Obstacles { get => obstacles; set => obstacles = value; }
    public static Boid CenterBoid { get => centerBoid; set => centerBoid = value; }

    // public static Vector2 Center { get => center; set => center = value; }

    // Start is called before the first frame update

    void Start()
    {
        boidCollider = GetComponent<Collider2D>();
    }


    public void Move()
    {
        velocity += calculateVelocity();
        if (velocity.sqrMagnitude > (speed * speed))
        {
            velocity = velocity.normalized * speed;
        }
        // velocity *=driveFactor;
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;


    }

    private Vector2 calculateVelocity()
    {
        return
        Avoidance()
        + Alignment()
        + Cohesion()
        + MoveWithinRadius()
        + AvoidObstacles();
    }

    // public Vector2 Circle()
    // {
    //     angle += RotateSpeed * Time.deltaTime; // increase angle over time
    //     float x = horizontalRadius * Mathf.Cos(angle); // calculate X coordinate
    //     float y = verticalRadius * Mathf.Sin(angle); // calculate Y coordinate
    //     Vector2 vec = new Vector2(x, y);
    //     return vec;
    // }

    public Vector2 MoveWithinRadius()
    {
        Vector2 centerVec = Vector2.zero;
        Vector2 displacement = centerVec - (Vector2)transform.position;
        float distance = displacement.magnitude;

        if (distance < radius)
        {
            return Vector2.zero;

        }
        return displacement * turnFactor;
    }

    Vector2 AvoidObstacles()
    {
        Vector2 avd = Vector2.zero;

        foreach (Transform item in obstacles)
        {
            float distance = Vector2.Distance(transform.position, item.position);

            if ((distance * distance) < (protected_range * protected_range))
            {
                avd += (Vector2)(transform.position - item.position);
            }
        }

        return avd * turnFactor;
    }

    Vector2 Avoidance()
    {
        Vector2 avd = Vector2.zero;

        foreach (Boid item in neighbors)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);

            if ((distance * distance) < (protected_range * protected_range))
            {
                avd += (Vector2)(transform.position - item.transform.position);
            }
        }

        return avd * avoidanceFactor;
    }

    Vector2 Alignment()
    {
        Vector2 sum_velocity = centerBoid.transform.up;

        if (neighbors == null || neighbors.Count == 0)
            return transform.up;

        foreach (Boid item in neighbors)
        {
            sum_velocity += (Vector2)item.transform.up;
        }

        Vector2 avg_vel = sum_velocity / neighbors.Count;

        return avg_vel * alignmentFactor;

    }

    Vector2 Cohesion()
    {

        Vector2 relative_center_sum = centerBoid.transform.position;

        if (neighbors == null || neighbors.Count == 0)

            return Vector2.zero;


        foreach (Boid item in neighbors)
        {
            relative_center_sum += (Vector2)item.transform.position;
        }

        Vector2 relative_center = relative_center_sum / neighbors.Count;

        return (relative_center - (Vector2)transform.position) * cohesionFactor;
    }
}
