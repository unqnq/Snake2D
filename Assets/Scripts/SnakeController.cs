using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab, tailPrefab;
    private GameObject food;
    [SerializeField] private float stepRate = 0.3f;
    private Vector3 move = new Vector2(-1f, 0), nextMove;
    private List<Transform> tail = new List<Transform>();
    private bool isFoodEating = false;
    public bool spawnFood = true;
    private float topOffset = 225f;
    private UIController uiController;

    private ColorData colorData;
    private DifficultyData difficultyData;
    void Start()
    {
        // topWallPosition = GameObject.Find("TopWall").transform.position;
        // bottomWallPosition = GameObject.Find("BottomWall").transform.position;
        // leftWallPosition = GameObject.Find("LeftWall").transform.position;
        // rightWallPosition = GameObject.Find("RightWall").transform.position;
        uiController = GameObject.Find("UIController")?.GetComponent<UIController>();
        colorData = Resources.Load<ColorData>("ColorData");

        if (colorData != null)
        {
            colorData.currrentColor = SnakeColorManager.LoadColor(colorData.currrentColor);
            GetComponent<SpriteRenderer>().color = colorData.currrentColor;
        }

        difficultyData = Resources.Load<DifficultyData>("DifficultyData");

        if (difficultyData != null)
        {
#if UNITY_WEBGL
            stepRate = PlayerPrefs.GetFloat("StepRate", difficultyData.stepRate);
            difficultyData.startingTailLength = PlayerPrefs.GetInt("TailLength", difficultyData.startingTailLength);
#else
            stepRate = difficultyData.stepRate;
#endif
        }
        nextMove = move;
        InvokeRepeating("Movement", 0.1f, stepRate);
        SpawnFood();

        for (int i = 1; i <= difficultyData.startingTailLength; i++)
        {
            AddTail(transform.position - move * i);
        }
    }

    void Update()
    {
        if (colorData != null)
        {
            GetComponent<SpriteRenderer>().color = colorData.currrentColor;
        }

        if (WebGLUtils.IsMobile() || Application.isMobilePlatform)
        {
            HandleTouchInput();
        }

        CheckScreenBounds();
    }

    void SpawnFood()
    {
        if (spawnFood)
        {


            Vector3 spawnPosition;
            bool canSpawn;

            Vector2 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector2 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height - topOffset, 0f));

            int minX = Mathf.CeilToInt(min.x);
            int maxX = Mathf.FloorToInt(max.x);
            int minY = Mathf.CeilToInt(min.y);
            int maxY = Mathf.FloorToInt(max.y);

            do
            {
                canSpawn = true;

                int x = Random.Range(minX + 1, maxX);
                int y = Random.Range(minY + 1, maxY);
                spawnPosition = new Vector2(x, y);

                if (spawnPosition == transform.position)
                {
                    canSpawn = false;
                }

                if (canSpawn)
                {
                    foreach (Transform segment in tail)
                    {
                        if (spawnPosition == segment.position)
                        {
                            canSpawn = false;
                            break;
                        }
                    }
                }
            }
            while (!canSpawn); //поки не можна спавнити їжу - повторюємо цикл

            food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void Movement()
    {
        move = nextMove;
        Vector3 currentPosition = transform.position;
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
        Vector3 newDirection = context.ReadValue<Vector2>();
        if (newDirection == Vector3.zero) return;
        if (newDirection == -move) return;
        if (Mathf.Abs(newDirection.x) > 0 && Mathf.Abs(newDirection.y) > 0) return; // ігнорувати діагональ 
        nextMove = newDirection;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Moved)
            {
                Vector3 newDirection = Vector3.zero;

                if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                {
                    // Горизонтальний свайп
                    newDirection = touch.deltaPosition.x > 0f ? Vector3.right : Vector3.left;
                }
                else if (Mathf.Abs(touch.deltaPosition.y) > Mathf.Abs(touch.deltaPosition.x))
                {
                    // Вертикальний свайп
                    newDirection = touch.deltaPosition.y > 0f ? Vector3.up : Vector3.down;
                }

                if (newDirection != Vector3.zero && newDirection != -move)
                {
                    nextMove = newDirection;
                }
            }
        }
    }

    private void CheckScreenBounds()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 newPosition = transform.position;

        if (screenPos.x < 0f)
        {
            newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        }
        else if (screenPos.x > Screen.width)
        {
            newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        }
        else if (screenPos.y < 0f)
        {
            newPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - topOffset, 0)).y;
        }
        else if (screenPos.y > Screen.height - topOffset)
        {
            newPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        }
        if (newPosition != transform.position)
        {
            transform.position = newPosition;
            Movement();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            Destroy(collision.gameObject);
            isFoodEating = true;
            uiController?.UpdateScore();
            SpawnFood();
        }

        else if (collision.CompareTag("Snake"))
        {
            uiController?.GameOver();
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

    public void UpdateSnakeColor(Color newColor)
    {
        GetComponent<SpriteRenderer>().color = newColor;
        if (colorData != null)
        {
            colorData.currrentColor = newColor;
        }
        SnakeColorManager.SaveColor(newColor);
    }
}
