using System.Collections;
using UnityEngine;

public class Ball : PoolItem<Ball>, IItemEffectable
{
    Rigidbody2D rbody;
    [SerializeField, Min(0)] private float minSpeed;
    [SerializeField, Min(0)] private float maxSpeed;
    [SerializeField, Min(0)] private float minEscapeSpeed;
    [SerializeField, Min(0)] private float epsilonSpeed;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        BallManager.Instance.OnBallItemTrigger += OnRecieveItem;
    }
    private void OnDisable()
    {
        BallManager.Instance.OnBallItemTrigger-= OnRecieveItem;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnsureMinAxisSpeed();
    }

    public void Split(int num)
    {
        if (num <= 1) return;

        Vector2 OriginalDir = rbody.linearVelocity.normalized;
        float originalSpeed = rbody.linearVelocity.magnitude;

        if(Mathf.Abs(originalSpeed) < epsilonSpeed)
        {
            return;
        }

        float angleStep = 180f / (num + 1);
        float startAngle = -90f;

        for(int i = 1; i <= num; i++)
        {
            if(BallManager.Instance.TryGetBall(this.transform.position, out var newBall))
            {
                float currentAngle = startAngle + (i * angleStep);

                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
                Vector2 newDir = rotation * OriginalDir;

                newBall.SetVelocity(newDir * originalSpeed);
            }
        }

        Retrieve();
    }

    public void Push(Vector2 dir, float power)
    {
        rbody.AddForce(dir * power, ForceMode2D.Impulse);
    }

    public void ShiftSpeedByRate(float shiftRate)
    {
        float curSpeed = rbody.linearVelocity.magnitude;
        float nextSpeed = curSpeed * (1 + shiftRate);
        nextSpeed = Mathf.Min(maxSpeed, nextSpeed);
        nextSpeed = Mathf.Max(minSpeed, nextSpeed);

        SetVelocity(nextSpeed);
    }

    private void SetVelocity(Vector2 v)
    {
        rbody.linearVelocity = v;
    }


    private void SetVelocity(float v)
    {
        Vector2 dir = rbody.linearVelocity.normalized;
        rbody.linearVelocity = dir * v;
    }

    public override void Retrieve()
    {
        BallManager.Instance.RetrieveBall(this);
    }

    public void OnRecieveItem(ItemData data)
    {
        data.ItemEffect(gameObject);
    }

    private void EnsureMinAxisSpeed()
    {
        float x = rbody.linearVelocityX;
        Debug.Log(x);
        float y = rbody.linearVelocityY;
        Debug.Log(x);
        if (Mathf.Abs(x) < epsilonSpeed)
        {
            float sign = (Random.value > 0.5f ? 1f : -1f);

            rbody.linearVelocityX = sign * minEscapeSpeed;
        }

        if (Mathf.Abs(y) < epsilonSpeed)
        {
            float sign = (Random.value > 0.5f ? 1f : -1f);

            rbody.linearVelocityY = sign * minEscapeSpeed;
        }
    }
}
