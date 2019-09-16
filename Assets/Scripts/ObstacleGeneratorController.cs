using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGeneratorController : MonoBehaviour
{
    public GameObject obstacleTile;
    public GameObject obstacleContainer;
    public GameObject obstacleSubContainer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GenerateObstacle(int maxHeight, Vector3 at, Quaternion rotation) {
        var topHeight = Mathf.Round(Random.Range(1f, maxHeight/2f));
        var bottomHeight = Mathf.Round(Random.Range(1f, maxHeight/2f));

        GameObject oc = Instantiate(obstacleContainer, at, rotation);
        GameObject obstacleSubTop = Instantiate(obstacleSubContainer, at, rotation, oc.transform);
        GameObject obstacleSubBot = Instantiate(obstacleSubContainer, at, rotation, oc.transform);

        obstacleSubTop.tag = "Obstacle Sub Top";
        obstacleSubBot.tag = "Obstacle Sub Bot";

        SetupObstacleSubContainer(
            obstacleSubBot,
            oc.transform,
            new Vector2(1f, bottomHeight),
            new Vector2(0f, (bottomHeight - 1f)/2f));

        SetupObstacleSubContainer(
            obstacleSubTop,
            oc.transform,
            new Vector2(1f, topHeight),
            new Vector2(0f, maxHeight - topHeight/2f + 0.5f));

        for (int i = 0; i < bottomHeight; i++) {
            Instantiate(obstacleTile, new Vector3(at.x, i - 3.5f, 0), rotation, obstacleSubBot.transform);
        }

        for (int i = maxHeight; i > maxHeight - topHeight; i--) {
            Instantiate(obstacleTile, new Vector3(at.x, i - 3.5f, 0), rotation, obstacleSubTop.transform);
        }

        return oc;
    }

    public void Move(Queue<GameObject> obstacles, float movementMagnitude) {
       foreach (var obstacle in obstacles)
       {
           var sub0 = obstacle.transform.GetChild(0).GetComponent<Rigidbody2D>();
           var sub1 = obstacle.transform.GetChild(1).GetComponent<Rigidbody2D>();
           sub0.MovePosition(new Vector2(sub0.position.x - movementMagnitude, sub0.position.y));
           sub1.MovePosition(new Vector2(sub1.position.x - movementMagnitude, sub1.position.y));
       } 
    }

    void SetupObstacleSubContainer(GameObject osc, Transform parentTransform, Vector2 size, Vector2 offset)
    {
        var bc2d = osc.GetComponent<BoxCollider2D>();
        bc2d.size = size;
        bc2d.offset = offset;
    }
}
