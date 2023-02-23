using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid myPrefab;
    // public MainBoid mainPrefab;
    // Boid centerBoid;
    public GameObject myObstable;
    // MainBoid mainBoid;
    List<Boid> boids = new List<Boid>();

    [Range(1, 500)]
    public int boidNum = 2;
    [Range(1f, 10f)]
    public float neighborRadius = 10f;
    [Range(1f, 100f)]
    public float visibleRange = 5f;
    int i = 0;

    // private void Awake()
    // {
    //     mainBoid = Instantiate(
    //     mainPrefab,
    //     new Vector2(0, -7),
    //     Quaternion.Euler(0, 0, 0),
    //     transform
    // );
    //     mainBoid.name = "Main Boid";
    // }

    // Start is called before the first frame update
    void Start()
    {

        // for (int i = 0; i < boidNum; i++)
        // {
            Boid newBoid = Instantiate(
             myPrefab,
             Random.insideUnitCircle * boidNum,
             Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
             transform
            );

            newBoid.name = "Boid " + i;
            boids.Add(newBoid);

            // centerBoid = newBoid;
        // }

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Boid newBoid = Instantiate(
               myPrefab,
            //    Random.insideUnitCircle * boidNum,
            new Vector2(pos.x, pos.y),
               Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
               transform
              );

            newBoid.name = "Boid " + i;
            boids.Add(newBoid);
            i++;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(
              myObstable,
            //    Random.insideUnitCircle * boidNum,
            new Vector2(pos.x, pos.y),
               Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
               transform
              );
        }

    }

    // Update is called once per frame
    void Update()
    {
        Boid.CenterBoid = getFlockCenter();

        foreach (Boid item in boids)
        {
            List<Boid> neighbors = getNeighbors(item);
            item.Neighbors = neighbors;
            item.Obstacles = getObstacles(item);
            // print(item.Neighbors.Count);
            // item.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, item.Neighbors.Count / 6f);

            item.Move();
        }
    }

    Boid getFlockCenter()
    {
        // Debug.Log(boids.Count);
        boids[0].gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        return boids[0];

    }

    List<Boid> getNeighbors(Boid boid)
    {
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
    {
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
