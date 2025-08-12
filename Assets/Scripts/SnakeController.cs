using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab, tailPrefab;
    private GameObject food;
    [SerializeField] private float stepRate = 0.3f, currentDirection = 0.0f;
    private Vector2 move = new Vector2(-1f, 0);
    private List<Transform> tail = new List<Transform>();
    private bool issFoodEating = false;
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
        Vector2 currentPosition = transform.position;
        transform.position = currentPosition + move;
    }

    public void SetDirection(InputAction.CallbackContext context)
    {
        Vector2 newDirection = context.ReadValue<Vector2>();
        if (newDirection == Vector2.zero) return;
        if (newDirection == -move) return;
        if (Mathf.Abs(newDirection.x) > 0 && Mathf.Abs(newDirection.y) > 0) return; // ігнорувати діагональ 
        move = newDirection;
    }
}
