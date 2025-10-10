using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ItemShell : PoolItem<ItemShell>
{
    [HideInInspector] public ItemData data;
    [SerializeField] private float dropSpeed;

    private Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + dropSpeed * Time.fixedDeltaTime * Vector2.down);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (data && collision.TryGetComponent<PlayerController>(out var _))
        {
            ItemManager.Instance.InvokeEvent(data);
            data = null;
            Retrieve();
        }
    }
}
