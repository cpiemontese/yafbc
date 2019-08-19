using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float tileMovementSpeed = 1.0f;
    public GameObject topTile;
    public GameObject bottomTile;
    public ObstacleGeneratorController obstacleGeneratorController;

    List<GameObject> topTiles;
    List<GameObject> bottomTiles;
    List<Tuple<Rigidbody2D, Rigidbody2D>> zippedTiles;

    Vector3 topRight;
    Vector3 bottomRight;
    Vector3 bottomLeft;

    enum Coordinate {
        TopRight, TopLeft, BottomRight, BottomLeft   
    }

    // Start is called before the first frame update
    void Start()
    {
        var camera = GetComponent<Camera>();
        var collider2d = GetComponent<BoxCollider2D>();
        bottomTiles = new List<GameObject>();
        topTiles = new List<GameObject>();
        zippedTiles = new List<Tuple<Rigidbody2D, Rigidbody2D>>();

        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        collider2d.size = new Vector2(bottomRight.x - bottomLeft.x, topRight.y - bottomRight.y);

        var topY = topRight.y - 0.5f;
        var bottomY = bottomRight.y + 0.5f;
        var topTilesContainer = new GameObject("Top tiles container");
        var bottomTilesContainer = new GameObject("Bottom tiles container");

        for (float d = bottomLeft.x + 0.5f; d < bottomRight.x + 2.5f; d++) {
            var newTopTile = Instantiate(topTile, new Vector3(d, topY), Quaternion.identity, topTilesContainer.transform);
            var newBottomTile = Instantiate(bottomTile, new Vector3(d, bottomY), Quaternion.identity, bottomTilesContainer.transform);
            topTiles.Add(newTopTile);
            bottomTiles.Add(newBottomTile);
            zippedTiles.Add(Tuple.Create(
                newTopTile.GetComponent<Rigidbody2D>(),
                newBottomTile.GetComponent<Rigidbody2D>()
            ));
        }

        var height = topRight.y - bottomRight.y - 3f;
        var obstacle = obstacleGeneratorController.GenerateObstacle(Mathf.RoundToInt(height));
        obstacle.transform.SetPositionAndRotation(new Vector3(bottomRight.x, bottomRight.y + 1.5f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate() {
        zippedTiles.ForEach(tuple => {
            tuple.Item1.MovePosition(new Vector2(tuple.Item1.position.x - tileMovementSpeed * Time.fixedDeltaTime, tuple.Item1.position.y));
            tuple.Item2.MovePosition(new Vector2(tuple.Item2.position.x - tileMovementSpeed * Time.fixedDeltaTime, tuple.Item2.position.y));
        });
    }

    void OnTriggerExit2D(Collider2D other) {
        var oldY = other.gameObject.GetComponent<Rigidbody2D>().position.y;
        var newX = topRight.x + 1.5f;
        other.gameObject.transform.SetPositionAndRotation(new Vector3(newX, oldY), Quaternion.identity);
    }
}
