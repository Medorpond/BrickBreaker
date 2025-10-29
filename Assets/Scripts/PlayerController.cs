using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour, IItemEffectable
{
    StageManager sm;
    ItemManager im;
    BallManager bm;

    Rigidbody2D rbody;

    [Header("SFXs")]
    [SerializeField] AudioClip boundSound;

    [Header("Paddle Configs")]
    [SerializeField, Min(0)] private float moveSpeed = 20f;
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

    public float ballInfluence;


    private Vector2 moveDir;
    private bool isLoaded = false;
    private Ball ball;

    //====================================================
    #region Lifecycle

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();

        sm = StageManager.Instance;
        im = ItemManager.Instance;
        bm = BallManager.Instance;


        maxWidthLevel = paddleWidthSteps.Count - 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        paddleCollider = GetComponent<BoxCollider2D>();

        ChangeWidthByLevel(paddleWidthLevel);
    }

    private void OnEnable()
    {
        sm.OnBallLost += OnAllBallLost;
        im.PaddleItem += OnRecieveItem;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDisable()
    {
        sm.OnBallLost += OnAllBallLost;
        im.PaddleItem -= OnRecieveItem;
    }
    #endregion


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball))
        {
            InfluenceBall(ball);
            SoundManager.Instance.PlaySfx(boundSound);
        }
    }

    private void InfluenceBall(Ball ball) 
    {
        ball.Push(moveDir, moveSpeed * ballInfluence);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        float inputX = ctx.ReadValue<Vector2>().x;
        moveDir = new Vector2(inputX, 0);
    }

    private void Move()
    {
        rbody.MovePosition(rbody.position + (moveSpeed * moveDir * Time.fixedDeltaTime));
    }

    private void OnAllBallLost(int life)
    {
        if (life > 0) Reload();
    }

    private void Reload()
    {
        Vector3 initPos = transform.position;
        initPos.y += ballOffset;
        ball = bm.GetBall(initPos);
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

    public void OnLaunch(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

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
