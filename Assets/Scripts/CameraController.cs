using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform lookAt;

    // Start is called before the first frame update
    void Start()
    {
        var camera = GetComponent<Camera>();
        var collider2d = GetComponent<BoxCollider2D>();

        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));

        collider2d.size = new Vector2(bottomRight.x - bottomLeft.x, topRight.y - bottomRight.y);
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(
            new Vector3(lookAt.position.x, transform.position.y, transform.position.z),
            transform.rotation
        );
    }

    void OnTriggerExit2D(Collider2D other) {
        Destroy(other.gameObject);
    }
}
