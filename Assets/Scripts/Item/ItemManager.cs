using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : BaseManager<ItemManager>
{

    [SerializeField] private ItemPool poolModel;
    private ItemPool pool;

    public List<ItemData> Items;

    #region Events
    // public event Action<ItemData> XXXItem;
    public event Action<ItemData> BallItem;
    public event Action<ItemData> PaddleItem;
    public event Action<ItemData> BrickItem;
    public event Action<ItemData> GlobalItem;
    #endregion

    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();
        InitPool();
    }
    #endregion

    void InitPool() => pool = Instantiate(poolModel, transform);


    public void InvokeEvent(ItemData data)
    {
        switch (data.TargetType)
        {
            case ItemTarget.Paddle:
                PaddleItem?.Invoke(data);
                break;
            case ItemTarget.Ball:
                BallItem?.Invoke(data);
                break;
            case ItemTarget.Brick:
                BrickItem?.Invoke(data);
                break;
            default:
                GlobalItem?.Invoke(data);
                break;
        }
    }

    public void TrySpawnRandomItem(Vector3 initPos)
    {
        if (Items.Count == 0)
        {
            Debug.LogWarning("There's No Item Set!");
            return;
        }

        var item = pool.GetItem(initPos);

        int index = UnityEngine.Random.Range(0, Items.Count);
        item.data = Items[index];
    }
}

public enum ItemTarget { Paddle, Ball, Brick, Global };