using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab, tailPrefab;
    private GameObject food;
    [SerializeField] private float stepRate, currentAngleZ;
    private Vector2 move;
    private List<Transform> tail = new List<Transform>();
    private bool issFoodEating;
    private Vector2 topWallPosition, bottomWallPosition, leftWallPosition, rightWallPosition;

    void Start()
    {
        topWallPosition = GameObject.Find("TopWall").transform.position;
        bottomWallPosition = GameObject.Find("BottomWall").transform.position;
        leftWallPosition = GameObject.Find("LeftWall").transform.position;
        rightWallPosition = GameObject.Find("RightWall").transform.position;
        InvokeRepeating("Movement", 0.1f, stepRate);
        SpawnFood();
    }

    void SpawnFood()
    {
        int spawnX = (int)Random.Range(leftWallPosition.x + 1f, rightWallPosition.x - 1f);
        int spawnY = (int)Random.Range(bottomWallPosition.y + 1f, topWallPosition.y - 1f);
        food = Instantiate(foodPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
    }

    void Movement()
    {

    }
}
