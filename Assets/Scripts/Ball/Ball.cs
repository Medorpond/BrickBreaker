using System.Collections;
using UnityEngine;

public class Ball : PoolItem<Ball>, IItemEffectable
{
    BallManager bm;

    Rigidbody2D rbody;
    [SerializeField, Min(0)] private float minSpeed;
    [SerializeField, Min(0)] private float maxSpeed;
    [SerializeField, Min(0)] private float minEscapeSpeed;
    [SerializeField, Min(0)] private float epsilonSpeed;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        bm = BallManager.Instance;
    }


    private void OnEnable()
    {
        bm.OnBallItemTrigger += OnRecieveItem;
    }
    private void OnDisable()
    {
        bm.OnBallItemTrigger-= OnRecieveItem;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnsureMinAxisSpeed();
        ClampSpeed();
        PlayCollisionEffect(collision);
    }

    private void PlayCollisionEffect(Collision2D collision)
    {
        VFXItem effect = VFXManager.Instance.GetEffect(VFXManager.EffectType.Bounce, collision.contacts[0].point);

        Vector2 effectDir = collision.relativeVelocity.normalized * -1;
        float angle = Mathf.Atan2(effectDir.y, effectDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90f);
        effect.PlayAnim(rotation);
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
            if(bm.TryGetBall(this.transform.position, out var newBall))
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
        bm.RetrieveBall(this);
    }

    public void OnRecieveItem(ItemData data)
    {
        data.ItemEffect(gameObject);
    }

    private void EnsureMinAxisSpeed()
    {
        float x = rbody.linearVelocityX;
        float y = rbody.linearVelocityY;
        
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

    private void ClampSpeed()
    {
        var speed = rbody.linearVelocity.magnitude;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        SetVelocity(speed);
    }
}
