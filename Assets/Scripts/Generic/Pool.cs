using System.Collections.Generic;
using UnityEngine;

public class Pool<TItem>: MonoBehaviour where TItem : PoolItem<TItem>
{
    [Header("Pool Data")]
    [SerializeField] private TItem model;
    [SerializeField] private int initSize;
    [field: SerializeField] public int MaxSize { get; private set; }
    [Space(10f)]

    private readonly Stack<TItem> pool = new();

    private void Awake()
    {
        if (initSize > MaxSize) { 
            (initSize, MaxSize) = (MaxSize, initSize); // Swap
            Debug.LogWarning($"InitSize({initSize}) cannot be Larger than MaxSize({MaxSize}). Value adjusted.");
        }

        FillPool(initSize);
    }

    public TItem GetItem(Vector3 initPos)
    {
        if (pool.Count == 0) return CreateItem();
        else
        {
            var nextItem =  pool.Pop();
            nextItem.transform.position = initPos;
            nextItem.gameObject.SetActive(true);
            return nextItem;
        } 
    }

    public void RetrieveItem(TItem item)
    {
        if (pool.Count < MaxSize)
        {
            item.gameObject.SetActive(false);
            pool.Push(item);
        }
        else
        {
            Destroy(item.gameObject);
        }
    }

    public void FillPool(int size)
    {
        size = Mathf.Min(size, MaxSize);

        for (int i = 0; i < size; i++)
        {
            TItem item = CreateItem();
            item.gameObject.SetActive(false);
            pool.Push(item);
        }
    }

    private TItem CreateItem()
    {
        TItem newItem = Instantiate(model);
        newItem.SetPool(this);
        return newItem;
    }
}
