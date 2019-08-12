using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    public Camera gameCamera;
    public BoxCollider2D worldBounds;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 topRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, gameCamera.nearClipPlane));
        Vector3 topLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, gameCamera.nearClipPlane));
        Vector3 bottomRight = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, gameCamera.nearClipPlane));
        Vector3 bottomLeft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, gameCamera.nearClipPlane));

        Debug.Log(topRight);
        Debug.Log(topLeft);
        Debug.Log(bottomRight);
        Debug.Log(bottomLeft);
        worldBounds.size = new Vector2(bottomRight.x - bottomLeft.x, topRight.y - bottomRight.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
