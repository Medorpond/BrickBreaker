using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour, IItemEffectable
{
    Rigidbody2D rbody;

    [Header("Paddle Configs")]
    [SerializeField, Min(0)] private float lerpSpeed = 20f;
    //[SerializeField, Min(0)] private float widthRatio = 1f;
    [SerializeField, Min(0)] private float launchPower = 5f;
    [SerializeField, Min(0)] private float ballOffset = 0.5f;
    [SerializeField, Min(0)] private float moveLimit;

    [Header("Paddle Width Set")]
    public List<float> paddleWidthSteps;
    public int paddleWidthLevel;

    private int maxWidthLevel;

    SpriteRenderer spriteRenderer;
    BoxCollider2D paddleCollider;

    //TESTONLY
    [SerializeField, Min(0)] private int life = 3;

    private float prevPosX = 0f;
    private float currentSpeed;
    public float ballInfluence;
    //TESTONLY


    private bool isLoaded = false;
    private Ball ball;

    //====================================================
    #region Lifecycle

    private void Awake()
    {
        maxWidthLevel = paddleWidthSteps.Count - 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        paddleCollider = GetComponent<BoxCollider2D>();

        ChangeWidthByLevel(paddleWidthLevel);
    }

    private void OnEnable()
    {
        BallManager.Instance.OnAllBallLost += OnAllBallLost;
        ItemManager.Instance.PaddleItem += OnRecieveItem;
    }

    private void Start()
    {
        Reload();
    }

    private void Update()
    {
        GetUserInput();


        //TESTONLY
        if (Input.GetKeyDown(KeyCode.D))
        {
            DecreaseWidth();
        }else if (Input.GetKeyDown(KeyCode.F))
        {
            IncreaseWidth();
        }
        //TESTONLY
    }
    private void FixedUpdate()
    {
        Move();
        currentSpeed = (transform.position.x - prevPosX) / Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        MarkCurrentPosX();
    }

    private void OnDisable()
    {
        BallManager.Instance.OnAllBallLost -= OnAllBallLost;
        ItemManager.Instance.PaddleItem -= OnRecieveItem;
    }
    #endregion

    //====================================================

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.TryGetComponent<Ball>(out var ball))
    //    {
    //        InfluenceBall(ball);
    //    }
    //}

    //====================================================

    private void MarkCurrentPosX() => prevPosX = transform.position.x;

    private void InfluenceBall(Ball ball)
    {
        float dirX = Mathf.Sign(currentSpeed);
        Vector2 Dir = new Vector2(dirX, 0);

        currentSpeed *= ballInfluence;

        ball.Push(Dir, currentSpeed);
    }

    public void Move()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPos = transform.position;
        targetPos.x = Mathf.Clamp(targetPos.x, -moveLimit, moveLimit);
        targetPos.y = currentPos.y;
        transform.position = Vector2.Lerp(currentPos, targetPos, lerpSpeed* Time.fixedDeltaTime);
    }

    private void OnAllBallLost()
    {
        life--;
        if (life <= 0) StageManager.Instance.GameOver(false);
        else Reload();
    }

    private void Reload()
    {
        Vector3 initPos = transform.position;
        initPos.y += ballOffset;
        ball = BallManager.Instance.GetBall(initPos);
        StartCoroutine(LaunchReady());
    }

    IEnumerator LaunchReady()
    {
        if (!ball) yield break;

        isLoaded = true;
        while (isLoaded)
        {
            ball.transform.position = new Vector2(transform.position.x, transform.position.y + ballOffset);
            yield return null;
        }
    }

    private void Launch()
    {
        if (ball && ball.TryGetComponent<Rigidbody2D>(out var ballRb))
        {
            ballRb.AddForce(Vector2.up * launchPower, ForceMode2D.Impulse);
        }
    }

    private void GetUserInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnUserInput();
        }
    }

    private void OnUserInput()
    {
        if (isLoaded)
        {
            Launch();
            isLoaded = false;
            ball = null;
        }
        else
        {
            // 아이템 사용 등 (확장 기능)
        }
    }

    public void IncreaseWidth() => ChangeWidthLevelByDiff(1);

    public void DecreaseWidth() => ChangeWidthLevelByDiff(-1);

    public void ChangeWidthLevelByDiff(int diff)
    {
        int newLevel = paddleWidthLevel += diff;
        ChangeWidthByLevel(newLevel);
    }

    public void ChangeWidthByLevel(int level)
    {
        paddleWidthLevel = Mathf.Clamp(level, 0, maxWidthLevel);
        SetWidth(paddleWidthSteps[paddleWidthLevel]);
    }

    public void SetWidth(float width)
    {
        float y = spriteRenderer.size.y;
        spriteRenderer.size = new Vector2(width, y);
        paddleCollider.size = new Vector2(width, y);
    }

    public void OnRecieveItem(ItemData data)
    {
        data.ItemEffect(gameObject);
    }
}
