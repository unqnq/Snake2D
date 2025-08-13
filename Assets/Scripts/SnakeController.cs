using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab, tailPrefab;
    private GameObject food;
    [SerializeField] private float stepRate = 0.3f;
    private Vector2 move = new Vector2(-1f, 0), nextMove;
    private List<Transform> tail = new List<Transform>();
    private bool isFoodEating = false;
    private Vector2 topWallPosition, bottomWallPosition, leftWallPosition, rightWallPosition;
    private UIController uiController;

    void Start()
    {
        topWallPosition = GameObject.Find("TopWall").transform.position;
        bottomWallPosition = GameObject.Find("BottomWall").transform.position;
        leftWallPosition = GameObject.Find("LeftWall").transform.position;
        rightWallPosition = GameObject.Find("RightWall").transform.position;
        uiController = GameObject.Find("UIController").GetComponent<UIController>();

        nextMove = move;
        InvokeRepeating("Movement", 0.1f, stepRate);
        SpawnFood();
        AddTail(transform.position);
        AddTail(transform.position);
    }

    void SpawnFood()
    {

        Vector3 spawnPosition;
        bool canSpawn;
        do
        {
            canSpawn = true;
            int spawnX = (int)Random.Range(leftWallPosition.x + 1f, rightWallPosition.x - 1f);
            int spawnY = (int)Random.Range(bottomWallPosition.y + 1f, topWallPosition.y - 1f);

            spawnPosition = new Vector2(spawnX, spawnY);

            if (spawnPosition == transform.position)
            {
                canSpawn = false;
                // Debug.Log("false");
            }

            if (canSpawn)
            {
                foreach (Transform segment in tail)
                {
                    if (spawnPosition == segment.position)
                    {
                        canSpawn = false;
                        // Debug.Log("false");
                        break;
                    }
                }
            }
        }
        while (!canSpawn); //поки не можна спавнити їжу - повторюємо цикл

        food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }

    void Movement()
    {
        move = nextMove;
        Vector2 currentPosition = transform.position;
        transform.position = currentPosition + move;

        if (isFoodEating)
        {
            AddTail(currentPosition);
            isFoodEating = false;
        }

        UpdateTailPosition(currentPosition);
    }

    public void SetDirection(InputAction.CallbackContext context)
    {
        Vector2 newDirection = context.ReadValue<Vector2>();
        if (newDirection == Vector2.zero) return;
        if (newDirection == -move) return;
        if (Mathf.Abs(newDirection.x) > 0 && Mathf.Abs(newDirection.y) > 0) return; // ігнорувати діагональ 
        nextMove = newDirection;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            isFoodEating = true;
            uiController.UpdateScore();
            SpawnFood();
        }

        else if (collision.CompareTag("Wall"))
        {
            Vector2 teleportPosition = transform.position;

            if (collision.gameObject.name == "TopWall")
            {
                teleportPosition.y = bottomWallPosition.y;
            }
            else if (collision.gameObject.name == "BottomWall")
            {
                teleportPosition.y = topWallPosition.y;
            }
            else if (collision.gameObject.name == "LeftWall")
            {
                teleportPosition.x = rightWallPosition.x;
            }
            else if (collision.gameObject.name == "RightWall")
            {
                teleportPosition.x = leftWallPosition.x;
            }

            transform.position = teleportPosition;
            Movement();
        }
    }

    void AddTail(Vector2 headPosition)
    {
        Vector2 spawnPosition;
        if (tail.Count == 0)
        {
            spawnPosition = headPosition;
        }
        else
        {
            spawnPosition = tail[tail.Count - 1].position;
        }

        GameObject newTail = Instantiate(tailPrefab, spawnPosition, Quaternion.identity);
        tail.Add(newTail.transform);
    }

    void UpdateTailPosition(Vector2 headPosition)
    {
        for (int i = tail.Count - 1; i > 0; i--)
        {
            tail[i].position = tail[i - 1].position;
        }
        if (tail.Count != 0)
        {
            tail[0].position = headPosition;
        }
    }
}
