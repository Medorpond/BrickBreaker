using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour, IItemEffectable
{
    Rigidbody2D rbody;

    [Header("Paddle Configs")]
    [SerializeField, Min(0)] private float lerpSpeed = 20f;
    //[SerializeField, Min(0)] private float widthRatio = 1f;
    [SerializeField, Min(0)] private float launchPower = 5f;
    [SerializeField, Min(0)] private float ballOffset = 0.5f;
    [SerializeField, Min(0)] private float moveLimit;
    

    private bool isLoaded = false;
    private Ball ball;

    //====================================================
    #region Lifecycle

    private void OnEnable()
    {
        BallManager.Instance.OnAllBallLost += Reload;
        ItemManager.Instance.PaddleItem += OnRecieveItem;
    }

    private void Start()
    {
        Reload();
    }

    private void Update()
    {
        GetUserInput();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void OnDisable()
    {
        BallManager.Instance.OnAllBallLost -= Reload;
        ItemManager.Instance.PaddleItem -= OnRecieveItem;
    }
    #endregion

    public void Move()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentPos = transform.position;
        targetPos.x = Mathf.Clamp(targetPos.x, -moveLimit, moveLimit);
        targetPos.y = currentPos.y;
        transform.position = Vector2.Lerp(currentPos, targetPos, lerpSpeed* Time.fixedDeltaTime);
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
        if (Input.GetKeyUp(KeyCode.Space))
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

    public void OnRecieveItem(ItemData data)
    {
        data.ItemEffect(gameObject);
    }
}
