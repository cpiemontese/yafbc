using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneratorController : MonoBehaviour
{
    public GameObject obstacleTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateObstacle(int maxHeight) {
        int height = Random.Range(1, maxHeight);

        GameObject obstacle = new GameObject("Obstacle Container");
        obstacle.AddComponent<BoxCollider2D>();
        var bc2d = obstacle.GetComponent<BoxCollider2D>();
        bc2d.size = new Vector2(1f, maxHeight + 1f);
        bc2d.offset = new Vector2(0f, ((maxHeight + 1f) / 2.0f) - 0.5f);
        bc2d.isTrigger = true;
        obstacle.AddComponent<Rigidbody2D>();
        var rb2d = obstacle.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;;

        GameObject obstacleSubTop = new GameObject("Obstacle Sub Container Top");
        GameObject obstacleSubBot = new GameObject("Obstacle Sub Container Bot");

        var bottomHeight = Mathf.Round(height/2f);
        var topHeight = Mathf.Round(height/2f + 1f);
        SetupObstacleSubContainer(
            obstacleSubBot,
            obstacle.transform,
            new Vector2(1f, bottomHeight),
            new Vector2(0f, (bottomHeight - 1f)/2f));
        SetupObstacleSubContainer(
            obstacleSubTop,
            obstacle.transform,
            new Vector2(1f, topHeight),
            new Vector2(0f, maxHeight - topHeight/2f + 0.5f));

        for (int i = 0; i < bottomHeight; i++) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacleSubBot.transform);
        }

        for (int i = maxHeight; i > maxHeight - topHeight; i--) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacleSubTop.transform);
        }

        return obstacle;
    }

    void SetupObstacleSubContainer(GameObject osc, Transform parentTransform, Vector2 size, Vector2 offset)
    {
        osc.tag = "Obstacle Sub Container";
        osc.transform.parent = parentTransform;
        osc.AddComponent<BoxCollider2D>();
        var bc2d = osc.GetComponent<BoxCollider2D>();
        bc2d.size = size;
        bc2d.offset = offset;
        bc2d.isTrigger = true;
    }
}
