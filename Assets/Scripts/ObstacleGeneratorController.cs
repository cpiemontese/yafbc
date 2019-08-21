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

        GameObject obstacle = new GameObject("Obstacle");
        obstacle.tag = "Obstacle";
        obstacle.AddComponent<BoxCollider2D>();
        var bc2d = obstacle.GetComponent<BoxCollider2D>();
        bc2d.size = new Vector2(1f, 8f);
        bc2d.offset = new Vector2(0f, 3.5f);
        bc2d.isTrigger = true;
        obstacle.AddComponent<Rigidbody2D>();
        var rb2d = obstacle.GetComponent<Rigidbody2D>();
        rb2d.isKinematic = true;
        rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;

        for (int i = 0; i < height/2; i++) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacle.transform);
        }

        for (int i = maxHeight; i > maxHeight - (height/2 + 1); i--) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacle.transform);
        }

        return obstacle;
    }
}
