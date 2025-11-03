using System;
using UnityEngine;

public class BallManager : BaseManager<BallManager>
{
    [SerializeField] private BallPool poolModel;
    private BallPool pool;

    private ItemManager itemManager;
    public event Action<ItemData> OnBallItemTrigger;
    public event Action OnAllBallLost;

    private int ballCount = 0;

    #region LifeCycle
    protected override void Awake()
    {
        base.Awake();
        itemManager = ItemManager.Instance;
        InitPool();
    }

    private void Start()
    {
        itemManager.BallItem += RecieveBallItem;
    }

    private void OnDestroy()
    {
        itemManager.BallItem -= RecieveBallItem;
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
        
        if (ballCount <= 0) OnAllBallLost.Invoke();
    }

    void RecieveBallItem(ItemData item)
    {
        OnBallItemTrigger.Invoke(item);
    }
}
