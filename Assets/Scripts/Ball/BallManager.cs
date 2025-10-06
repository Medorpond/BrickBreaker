using System;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [Header("Pool Data")]
    [SerializeField] private Ball model;
    [SerializeField, Min(0)] private int initSize;
    [SerializeField, Min(0)] private int maxSize;

    [HideInInspector] public BallPool pool;

    private static BallManager _instance;
    public static BallManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<BallManager>();

                if(_instance == null)
                {
                    Debug.LogError("No Manager Found on Scene");
                }
            }

            return _instance;
        }
    }

    private int ballCount = 0;
    public event Action OnAllBallLost;

    #region LifeCycle
    private void Awake()
    {
        InitInstance();
        InitPool();
    }
    #endregion

    void InitPool()
    {
        GameObject poolObj = new GameObject("Ball Pool");
        poolObj.transform.SetParent(this.transform);
        pool = poolObj.AddComponent<BallPool>();

        Debug.Log($"{pool}");

        pool.InitPool(model, initSize, maxSize);
    }

    void InitInstance()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    public Ball GetBall()
    {
        ballCount++;
        return pool.GetItem();
    }

    public void RetrieveBall(Ball ball)
    {
        ballCount--;
        pool.RetrieveItem(ball);

        if (ballCount <= 0) OnAllBallLost?.Invoke();
    }
}
