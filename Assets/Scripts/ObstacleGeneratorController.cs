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
        for (int i = 0; i < height/2; i++) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacle.transform);
        }

        for (int i = maxHeight; i > maxHeight - (height/2 + 1); i--) {
            Instantiate(obstacleTile, new Vector3(0, i, 0), Quaternion.identity, obstacle.transform);
        }

        return obstacle;
    }
}
