using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float spawnTimer = 1.0f;
    public float spawnProbability = 0.5f;
    public float tileMovementSpeed = 1.0f;
    public GameObject topTile;
    public GameObject bottomTile;
    public PlayerController playerController;
    public ObstacleGeneratorController obstacleGeneratorController;

    Queue<GameObject> obstacles;
    List<Tuple<Rigidbody2D, Rigidbody2D>> zippedTiles;

    bool resetting = false;
    float cameraHeight;
    float timeSinceLastSpawn;
    Vector3 topRight;
    Vector3 bottomRight;
    Vector3 bottomLeft;

    enum Coordinate {
        TopRight, TopLeft, BottomRight, BottomLeft   
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController.resetEventHandlers += ResetEventHandler;
        playerController.exitEventHandlers += ExitEventHandler;

        var camera = GetComponent<Camera>();
        var collider2d = GetComponent<BoxCollider2D>();

        timeSinceLastSpawn = 1.0f;
        obstacles = new Queue<GameObject>();
        zippedTiles = new List<Tuple<Rigidbody2D, Rigidbody2D>>();

        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        collider2d.size = new Vector2(bottomRight.x - bottomLeft.x, topRight.y - bottomRight.y);

        var topY = topRight.y - 0.5f;
        var bottomY = bottomRight.y + 0.5f;
        var topTilesContainer = new GameObject("Top tiles container");
        var bottomTilesContainer = new GameObject("Bottom tiles container");

        var size = bottomRight.x + 2.5f - (bottomLeft.x + 0.5f);

        SetupTilesContainer(topTilesContainer, new Vector2(size, 1f), new Vector2(0f, topY));
        SetupTilesContainer(bottomTilesContainer, new Vector2(size, 1f), new Vector2(0f, bottomY));

        for (float d = bottomLeft.x + 0.5f; d < bottomRight.x + 2.5f; d++)
        {
            var newTopTile = Instantiate(topTile, new Vector3(d, topY), Quaternion.identity, topTilesContainer.transform);
            var newBottomTile = Instantiate(bottomTile, new Vector3(d, bottomY), Quaternion.identity, bottomTilesContainer.transform);
            zippedTiles.Add(Tuple.Create(
                newTopTile.GetComponent<Rigidbody2D>(),
                newBottomTile.GetComponent<Rigidbody2D>()
            ));
        }

        cameraHeight = topRight.y - bottomRight.y - 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastSpawn >= spawnTimer)
        {
            timeSinceLastSpawn -= spawnTimer;
            if (UnityEngine.Random.Range(0.0f, 1.0f) < spawnProbability)
            {
                var obstacle = obstacleGeneratorController.GenerateObstacle(
                    Mathf.RoundToInt(cameraHeight),
                    new Vector3(bottomRight.x + 1.5f, bottomRight.y + 1.5f, 0),
                    Quaternion.identity);
                obstacles.Enqueue(obstacle);
            }
        }
        timeSinceLastSpawn += Time.deltaTime;
    }

    void FixedUpdate()
    {
        var movementMagnitude = tileMovementSpeed * Time.fixedDeltaTime;
        zippedTiles.ForEach(tuple => {
            tuple.Item1.MovePosition(new Vector2(tuple.Item1.position.x - movementMagnitude, tuple.Item1.position.y));
            tuple.Item2.MovePosition(new Vector2(tuple.Item2.position.x - movementMagnitude, tuple.Item2.position.y));
        });

        obstacleGeneratorController.Move(obstacles, movementMagnitude);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle Sub Top":
                if (!resetting)
                {
                    Destroy(obstacles.Dequeue());
                }
                break;
            case "Tile":
                var oldY = other.gameObject.GetComponent<Rigidbody2D>().position.y;
                var newX = topRight.x + 1.5f;
                other.gameObject.transform.SetPositionAndRotation(new Vector3(newX, oldY), Quaternion.identity);
                break;
        }
    }

    void SetupTilesContainer(GameObject tileContainer, Vector2 size, Vector2 offset)
    {
        tileContainer.tag = "Floor";
        tileContainer.AddComponent<BoxCollider2D>();
        var bc2d = tileContainer.GetComponent<BoxCollider2D>();
        bc2d.isTrigger = true;
        bc2d.size = size;
        bc2d.offset = offset;
    }

    void ResetEventHandler(object o, EventArgs e)
    {
        resetting = true;
        while (obstacles.Count > 0)
        {
            Destroy(obstacles.Dequeue());
        }
        resetting = false;
    }

    void ExitEventHandler(object o, EventArgs e) {
        SceneManager.LoadScene("RetryMenu");
    }
}
