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
    float majorAxis = 15f;
    float minorAxis = 10f;
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


    // Start is called before the first frame update

    void Start()
    {
        boidCollider = GetComponent<Collider2D>();
    }


    public void Move()
    {
        velocity += calculateVelocity();

        //limit boid speed
        if (velocity.sqrMagnitude > (speed * speed))
        {
            velocity = velocity.normalized * speed;
        }
        //point in direction of movement
        transform.up = velocity;

        //move boid. Time.delta smoothens the movement
        transform.position += (Vector3)velocity * Time.deltaTime;

    }

    private Vector2 calculateVelocity()
    {
        return
        Avoidance()
        + Alignment()
        + Cohesion()
        + MoveWithinScreenBounds()
        + AvoidObstacles();
    }

    public Vector2 MoveWithinScreenBounds()
    {
        // limit movement within screenbounds in an oval
        Vector2 center = Vector2.zero;

        Vector2 position = transform.position;

        Vector2 displacement = center - position;
        float distance = displacement.magnitude;

        float x = position.x - center.x;
        float y = position.y - center.y;
        float result = (x * x) / (majorAxis * majorAxis) + (y * y) / (minorAxis * minorAxis);

        if (result <= 1)
        {
            return Vector2.zero;
        }

        return displacement * turnFactor;
    }

    Vector2 AvoidObstacles()
    {
        // use avoidance login for obstacles
        Vector2 avd = Vector2.zero;

        foreach (Transform item in obstacles)
        {
            float distance = Vector2.Distance(transform.position, item.position);

            if ((Math.Abs(distance) < protected_range))
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

            // get average displacement vector between boid and neighbors if within its protected range

            if ((distance * distance) < (protected_range * protected_range))
            {
                avd += (Vector2)(transform.position - item.transform.position);
            }
        }

        return avd * avoidanceFactor;
    }

    Vector2 Alignment()
    {
        // get average direction in which neighbors are pointing
        Vector2 sum_velocity = Vector2.zero;

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
        //get velocity from center relative to its neighbors

        Vector2 relative_center_sum = Vector2.zero;

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
