using System;
using UnityEngine;

public class BallManager : BaseManager<BallManager>
{
    [SerializeField] private BallPool poolModel;
    private BallPool pool;

    public event Action<ItemData> OnBallItemTrigger;

    private int ballCount = 0;

    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }

    private void Start()
    {
        ItemManager.Instance.BallItem += RecieveBallItem;
    }

    private void OnDestroy()
    {
        ItemManager.Instance.BallItem -= RecieveBallItem;
    }
    #endregion

    void InitPool() => pool = Instantiate(poolModel, transform);

    public Ball GetBall(Vector3 initPos)
    {
        ballCount++;
        return pool.GetItem(initPos);
    }

    public bool TryGetBall(Vector3 initPos, out Ball ball)
    {
        if(ballCount >= pool.MaxSize)
        {
            ball = null;
            return false;
        }
        else
        {
            ball = GetBall(initPos);
            return true;
        }
    }

    public void RetrieveBall(Ball ball)
    {
        ballCount--;
        pool.RetrieveItem(ball);

        if (ballCount <= 0) StageManager.Instance.OnAllBallLost();
    }

    void RecieveBallItem(ItemData item)
    {
        OnBallItemTrigger.Invoke(item);
    }
}
