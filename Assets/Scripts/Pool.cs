using System.Collections.Generic;
using UnityEngine;

public class Pool<TItem>: MonoBehaviour where TItem : PoolItem<TItem>
{
    [SerializeField] private TItem model;
    [SerializeField] private int maxSize;
    [SerializeField] private int initSize;

    private readonly Stack<TItem> pool = new();

    public void InitPool(TItem _model, int _initSize, int _maxSize)
    {
        model = _model;
        initSize = _initSize;
        maxSize = _maxSize;

        if (initSize > maxSize)
        {
            (initSize, maxSize) = (maxSize, initSize); // swap
        }

        FillPool(initSize);
    }

    public TItem GetItem()
    {
        if (pool.Count == 0) return CreateItem();
        else
        {
            var nextItem =  pool.Pop();
            nextItem.gameObject.SetActive(true);
            return nextItem;
        } 
    }

    public void RetrieveItem(TItem item)
    {
        if (pool.Count < maxSize)
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
        size = Mathf.Min(size, maxSize);

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
