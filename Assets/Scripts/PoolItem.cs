using UnityEngine;

public class PoolItem<TItem> : MonoBehaviour where TItem : PoolItem<TItem>
{
    private Pool<TItem> pool;

    public void SetPool(Pool<TItem> _pool) => pool = _pool;
    public void Retrieve() => pool.RetrieveItem((TItem)this);

}
