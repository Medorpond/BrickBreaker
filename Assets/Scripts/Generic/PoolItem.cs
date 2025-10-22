using UnityEngine;

public class PoolItem<TItem> : MonoBehaviour, IRetrievable where TItem : PoolItem<TItem>
{
    private Pool<TItem> pool;

    public void SetPool(Pool<TItem> _pool) => pool = _pool;
    public virtual void Retrieve() => pool.RetrieveItem(this as TItem);

}
