using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;
    public GameObject topTile;
    public GameObject bottomTile;

    ArrayList bottomTiles;
    ArrayList topTiles;

    enum Coordinate {
        TopRight, TopLeft, BottomRight, BottomLeft   
    }

    // Start is called before the first frame update
    void Start()
    {
        var camera = GetComponent<Camera>();
        var collider2d = GetComponent<BoxCollider2D>();
        bottomTiles = new ArrayList();
        topTiles = new ArrayList();

        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        collider2d.size = new Vector2(bottomRight.x - bottomLeft.x, topRight.y - bottomRight.y);

        var topY = topRight.y - 0.5f;
        var bottomY = bottomRight.y + 0.5f;

        for (float d = bottomLeft.x + 0.5f; d < bottomRight.x + 2.5f; d++) {
            topTiles.Add(Instantiate(topTile, new Vector3(d, topY), Quaternion.identity));
            bottomTiles.Add(Instantiate(bottomTile, new Vector3(d, bottomY), Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(
            new Vector3(lookAt.position.x, transform.position.y, transform.position.z),
            transform.rotation
        );
    }

    Vector3 GetCoordinate(Coordinate coordinate) {
        var camera = GetComponent<Camera>();

        switch (coordinate)
        {
            case Coordinate.TopRight:
                return camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
            case Coordinate.BottomRight:
                return camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
            case Coordinate.BottomLeft:
                return camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            case Coordinate.TopLeft:
                return camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        }
        return Vector3.zero;
    }

    void OnTriggerExit2D(Collider2D other) {
        var oldY = other.gameObject.GetComponent<Rigidbody2D>().position.y;
        var newX = GetCoordinate(Coordinate.TopRight).x + 1.5f;
        other.gameObject.transform.SetPositionAndRotation(new Vector3(newX, oldY), Quaternion.identity);
        //other.gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(newX, oldY));
    }
}
