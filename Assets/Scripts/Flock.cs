using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid myPrefab;
    public GameObject myObstable;
    List<Boid> boids = new List<Boid>();

    [Range(1, 500)]
    public int boidNumber = 2;

    [Range(1f, 100f)]
    public float visibleRange = 5f;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {

        // create center boid
        Boid newBoid = Instantiate(
         myPrefab,
         Random.insideUnitCircle * 10,
         Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
         transform
        );

        newBoid.name = "Boid " + i;
        boids.Add(newBoid);


    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        { // on left click, generate boids
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Boid newBoid = Instantiate(
               myPrefab,
            new Vector2(pos.x, pos.y),
               Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
               transform
              );

            newBoid.name = "Boid " + i;
            boids.Add(newBoid);
            i++;
        }

        if (Input.GetMouseButtonDown(1))
        { // on right click, generate obstacles
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(
              myObstable,
            new Vector2(pos.x, pos.y),
               Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
               transform
              );
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    // Update is called once per frame
    void Update()
    {

        foreach (Boid item in boids)
        { // simulate flocking movement
            List<Boid> neighbors = getNeighbors(item);
            item.Neighbors = neighbors;
            item.Obstacles = getObstacles(item);
            item.Move();
        }
    }


    List<Boid> getNeighbors(Boid boid)
    { //get all neighbors within the boid visible range
        List<Boid> neighbors = new List<Boid>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(boid.transform.position, visibleRange);

        foreach (Collider2D item in contextColliders)
        {
            if (item != boid.BoidCollider && item.gameObject.layer != 8)
            {
                Boid context = item.gameObject.GetComponent<Boid>();
                neighbors.Add(context);
            }

        }
        return neighbors;
    }

    List<Transform> getObstacles(Boid boid)
    { // get all obstables within the boid visible range
        List<Transform> obstacles = new List<Transform>();
        Collider2D[] collection = Physics2D.OverlapCircleAll(boid.transform.position, visibleRange);

        foreach (Collider2D item in collection)
        {
            if (item != boid.BoidCollider && item.gameObject.layer == 8)
            {
                obstacles.Add(item.transform);
            }
        }

        return obstacles;
    }
}
